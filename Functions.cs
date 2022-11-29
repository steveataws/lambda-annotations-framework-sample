using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Translate;
using Amazon.Translate.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Translator;

public class Functions
{
    /// <summary>
    /// A simple function that accepts a body of text, and two query string parameters
    /// indicating the source and target language. The body of text is converted
    /// from the source language to the target language.
    /// </summary>
    /// <param name="apigProxyEvent"></param>
    /// <param name="context"></param>
    /// <returns>The translated text</returns>
    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Post, "/")]
    public async Task<string> TranslateText([FromQuery]string input, [FromQuery]string output, [FromBody]string text2Convert, ILambdaContext context)
    {
        var translateClient = new AmazonTranslateClient();

        try
        {
            var request = new TranslateTextRequest
            {
                SourceLanguageCode = input,
                TargetLanguageCode = output,
                Text = text2Convert,
                Settings = new TranslationSettings
                {
                    Profanity = Profanity.MASK
                }
            };

            var response = await translateClient.TranslateTextAsync(request);

            return response.TranslatedText;
        }
        catch (AmazonTranslateException e)
        {
            context.Logger.LogLine(e.Message);
            throw;
        }
    }
}
