namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Domain.Interfaces.Configuration;
    using EasyNetQ;
    using EasyNetQ.Management.Client;
    using RabbitMq.Configuration;

    public class CheckRabbitMessageQueue : IMonitorTask
    {
        private readonly RabbitConfiguration _rabbitConfiguration;
        private readonly ILogService _logger;
        public CheckRabbitMessageQueue(IConfigurationService configurationService, ILogService logger)
        {
            _rabbitConfiguration = configurationService.Get<RabbitConfiguration>(RabbitConfiguration.ConfigurationName);
            _logger = logger;
        }

        public string TaskName
        {
            get { return "Check rabbit message queues"; }
        }

        public void Run()
        {
            CheckRabbitHealth(_rabbitConfiguration.MessagingHost);
            CheckRabbitHealth(_rabbitConfiguration.LoggingHost);
        }

        private void CheckRabbitHealth(RabbitHost rabbitConfiguration)
        {
            var rabbitClient = new ManagementClient(rabbitConfiguration.HostName, rabbitConfiguration.UserName, rabbitConfiguration.Password);

            CheckRabbitQueueConnectivity(rabbitConfiguration.ConnectionString);
            CheckRabbitNodeCount(rabbitClient, rabbitConfiguration);
            CheckRabbitQueueWarningLimit(rabbitClient, rabbitConfiguration);
        }

        private void CheckRabbitQueueConnectivity(string connectionString)
        {
            try
            {
                using (var rabbitBus = RabbitHutch.CreateBus(connectionString))
                {
                    rabbitBus.Publish(new MonitorMessage());
                }
            }
            catch (Exception ex)
            {
                var host = connectionString.Split(new[] { ';' }).First();
                LogError(host, ex);
            }
        }

        private void CheckRabbitNodeCount(ManagementClient managementClient, RabbitHost rabbitConfiguration)
        {
            try
            {
                var nodes = managementClient.GetNodes();
                int nodeCount = nodes.Count(n => n.Running);
                if (nodeCount != rabbitConfiguration.NodeCount)
                {
                    _logger.Error("Node count in {0} is incorrect. Expecting {1} but was {2}", managementClient.HostUrl,
                        rabbitConfiguration.NodeCount,
                        nodeCount);
                }
            }
            catch (Exception ex)
            {
                LogError(managementClient.HostUrl, ex);
            }
        }

        private void CheckRabbitQueueWarningLimit(ManagementClient managementClient, RabbitHost rabbitConfiguration)
        {
            try
            {
                var rabbitQueues = managementClient.GetQueues();
                var defaultQueueWarningLimit = rabbitConfiguration.DefaultQueueWarningLimit;
                var queueWarningLimits = rabbitConfiguration.QueueWarningLimits.ToList();
                var checkedQueueWarningLimits = new List<QueueWarningLimit>(queueWarningLimits.Count);

                foreach (var rabbitQueue in rabbitQueues)
                {
                    var queueWarningLimit = queueWarningLimits.SingleOrDefault(qwl => rabbitQueue.Name.EndsWith(qwl.NameEndsWith));
                    int messageLimit;
                    if (queueWarningLimit == null)
                    {
                        messageLimit = defaultQueueWarningLimit;
                    }
                    else
                    {
                        messageLimit = queueWarningLimit.Limit;
                        checkedQueueWarningLimits.Add(queueWarningLimit);
                    }

                    if (rabbitQueue.Messages > messageLimit)
                    {
                        _logger.Warn(
                            "Queue '{0}' on node '{1}' has exceeded the queue item limit threshold of {2} and currently has {3} messages queued, please check queue is processing as expected",
                            rabbitQueue.Name, rabbitQueue.Node, messageLimit, rabbitQueue.Messages);
                    }
                }

                if (checkedQueueWarningLimits.Count != queueWarningLimits.Count)
                {
                    var missingQueues = queueWarningLimits.Except(checkedQueueWarningLimits);
                    foreach (var missingQueue in missingQueues)
                    {
                        _logger.Error("Queue ending with '{0}' appears to be missing!", missingQueue.NameEndsWith);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(managementClient.HostUrl, ex);
            }            
        }

        private void LogError(string host, Exception exception)
        {
            var message = string.Format("Error while connecting to Rabbit queue on {0}", host);
            _logger.Error(message, exception);
        }

        internal class MonitorMessage
        {
            public string Text
            {
                get { return "Monitor test message"; }
            }
        }
    }
}