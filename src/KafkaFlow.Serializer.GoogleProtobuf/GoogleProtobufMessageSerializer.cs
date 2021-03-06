﻿namespace KafkaFlow.Serializer.GoogleProtobuf
{
    using Google.Protobuf;
    using KafkaFlow.Serializer.ProtoBuf;
    using System;
    using KafkaFlow;

    /// <summary>
    /// A message serializer using Google.Protobuf library
    /// </summary>
    public class GoogleProtobufMessageSerializer : IMessageSerializer
    {
        /* Once the methods on ProtobufMessageSerializer are not virtual, we decided to use composition
         So if the class is not generated by google, as fallback, we are going to try to use the Serializer based on
         https://github.com/protobuf-net/protobuf-net */
        private readonly ProtobufMessageSerializer defaultSerializer = new ProtobufMessageSerializer();
        
        /// <summary>
        /// If the object is instance of Google.Protobuf.IMessage will use Google.Protobuf to Serialize.
        /// Otherwise, will use the Protobuf-Net library
        /// </summary>
        /// <param name="message">The message to be serialized</param>
        /// <returns>The serialized message</returns>
        public byte[] Serialize(object message)
        {
            if (message is IMessage)
            {
                IMessage protoMsg = (IMessage) message;
                return protoMsg.ToByteArray();
            }
            return defaultSerializer.Serialize(message);
        }
        
        /// <summary>
        /// If the Type implements the interface Google.Protobuf.IMessage will use Google.Protobuf to Deserialize.
        /// Otherwise, will use the Protobuf-Net library 
        /// </summary>
        /// <param name="data">The message to be deserialized</param>
        /// <param name="type">The destination type</param>
        /// <returns>The deserialized message</returns>
        public object Deserialize(byte[] data, Type type)
        {
            if(typeof(IMessage).IsAssignableFrom(type))
            {
                IMessage instance =(IMessage) Activator.CreateInstance(type);
                instance.MergeFrom(data);
                return instance;
            }
            return defaultSerializer.Deserialize(data, type);
        }
    }
}