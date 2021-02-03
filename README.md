# BlazorWASMAuthorization

## Introduction

This small project is about how to get the roles of a user which is authenticated via Azure Active Directory (AAD) and has a roles assigned in AAD.

If you have the role you can hide UI elements (e.g. menu entries or links) and deny calls to pages (e.g. Admin area)

If you create your project with the following command, authentication works fine against the AAD but every check regarding the role ( IsInRole or [Authorize(Roles="Admin")]) will fail.

The reason is how the roles claim is transfered to the client.
The framework expects a string with a list of roles, but the client gets an array of strings

## Getting started

To setup your Azure environment follow the instructions at the following link (Microsoft docs)

[Secure an ASP.NET Core Blazor WebAssembly hosted app with Azure Active Directory](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/hosted-with-azure-active-directory?view=aspnetcore-5.0)

:warning: **Don`t forget to check to the the API URL iin the client program.cs as mentioned in the docs** 

```csharp
options.ProviderOptions.DefaultAccessTokenScopes.Add(
    "api://41451fa7-82d9-4673-8fa5-69eff5a761fd/API.Access");
```
Sometimes there is **api://api://...** if this is the case **remove one "api://"**



If the it's up and running you can use my nuget package or copy the code from this folder **dnkh.AuthZ.WASM** from this repo

## Change the client code

Open the file program.cs in the client folder ( Blazor WASM project)

Add a using statement 

```csharp
using dnkh.AuthZ.WASM;
```

and replace the following code 

```csharp
    builder.Services.AddMsalAuthentication(options =>
    {
        builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
        options.ProviderOptions.DefaultAccessTokenScopes.Add("api://41451fa7-82d9-4673-8fa5-69eff5a761fd/API.Access");
     });

```

with this code

```csharp
            builder.Services.AddMsalAuthentication<RemoteAuthenticationState, AuthZUserAccount>(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add("api://41451fa7-82d9-4673-8fa5-69eff5a761fd/API.Access");
                
                options.UserOptions.RoleClaim = "roles";

            }).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, AuthZUserAccount, AuthZUserAccountFactory>();

```

In my code example I moved the API Url into the appsettings.json file in the wwwroot folder.