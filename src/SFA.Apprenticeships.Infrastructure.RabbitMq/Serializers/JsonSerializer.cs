﻿namespace SFA.Apprenticeships.Infrastructure.RabbitMq.Serializers
{
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// Json serialiser for rabbit messages that support interface types.
    /// Note: we are using ServiceStack version 3.9.71 as this is stil offered under a BSD license
    /// and if we upgrade there is a cost associated with it.
    /// </summary>
    internal class JsonSerializer : EasyNetQ.ISerializer
    {
        private readonly EasyNetQ.ITypeNameSerializer _typeNameSerializer;

        public JsonSerializer(EasyNetQ.ITypeNameSerializer typeNameSerializer)
        {
            _typeNameSerializer = typeNameSerializer;
        }

        public byte[] MessageToBytes<T>(T message) where T : class
        {
            return GetBytes(JsonConvert.SerializeObject(message));
        }

        public T BytesToMessage<T>(byte[] bytes)
        {
            return JsonConvert.DeserializeObject<T>(GetString(bytes));
        }

        public object BytesToMessage(string typeName, byte[] bytes)
        {
            var type = _typeNameSerializer.DeSerialize(typeName);
            return JsonConvert.DeserializeObject(GetString(bytes), type);
        }

        private static string GetString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        private static byte[] GetBytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }
    }
}