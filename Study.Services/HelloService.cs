using Study.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Study.Services
{
    [DI(InjectionType.Transient)]
    public class HelloService : IService
    {
        private readonly HelloWorldService helloWorldService;
        private readonly HelloWorldService2 helloWorldService2;
        private readonly string name;
        public HelloService(HelloWorldService helloWorldService, HelloWorldService2 helloWorldService2)
        {
            this.helloWorldService = helloWorldService ?? throw new ArgumentNullException(nameof(helloWorldService));
            this.helloWorldService2 = helloWorldService2 ?? throw new ArgumentNullException(nameof(helloWorldService2));
        }
        public HelloService(HelloWorldService helloWorldService, HelloWorldService2 helloWorldService2, string name)
        {
            this.helloWorldService = helloWorldService ?? throw new ArgumentNullException(nameof(helloWorldService));
            this.helloWorldService2 = helloWorldService2 ?? throw new ArgumentNullException(nameof(helloWorldService2));
            this.name = name;
        }
        public string HelloWorld()
        {
            return $"this.helloWorldService.ID: {this.helloWorldService.ID}{Environment.NewLine}this.helloWorldService2.ID: {this.helloWorldService2.ID}";
        }
        public string HelloWorld2()
        {
            return $"this.helloWorldService.ID: {this.helloWorldService.ID}{Environment.NewLine}this.helloWorldService2.ID: {this.helloWorldService2.ID} {name}";
        }
    }

    [DI(InjectionType.Singleton)]
    public class HelloWorldService : IHelloService, IService
    {
        public HelloWorldService()
        {
            this.ID = Guid.NewGuid().ToString();
        }

        public string ID { get; set; }
        public string Hello(string name) => $"Hello {name}";
        public string Hello(string firstname, string lastname) => $"{ID}: Hello {firstname} {lastname}";
    }

    [DI(InjectionType.Singleton)]
    public class HelloWorldService2 : HelloWorldService
    {

    }

    public interface IHelloService
    {
        string Hello(string name);
        string Hello(string firstName, string lastName);
    }
}
