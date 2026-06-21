using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace KFHAssessment.Client.Services;

public class AppAuthStateProvider : AuthenticationStateProvider
{
    private readonly TokenStore _store;

    public AppAuthStateProvider(TokenStore store) => _store = store;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await _store.InitAsync();

        if (string.IsNullOrEmpty(_store.Token))
            return new AuthenticationState(new ClaimsPrincipal());

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_store.Token);
        if (jwt.ValidTo < DateTime.UtcNow)
        {
            await _store.ClearAsync();
            return new AuthenticationState(new ClaimsPrincipal());
        }

        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void Notify() =>
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
}