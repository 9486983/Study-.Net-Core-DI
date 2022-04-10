using AutoDI.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDI.Attributes
{
    public class DIAttribute : Attribute
    {
        /// <summary>
        /// 抽象于什么类或接口
        /// </summary>
        public Type FromAbstract { get; set; }
        /// <summary>
        /// 生命周期
        /// </summary>
        public InjectionType InjectionType { get; set; }

        /// <summary>
        /// 构造函数使用，默认取带有该标签的第一个构造函数
        /// </summary>
        public DIAttribute()
        {

        }

        /// <summary>
        /// 类使用，注入时根据该标签筛查
        /// </summary>
        /// <param name="fromAbstract"></param>
        /// <param name="injectionType"></param>
        public DIAttribute(Type fromAbstract, InjectionType injectionType = InjectionType.Transient)
        {
            this.FromAbstract = fromAbstract;
            this.InjectionType = injectionType;
        }
    }
}
