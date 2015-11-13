namespace SFA.Apprenticeship.Api.AvService.ErrorHandlers
{
    using System;
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    public class CustomErrorHandler : ErrorHandlerBase, IErrorHandler, IServiceBehavior
    {
        private const string DefaultPolicyName = "ApplicationTier";

        public CustomErrorHandler()
            : this(DefaultPolicyName)
        {
        }

        public CustomErrorHandler(string policyName) : base(policyName)
        {
        }

        // ReSharper disable once RedundantAssignment
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            // Shield the unknown exception.
            var faultException = new FaultException(error.Message);
            var messageFault = faultException.CreateMessageFault();

            fault = Message.CreateMessage(version, messageFault, faultException.Action);
        }

        public bool HandleError(Exception error)
        {
            // ReSharper disable once RedundantBaseQualifier
            base.HandleException(error);
            return false;
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // Empty implementation.
        }

        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
            // Empty implementation.
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            {
                IErrorHandler errorHandler = new CustomErrorHandler();

                foreach (var channelDispatcher in serviceHostBase.ChannelDispatchers)
                {
                    (channelDispatcher as ChannelDispatcher)?.ErrorHandlers.Add(errorHandler);
                }
            }
        }
    }
}
