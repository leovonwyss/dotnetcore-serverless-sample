# Serverless asp.net core on AWS Lambda

This repository contains sample code to show the basic building blocks
of an asp.net core web API deployed on AWS lambda using CDK.

## Repository Contents

### WebApiDemo
This folder contains a simple asp.net core 3.1 web api. It is based on the "Web API" template with just the required additions for running on AWS lambda.

### Cdk
This folder contains the AWS CDK code to deploy the asp.net core site to AWS lambda.
For more information on CDK and C#, see https://docs.aws.amazon.com/cdk/latest/guide/work-with-cdk-csharp.html


## Getting started

Prerequisites:
* Have AWS command line tools installed and configured
* Have AWS CDK tools installed and configured (see https://docs.aws.amazon.com/cdk/latest/guide/getting_started.html )

Steps to build and deploy the sample:
* Build the web app using  `dotnet build WebApiDemo`
* Build the CDK script using  `dotnet build WebApiDemo`
* Deploy the site to AWS using `cd Cdk && cdk deploy`

You will see an output similar to this:
`LambdaWebApiDemoStack.lburl = http://Lambd-lambd-29FADJC1E9PS-1063229254.eu-central-1.elb.amazonaws.com/WeatherForecast`

Copy the Url shown and paste it into your browser. You will see the output of the sample API with some random weather forecast data.
