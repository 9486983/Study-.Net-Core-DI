using AutoDI.Attributes;
using AutoDI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AutoDI.DI
{
    public partial class ServicesContainer
    {
        /// <summary>
        /// 注册当前程序集
        /// </summary>
        public virtual ServicesContainer RegisterAssembly()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            RegisterToContainer(assembly);
            return this;
        }
        /// <summary>
        /// 注册多个程序集
        /// </summary>
        /// <param name="assemblyNames">程序集全称</param>
        public virtual ServicesContainer RegisterAssembly(params string[] assemblyNames)
        {
            foreach (string assemblyName in assemblyNames)
            {
                Assembly assembly = Assembly.Load(assemblyName);
                RegisterToContainer(assembly);
            }
            return this;
        }

        /// <summary>
        /// 将程序集中符合带有DIAttribute标签的类注入容器
        /// </summary>
        /// <param name="assembly">程序集</param>
        protected virtual void RegisterToContainer(Assembly assembly)
        {
            //  找到所有的DI标签，获取它们的抽象和注册类型等信息
            var typeAttributes = from type in assembly.GetExportedTypes()
                                 let attribute = type.GetCustomAttribute<DIAttribute>()
                                 where attribute != null
                                 select new { Type = type, Attribute = attribute };
            foreach (var item in typeAttributes)
            {
                Func<ServicesContainer, Type[], object> factory = (container, args) => CreateInstance(container, item.Type, args);
                Register(new DependencyDefine(item.Attribute.FromAbstract, item.Attribute.InjectionType, factory));
            }
        }

        /// <summary>
        /// 是具体的实现
        /// </summary>
        /// <param name="container"></param>
        /// <param name="type">实现类</param>
        /// <param name="args">泛型类型的参数</param>
        /// <returns></returns>
        public object CreateInstance(ServicesContainer container, Type type, Type[] args)
        {
            if (args.Length > 0)
            {
                type = type.MakeGenericType(args);  //当有参数时创建个泛型的类型
            }
            var constructs = type.GetConstructors();
            if (constructs.Length == 0)
            {
                throw new Exception($"{type.Name} 找不到构造函数!");
            }

            var constructor = constructs.FirstOrDefault(ct => ct.GetCustomAttributes(false).OfType<DIAttribute>().Any()) ?? constructs.First();

            var paramters = constructor.GetParameters();
            //  如果是无参构造函数则直接用Activator创建实例即可
            //  否则要获取构造函数中的参数，为参数实例化并赋值
            if (paramters.Length == 0)
            {
                return Activator.CreateInstance(type);
            }
            else
            {
                var createInstanceArgs = new object[paramters.Length];
                for (int i = 0; i < createInstanceArgs.Length; i++)
                {
                    var paramType = paramters[i].ParameterType;
                    //  首先从DI容器中获取，如果已经有了则不需要再次创建
                    createInstanceArgs[i] = container.GetService(paramType);
                }
                return constructor.Invoke(createInstanceArgs);
            }
        }

        public virtual ServicesContainer Register(DependencyDefine define)
        {
            //  放入注册容器，没有的话会先创建
            //  实例在使用时才创建，首次加载全部实例化效率差且占用内存

            //  如果一个接口有多个实例则替换为当前的定义
            if (this.DefineContainer.TryGetValue(define.FromTypeAbstrcut, out DependencyDefine existing))
            {
                this.DefineContainer[define.FromTypeAbstrcut] = define;
                define.Next = existing;
            }
            else
            {
                this.DefineContainer[define.FromTypeAbstrcut] = define;
            }


            return this;
        }

        protected bool TryAdd(Type type, DependencyDefine define)
        {
            return this.DefineContainer.TryAdd(type, define);
        }
    }
}
