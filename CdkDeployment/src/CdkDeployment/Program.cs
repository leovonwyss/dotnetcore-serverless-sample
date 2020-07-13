using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CdkDeployment
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new CdkDeploymentStack(app, "CdkDeploymentStack");
            app.Synth();
        }
    }
}
