using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cdk
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
