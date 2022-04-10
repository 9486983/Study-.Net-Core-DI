using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using AutoDI.Model;

namespace AutoDI.DI
{
    public partial class ServicesContainer : IDisposable
    {
        private ServicesContainer Root { get; set; }
        /// <summary>
        /// 注册容器
        /// </summary>
        private readonly ConcurrentDictionary<Type, DependencyDefine> DefineContainer;
        /// <summary>
        /// 实例容器
        /// </summary>
        private readonly ConcurrentDictionary<Key, object> InstanceContainer;

        private volatile bool _isDisposed;
        private readonly ConcurrentBag<IDisposable> _disposables;

        public ServicesContainer()
        {
            Root = this;
            DefineContainer = new ConcurrentDictionary<Type, DependencyDefine>();
            InstanceContainer = new ConcurrentDictionary<Key, object>();
            _disposables = new ConcurrentBag<IDisposable>();
        }

        public ServicesContainer(ServicesContainer Parent)
        {
            Root = Parent.Root;
            DefineContainer = Parent.DefineContainer;
            InstanceContainer = new ConcurrentDictionary<Key, object>();
            _disposables = new ConcurrentBag<IDisposable>();
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                foreach (var item in _disposables)
                {
                    item.Dispose();
                }
                _disposables.Clear();
                InstanceContainer.Clear();
            }
        }
    }
}
