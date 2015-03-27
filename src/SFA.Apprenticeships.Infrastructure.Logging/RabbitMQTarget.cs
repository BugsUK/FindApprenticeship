﻿namespace SFA.Apprenticeships.Infrastructure.Logging
{
    using System;
    using System.Configuration;
    using System.Text;
    using Common.Configuration;
    using Common.IoC;
    using Domain.Interfaces.Configuration;
    using EasyNetQ;
    using EasyNetQ.Topology;
    using IoC;
    using Layouts;
    using NLog;
    using NLog.Common;
    using NLog.Targets;
    using RabbitMq.Configuration;
    using StructureMap;
    using ConfigurationManager = Common.Configuration.ConfigurationManager;

    /// <summary>
    /// A RabbitMQ-target for NLog that must use a JsonLayout!
    /// </summary>
    [Target("RabbitMQTarget")]
    public class RabbitMQTarget : TargetWithLayout
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private static RabbitHost _rabbitMqHostHostConfig;
        private string _appId = "SFA.Apprenticeships.App";
        private IAdvancedBus _bus;
        private IExchange _exchange;

        private string _exchangeName = "app-logging";
        private string _exchangeType = EasyNetQ.Topology.ExchangeType.Topic;
        private string _queueName = "NLog";
        private string _routingKeyConst = "{0}";

        public RabbitMQTarget()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<CommonRegistry>();
            });

            var configService = container.GetInstance<IConfigurationService>();
            var rabbitConfig = configService.Get<RabbitConfiguration>();
            _rabbitMqHostHostConfig = rabbitConfig.LoggingHost;
        }

        #region Target Configuration

        public string QueueName
        {
            get { return _queueName; }
            set { _queueName = value; }
        }

        public string ExchangeName
        {
            get { return _exchangeName; }
            set { _exchangeName = value; }
        }

        public string ExchangeType
        {
            get { return _exchangeType; }
            set
            {
                switch (value)
                {
                    case EasyNetQ.Topology.ExchangeType.Topic:
                    case EasyNetQ.Topology.ExchangeType.Fanout:
                    case EasyNetQ.Topology.ExchangeType.Header:
                    case EasyNetQ.Topology.ExchangeType.Direct:
                        _exchangeType = value;
                        break;
                    default:
                        throw new ConfigurationErrorsException(
                            "ExchangeType not valid ExchangeType, see EasyNetQ.Topology.ExchangeType for valid values");
                }
            }
        }

        public string RoutingKey
        {
            get { return _routingKeyConst; }
            set { _routingKeyConst = value; }
        }

        public string AppId
        {
            get { return _appId; }
            set { _appId = value; }
        }

        private IAdvancedBus Bus
        {
            get
            {
                if (_bus != null)
                {
                    return _bus;
                }

                try
                {
                    if (_rabbitMqHostHostConfig.OutputEasyNetQLogsToNLogInternal)
                    {
                        var logger = new EasyNetNLogInternalLogger();
                        _bus =
                            RabbitHutch.CreateBus(_rabbitMqHostHostConfig.HostName, _rabbitMqHostHostConfig.Port,
                                _rabbitMqHostHostConfig.VirtualHost, _rabbitMqHostHostConfig.UserName,
                                _rabbitMqHostHostConfig.Password, _rabbitMqHostHostConfig.HeartBeatSeconds,
                                reg => reg.Register<IEasyNetQLogger>(log => logger)).Advanced;
                    }
                    else
                    {
                        _bus =
                            RabbitHutch.CreateBus(_rabbitMqHostHostConfig.HostName, _rabbitMqHostHostConfig.Port,
                                _rabbitMqHostHostConfig.VirtualHost, _rabbitMqHostHostConfig.UserName,
                                _rabbitMqHostHostConfig.Password, _rabbitMqHostHostConfig.HeartBeatSeconds, reg => { })
                                .Advanced;
                    }

                    // This will create the exchange and queue and bind them if they doesn't already exist 
                    // Change passive from false to true on both calls if it needs to be pre-declared.
                    _exchange = _bus.ExchangeDeclare(ExchangeName, ExchangeType, false, _rabbitMqHostHostConfig.Durable);
                    var queue = _bus.QueueDeclare(QueueName, false);
                    _bus.Bind(_exchange, queue, GetRoutingKey("*"));
                }
                catch (Exception ex)
                {
                    InternalLogger.Error("Failed setting up rabbit connections/bindings: Error:{0}{1}Stacktrace: {2}", ex.Message, Environment.NewLine, ex.StackTrace);
                    throw;
                }

                return _bus;
            }
        }

        #endregion

        protected override void Write(LogEventInfo logEvent)
        {
            var message = GetMessage(logEvent);
            var routingKey = GetRoutingKey(logEvent.Level.Name);

            var properties = new MessageProperties
            {
                AppId = AppId,
                ContentEncoding = "utf8",
                ContentType = "application/json",
                Timestamp = GetEpochTimeStamp(logEvent),
                UserId = _rabbitMqHostHostConfig.UserName,
                Type = "Log",
            };

            Bus.Publish(_exchange, routingKey, true, false, properties, message);
        }

        private string GetRoutingKey(string routeParam)
        {
            var routingKey = string.Format(RoutingKey, routeParam);
            return routingKey;
        }

        private byte[] GetMessage(LogEventInfo logEvent)
        {
            var jsonLayout = Layout as JsonLayout;
            if (jsonLayout == null)
            {
                throw new ConfigurationErrorsException("The layout configuration must use the JsonLayout");
            }

            var messageJson = Layout.Render(logEvent);

            return Encoding.UTF8.GetBytes(messageJson);
        }

        private static long GetEpochTimeStamp(LogEventInfo @event)
        {
            return Convert.ToInt64((@event.TimeStamp - Epoch).TotalSeconds);
        }

        /// <summary>
        /// Targets Dispose calls CloseTarget and therefore tidies up any resources open to RabbitMQ
        /// </summary>
        protected override void CloseTarget()
        {
            base.CloseTarget();
            if (Bus != null)
            {
                Bus.Dispose();
            }
        }

        internal class EasyNetNLogInternalLogger : IEasyNetQLogger
        {
            public void DebugWrite(string format, params object[] args)
            {
                InternalLogger.Debug(format, args);
            }

            public void InfoWrite(string format, params object[] args)
            {
                InternalLogger.Debug(format, args);
            }

            public void ErrorWrite(string format, params object[] args)
            {
                InternalLogger.Error(format, args);
            }

            public void ErrorWrite(Exception exception)
            {
                InternalLogger.Error(exception.ToString());
            }
        }
    }
}