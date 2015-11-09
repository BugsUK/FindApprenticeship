namespace SFA.Apprenticeship.Api.AvService.IoC
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    public class StructureMapInstanceProvider : IInstanceProvider
    {
        private readonly Type _type;

        public StructureMapInstanceProvider(Type type)
        {
            _type = type;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return IoC.Container.GetInstance(_type);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return IoC.Container.GetInstance(_type);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            // Empty implementation.
        }
    }
}
