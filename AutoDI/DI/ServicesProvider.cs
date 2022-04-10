using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using AutoDI.Model;
using System.Linq;
using AutoDI.Enums;

namespace AutoDI.DI
{
    public partial class ServicesContainer
    {
        public object GetService(Type type)
        {
            DependencyDefine define;

            /// 如果是泛型并且可迭代的类
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var genericAargs = type.GetGenericArguments();
                //  类似MyClass<T>的情况
                var arg_0 = genericAargs[0];
                if (!this.DefineContainer.TryGetValue(arg_0, out define))
                {
                    return Array.CreateInstance(arg_0, 0);
                }
                //  类似MyClass<T1, T2>的情况
                var services = this.DefineContainer.AsEnumerable().ToArray();
                Array array = Array.CreateInstance(arg_0, services.Length);
                services.CopyTo(array, 0);
                return array;
            }

            if (type.IsGenericType && !this.DefineContainer.ContainsKey(type))
            {
                var definition = type.GetGenericTypeDefinition();
                this.DefineContainer.TryGetValue(definition, out define);

                if (define == null)
                {
                    throw new Exception($"未能找到{definition.GetType()?.Name}的类型");
                }
                return GetService(define, type.GetGenericArguments());

            }

            this.DefineContainer.TryGetValue(type, out define);
            if (define == null)
            {
                throw new Exception($"未能找到{type?.Name}");
            }
            return GetService(define, type.GetGenericArguments());

        }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="define"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object GetService(DependencyDefine define, Type[] args)
        {
            switch (define.LifeTime)
            {
                case InjectionType.Singleton:
                case InjectionType.Scoped:
                    return GetOrCreate();
                default:
                    return define.Factory(this, args);
            }

            //  当存在时直接获取，不存在先创建再获取
            object GetOrCreate()
            {
                Key key = new Key(define, args);
                this.InstanceContainer.TryGetValue(key, out object instance);
                if (instance == null)
                {
                    instance = define.Factory(this, args);
                    this.InstanceContainer.TryAdd(key, instance);
                }
                return instance;
            }
        }


    }

    public static class ServicesProviderEx
    {
        public static ServicesContainer Register<TFrom, TTo>(this ServicesContainer services, InjectionType injectionType) where TTo : TFrom
        {
            return services.Register(typeof(TFrom), typeof(TTo), injectionType);
        }
        public static ServicesContainer Register(this ServicesContainer services, Type TFrom, Type TTo, InjectionType injectionType)
        {
            Func<ServicesContainer, Type[], object> factory = (container, args) => services.CreateInstance(container, TTo, args);
            return services.Register(new DependencyDefine(TFrom, injectionType, factory));
        }
        public static T GetService<T>(this ServicesContainer services)
        {
            return (T)services.GetService(typeof(T));
        }

        public static ServicesContainer CreateSingleton(this ServicesContainer services)
        {
            return new ServicesContainer(services);
        }
    }
}