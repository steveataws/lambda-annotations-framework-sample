using Microsoft.Extensions.DependencyInjection;

namespace Translator;

[Amazon.Lambda.Annotations.LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAWSService<Amazon.Translate.IAmazonTranslate>();
    }
}