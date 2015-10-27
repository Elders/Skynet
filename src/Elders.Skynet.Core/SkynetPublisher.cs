using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Elders.Skynet.Core
{
    public class SkynetPublisher : IPublisher
    {
        ConcurrentDictionary<Type, List<Type>> handlers;
        Func<Type, object> handlerFactory;
        public SkynetPublisher(Func<Type, object> handlerFactory)
        {
            this.handlerFactory = handlerFactory;
            handlers = new ConcurrentDictionary<Type, List<Type>>();
            var handlerTypes = Assembly.GetAssembly(typeof(SkynetPublisher)).GetTypes().Where(x => x.GetInterfaces().Select(y => y.IsGenericType && (y.GetGenericTypeDefinition() == typeof(IMessageHandler<>) || y.GetGenericTypeDefinition() == typeof(IMessageHandler<,>))).Any()).ToList();
            foreach (var item in handlerTypes)
            {
                var messagesHandlers = item.GetInterfaces().Where(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(IMessageHandler<>) || x.GetGenericTypeDefinition() == typeof(IMessageHandler<,>)));
                foreach (var messageHandler in messagesHandlers)
                {
                    var messageType = messageHandler.GetGenericArguments().First();
                    handlers.AddOrUpdate(messageType, new List<Type>() { item }, (x, y) =>
                    {
                        y.Add(item);
                        return y;
                    });
                }
            }
        }

        public void Publish(IMessage message, IMessageContext sender)
        {
            var messageTpye = message.GetType();
            if (handlers.ContainsKey(messageTpye))
            {
                foreach (var handler in handlers[messageTpye])
                {
                    dynamic instance = handlerFactory(handler);
                    try
                    {
                        if (HasResponse(message))
                        {
                            var result = instance.Handle((dynamic)(message.ToPublishedMessage(sender)));
                            sender.Respond(result);
                        }
                        else
                            instance.Handle((dynamic)(message.ToPublishedMessage(sender)));
                    }
                    catch (Exception ex)
                    {
                        sender.ErrorFormat("{0} , {1}", ex.Message, ex.StackTrace);
                    }
                }
            }
        }

        private bool HasResponse(IMessage message)
        {
            return message.GetType().GetInterfaces().Where(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(IMessage<>))).Any();
        }
    }

}