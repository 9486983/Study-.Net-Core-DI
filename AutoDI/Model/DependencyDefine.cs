using AutoDI.DI;
using AutoDI.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDI.Model
{
    public class DependencyDefine
    {
        public DependencyDefine(Type fromTypeAbstrcut, InjectionType lifeTime, Func<ServicesContainer, Type[], object> factory)
        {
            this.FromTypeAbstrcut = fromTypeAbstrcut;
            this.LifeTime = lifeTime;
            this.Factory = factory;
        }

        /// <summary>
        /// 自身是头部，其他都是Next，Next与Next之间相互关联
        /// </summary>
        public DependencyDefine Next { get; set; }

        /// <summary>
        /// 来自于哪一个抽象的定义
        /// 比如Cat: ICat的 ICat
        /// </summary>
        public Type FromTypeAbstrcut { get; }

        /// <summary>
        /// 生命周期
        /// </summary>
        public InjectionType LifeTime { get; }

        /// <summary>
        /// DependencyContainer:    指定一个容器，
        /// Type[]: 参数类型  
        /// object: 返回一个对象
        /// </summary>
        public Func<ServicesContainer, Type[], object> Factory { get; }
    }
}
