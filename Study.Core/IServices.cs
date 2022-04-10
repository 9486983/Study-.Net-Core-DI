using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Core
{
    public interface IService
    {

    }

    public enum InjectionType
    {
        Singleton,
        Transient,
        Scoped
    }
    public class DIAttribute : Attribute
    {
        public InjectionType InjectionType { get; set; }
        public DIAttribute(InjectionType injectionType)
        {
            this.InjectionType = injectionType;
        }
    }
    public interface IVehicle
    {
        public string ID { get; set; }
        public void Run();
    }
}
