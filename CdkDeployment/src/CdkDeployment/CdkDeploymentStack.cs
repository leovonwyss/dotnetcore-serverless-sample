using Amazon.CDK;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.S3.Deployment;

namespace CdkDeployment
{
    public class CdkDeploymentStack : Stack
    {
        const string deploymentVersion = "1.0.0";
        
        internal CdkDeploymentStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var sourceBucket = new Bucket(this, "leovonwyss-example-website-s3", new BucketProps()
            {
                BucketName = "cdk-angular",
                WebsiteIndexDocument = "index.html",
                PublicReadAccess = true
            });
            var deployment = new BucketDeployment(this, "DeployWebsite", new BucketDeploymentProps()
            {
                Sources = new []{ Source.Asset("../WebApplication/ClientApp/dist/AngularApp")},
                DestinationBucket = sourceBucket,
                DestinationKeyPrefix = deploymentVersion
            });
            
            var cloudFrontOia = new CfnCloudFrontOriginAccessIdentity(this, "OIA", new CfnCloudFrontOriginAccessIdentityProps()
            {
                CloudFrontOriginAccessIdentityConfig = new CfnCloudFrontOriginAccessIdentity.CloudFrontOriginAccessIdentityConfigProperty()
                {
                    Comment = "OIA for static site"
                }
            });
            
            var distribution = new CloudFrontWebDistribution(this, "cdk-distribution", new CloudFrontWebDistributionProps()
            {
                OriginConfigs = new []{ new SourceConfiguration()
                {
                    S3OriginSource = new S3OriginConfig()
                    {
                        S3BucketSource = sourceBucket,
                    },
                    OriginPath = "/" + deploymentVersion,
                    Behaviors = new []
                    {
                        new Behavior() { IsDefaultBehavior = true }
                    }
                }},
            });
            
            var policy = new PolicyStatement();
            policy.AddActions("s3:GetBucket*");
            policy.AddActions("s3:GetObject*");
            policy.AddActions("s3:List*");
            policy.AddResources(sourceBucket.BucketArn);
            policy.AddResources(sourceBucket.BucketArn + "/*");
            policy.AddCanonicalUserPrincipal(cloudFrontOia.AttrS3CanonicalUserId);
            sourceBucket.AddToResourcePolicy(policy);
        }
    }
}
