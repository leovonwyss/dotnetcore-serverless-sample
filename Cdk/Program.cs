using Amazon.CDK;

namespace LeoVonwyss.AspNetServerlessSample.Cdk
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new LambdaWebApiDemoStack(app, "LambdaWebApiDemoStack");
            app.Synth();
        }
    }
}
