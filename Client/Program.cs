using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using dnkh.AuthZ.WASM;

namespace BlazorWASMAuthorization.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("BlazorWASMAuthorization.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorWASMAuthorization.ServerAPI"));

            builder.Services.AddMsalAuthentication<RemoteAuthenticationState, AuthZUserAccount>(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                string apiScopeUrl = builder.Configuration.GetValue<string>("APIScopeUrl");
                options.ProviderOptions.DefaultAccessTokenScopes.Add(apiScopeUrl);
                
                options.UserOptions.RoleClaim = "roles";

            }).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, AuthZUserAccount, AuthZUserAccountFactory>();


            builder.Services.AddAuthorizationCore( options =>
            {
                    options.AddPolicy("GroupPolicy", policy => 
                    {
                        string requiredGroupId = builder.Configuration.GetValue<string>("RequiredGroupId");
                        policy.RequireClaim("groups",requiredGroupId);
                    });
            });

            await builder.Build().RunAsync();
        }
    }
}
