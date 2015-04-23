namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Serializers
{
    using Newtonsoft.Json;

    internal class JsonSettings
    {
        public static void Initialize()
        {
            //JsonConvert.DefaultSettings = () => new JsonSerializerSettings {};
        }
    }
}
