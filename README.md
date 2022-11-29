# Lambda Annotations Framework for .NET Sample

The code in this repository illustrates the steps to convert a "classic" .NET Lambda function to use the new, pre-release (and still under design) Lambda Annotations for .NET. You can find out more about the new framework, and links to the repository and design doc, in [this blog post](https://aws.amazon.com/blogs/developer/introducing-net-annotations-lambda-framework-preview/).

The sample serverless application uses [Amazon Translate](https://aws.amazon.com/translate) to convert text between two languages. Codes identifying the source language and the target language are passed using query parameters in the URL representing the [API Gateway](https://aws.amazon.com/api-gateway/) endpoint fronting the function. The text to convert is passed as the request body for the POST request. For example, `https://endpoint-url?sourceLang=en&targetLang=de`.

## Branches

The repository contains 4 branches:

* __main__ - this represents the starting point, with a "classic" function using the typical Lambda method signature of a .NET object representing the payload, and a context object. In this branch, the source and target language codes are expressed with the query parameters `sourceLang` and `targetLang`.

* __Update1_Annotations__ - this branch represents the changes to convert the function to use the annotations framework. In this version, to prove the change, the query parameters are expressed as `input` and `output`. These are mapped by the framework to the `input` and `output` method parameters using the `[FromQuery]` attribute. The body text to convert is mapped to a method parameter using the `[FromBody]` attribute.

* __Update2_DependencyInjection__ - this branch represents a further change to the function to use the new dependency injection mechanism included with the framework, and mentioned in the blog post linked earlier. Instead of instantiating the Amazon.Translate service client in the function, it is instead registered in a startup class and injected into the function's class constructor. This approach is ideal for injected objects that do not maintain state.

* __Update3_DependencyInjection__ - the final branch changes the dependency injection to add the required service client to the function's method signature using the `[FromService]` attribute. This approach can be used for injected objects that need to be scoped to the request.

## Deployment

To deploy the sample branches, first install the AWS Lambda tools for the dotNET CLI, [Amazon.Lambda.Tools](https://www.nuget.org/packages/Amazon.Lambda.Tools) (to install, run `dotnet tool install -g amazon.lambda.tools)`). Then, in the project folder, run the command `dotnet lambda deploy-serverless`. You will be asked to supply the name of the [AWS CloudFormation](https://aws.amazon.com/cloudformation) stack containing the resources for the serverless application, and an [Amazon S3](https://aws.amazon.com/s3) storage bucket to hold the built function code.

The command above will look for a credential profile named _default_ to determine the credentials that should be used to deploy the code, and region. To specify these in the command, change it to `dotnet lambda deploy-serverless --profile profile_name --region region_code_here`, for example `dotnet lambda deploy-serverless --profile steve-demo --region us-west-2`.

After deployment, the URL to the API Gateway endpoint fronting the function will be displayed. Use this URL in a tool like Postman to send a POST request, filling in the query parameters and body text to convert.

To redeploy after changing branches, simply run the same `dotnet lambda deploy-serverless` command again.
