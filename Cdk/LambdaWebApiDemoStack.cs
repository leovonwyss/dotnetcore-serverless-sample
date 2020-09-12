using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ElasticLoadBalancingV2;
using Amazon.CDK.AWS.ElasticLoadBalancingV2.Targets;
using Amazon.CDK.AWS.Lambda;

namespace LeoVonwyss.AspNetServerlessSample.Cdk
{
    public class LambdaWebApiDemoStack : Stack
    {
        internal LambdaWebApiDemoStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            // This defines the lambda function we want to deploy 
            var lambdaFunction = new Function(this, "backend-lambda", new FunctionProps()
            {
                // binary to deploy
                Code = Code.FromAsset("../WebApiDemo/bin/Debug/netcoreapp3.1"),
                Runtime = Runtime.DOTNET_CORE_3_1,
                
                // Handler name: ASSEMBLY::TYPE::FUNCTION, see https://docs.aws.amazon.com/lambda/latest/dg/csharp-handler.html
                // This tells the Lambda runtime what entry point to use in the code being deployed above
                Handler = "WebApiDemo::WebApiDemo.LambdaEntryPoint::FunctionHandlerAsync" 
                     
            });

            // This defines the Internet-facing http endpoint to create.
            // An API Gateway could be used instead of a load balancer depending on project needs
            ApplicationLoadBalancer lb = new ApplicationLoadBalancer(this, "lambda-lb",
                new ApplicationLoadBalancerProps()
                {
                    Vpc = new Vpc(this, "lambda-lb-vpc"),
                    InternetFacing = true
                });
            var listener = lb.AddListener("lambda-lb-listener", new ApplicationListenerProps()
            {
                Port = 80
            });
            
            // This links the load balancer and the lambda function.
            // IAM roles and other low-level configuration is taken care of by CDK.
            listener.AddTargets("lambda-lb-target", new AddApplicationTargetsProps() {
                Targets = new[]{new LambdaTarget(lambdaFunction) }
            });
            
            // not strictly needed, this simply outputs the (random) url of the generated
            // http endpoint
            CfnOutput output = new CfnOutput(this, "lb-url", new CfnOutputProps()
            {
                ExportName = "loadbalancer-url",
                Value = "http://" + lb.LoadBalancerDnsName + "/WeatherForecast"
            });
        }
    }
}

