using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDI.Model
{
    public struct Key
    {
        public Key(DependencyDefine define, Type[] genericArguments)
        {
            this.Define = define;
            this.GenericArguments = genericArguments;
        }
        public DependencyDefine Define { get; }
        public Type[] GenericArguments { get; }
    }
}
