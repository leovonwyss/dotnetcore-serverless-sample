using System;
using System.Threading.Tasks;
using Amazon.Lambda.ApplicationLoadBalancerEvents;
using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebApiDemo
{
    public class Program
    {
        private class LambdaEntryPoint :
            ApplicationLoadBalancerFunction
        {
            protected override void Init(IWebHostBuilder builder)
            {
                ConfigureWebHostBuilder(builder);
            }
        }
        
        public static void Main(string[] args)
        {
            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME")))
            {
                RunServerless();
            }
            else
            {
                RunAsWebserver(args);
            }
        }

        private static void RunServerless()
        {
            var lambdaEntry = new LambdaEntryPoint();
            Func<ApplicationLoadBalancerRequest, ILambdaContext, Task<ApplicationLoadBalancerResponse>> functionHandler =
                lambdaEntry.FunctionHandlerAsync;
            using var handlerWrapper = HandlerWrapper.GetHandlerWrapper(functionHandler, new JsonSerializer());
            using var bootstrap = new LambdaBootstrap(handlerWrapper);
            bootstrap.RunAsync().Wait();
        }

        private static void RunAsWebserver(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(ConfigureWebHostBuilder)
                .Build()
                .Run();
        }

        public static void ConfigureWebHostBuilder(IWebHostBuilder webHostBuilder)
        {
            webHostBuilder
                .UseStartup<Startup>();
        }
    }
}
