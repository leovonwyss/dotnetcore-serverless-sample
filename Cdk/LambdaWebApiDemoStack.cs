using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ElasticLoadBalancingV2;
using Amazon.CDK.AWS.ElasticLoadBalancingV2.Targets;
using Amazon.CDK.AWS.Lambda;

namespace Cdk
{
    public class LambdaWebApiDemoStack : Stack
    {
        internal LambdaWebApiDemoStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var lambdaFunction = new Function(this, "backend-lambda", new FunctionProps()
            {
                Code = Code.FromAsset("../WebApiDemo/bin/Debug/netcoreapp3.1"),
                Runtime = Runtime.DOTNET_CORE_3_1,
                Handler = "WebApiDemo::WebApiDemo.LambdaEntryPoint::FunctionHandlerAsync"
            });

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
            var targetGroup = new ApplicationTargetGroup(this, "lambda-lb-targetgroup",
                new ApplicationTargetGroupProps()
                {
                    TargetType = TargetType.LAMBDA,
                    Targets = new[] {new LambdaTarget(lambdaFunction)},
                    HealthCheck = new HealthCheck()
                    {
                        Enabled = true
                    }
                });
            listener.AddTargetGroups("lambda-lb-target",
                new AddApplicationTargetGroupsProps() {TargetGroups = new[] {targetGroup}});
            
            CfnOutput output = new CfnOutput(this, "lb-url", new CfnOutputProps()
            {
                ExportName = "loadbalancer-url",
                Value = lb.LoadBalancerDnsName
            });
        }
    }
}

