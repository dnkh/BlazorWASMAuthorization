using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace dnkh.AuthZ.WASM
{

    public class AuthZUserAccountFactory 
        : AccountClaimsPrincipalFactory<AuthZUserAccount>
    {
        public AuthZUserAccountFactory(NavigationManager navigationManager, 
            IAccessTokenProviderAccessor accessor) : base(accessor)
        {
        }
    
        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(
            AuthZUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var initialUser = await base.CreateUserAsync(account, options);

            if (initialUser.Identity.IsAuthenticated)
            {
                foreach (var value in account.Roles)
                {
                    ((ClaimsIdentity)initialUser.Identity)
                        .AddClaim(new Claim("roles", value));
                }

                foreach (var value in account.Groups)
                {
                    ((ClaimsIdentity)initialUser.Identity)
                        .AddClaim(new Claim("groups", value));
                }
            }

            return initialUser;
        }
    }
}