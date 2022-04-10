using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Study.Core
{
    public static class AutoDI
    {
        private static readonly ServiceCollection services = new ServiceCollection();

        public static ServiceProvider Default;

        public static void RegisterWithDll(params string[] assemblyNames)
        {
            try
            {
                foreach (string assembly in assemblyNames)
                {
                    var types = Assembly.Load(assembly).GetTypes();
                    RegisterByTypes(types);
                    Default = services.BuildServiceProvider();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void RegisterWithPath(string assemblyPath)
        {
            try
            {
                var folder = Path.Combine(Environment.CurrentDirectory, assemblyPath);
                var files = Directory.GetFiles(folder);
                foreach (var file in files)
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                    var types = assembly.GetTypes();
                    RegisterByTypes(types);
                }
                Default = services.BuildServiceProvider();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Register<T>(InjectionType injectionType = InjectionType.Singleton)
        {
            Register(typeof(T), injectionType);
        }

        public static void Register(Type type, InjectionType injectionType = InjectionType.Singleton)
        {
            switch (injectionType)
            {
                case InjectionType.Singleton:
                    services.AddSingleton(type);
                    break;
                case InjectionType.Transient:
                    services.AddTransient(type);
                    break;
                case InjectionType.Scoped:
                    services.AddScoped(type);
                    break;
                default:
                    break;
            }
        }

        private static void RegisterByTypes(Type[] types)
        {
            foreach (Type type in types)
            {
                if (type.GetInterfaces().Contains(typeof(IService)))
                {
                    var injectionType = type.GetCustomAttribute<DIAttribute>().InjectionType;
                    Register(type, injectionType);
                }
            }
        }

        public static T GetService<T>(this IServiceProvider provider, params object[] @params) where T : class
        {
            return ActivatorUtilities.CreateInstance<T>(provider, @params);
        }
    }

}
