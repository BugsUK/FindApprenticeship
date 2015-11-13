namespace SFA.Apprenticeship.Api.AvService.ErrorHandlers
{
    using System;
    using System.ServiceModel.Configuration;

    public class CustomErrorHandlerBehavior : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new CustomErrorHandler();
        }

        public override Type BehaviorType => typeof(CustomErrorHandler);
    }
}
