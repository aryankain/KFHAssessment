using Microsoft.JSInterop;

namespace KFHAssessment.Client.Services;

public class TokenStore
{
    private readonly IJSRuntime _js;

    public TokenStore(IJSRuntime js) => _js = js;

    public string? Token { get; private set; }
    public string? Username { get; private set; }
    public string? Role { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

    public async Task InitAsync()
    {
        Token = await _js.InvokeAsync<string?>("localStorage.getItem", "kfh_token");
        Username = await _js.InvokeAsync<string?>("localStorage.getItem", "kfh_username");
        Role = await _js.InvokeAsync<string?>("localStorage.getItem", "kfh_role");
    }

    public async Task SetAsync(string token, string username, string role)
    {
        Token = token;
        Username = username;
        Role = role;
        await _js.InvokeVoidAsync("localStorage.setItem", "kfh_token", token);
        await _js.InvokeVoidAsync("localStorage.setItem", "kfh_username", username);
        await _js.InvokeVoidAsync("localStorage.setItem", "kfh_role", role);
    }

    public async Task ClearAsync()
    {
        Token = null;
        Username = null;
        Role = null;
        await _js.InvokeVoidAsync("localStorage.removeItem", "kfh_token");
        await _js.InvokeVoidAsync("localStorage.removeItem", "kfh_username");
        await _js.InvokeVoidAsync("localStorage.removeItem", "kfh_role");
    }
}