using System;
using System.Collections.Concurrent;

namespace Elders.Skynet.Core.Util
{
    public class BasicContainer
    {
        private ConcurrentDictionary<Type, Func<object>> dependencies;

        public BasicContainer()
        {
            dependencies = new ConcurrentDictionary<Type, Func<object>>();
        }

        public void Register<T>(Func<T> factory)
        {
            dependencies[typeof(T)] = () => factory();
        }

        public object Resolve(Type t)
        {
            if (dependencies.ContainsKey(t))
                return dependencies[t]();

            var instance = Activator.CreateInstance(t);
            var props = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            foreach (var item in props)
            {
                if (dependencies.ContainsKey(item.PropertyType))
                {
                    item.SetValue(instance, dependencies[item.PropertyType]());
                }
            }
            return instance;
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }
    }
}
