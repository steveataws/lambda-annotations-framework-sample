using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
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
    public async Task<APIGatewayHttpApiV2ProxyResponse> TranslateText(APIGatewayHttpApiV2ProxyRequest apigProxyEvent, ILambdaContext context)
    {
        var translateClient = new AmazonTranslateClient();

        try
        {
            var request = new TranslateTextRequest
            {
                SourceLanguageCode = apigProxyEvent.QueryStringParameters["sourceLang"],
                TargetLanguageCode = apigProxyEvent.QueryStringParameters["targetLang"],
                Text = apigProxyEvent.Body,
                Settings = new TranslationSettings
                {
                    Profanity = Profanity.MASK
                }
            };

            var response = await translateClient.TranslateTextAsync(request);
            return new APIGatewayHttpApiV2ProxyResponse
            {
                Body = response.TranslatedText,
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }
        catch (AmazonTranslateException e)
        {
            context.Logger.LogLine(e.Message);
            throw;
        }
    }
}
