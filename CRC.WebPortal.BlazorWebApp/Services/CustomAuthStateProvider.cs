using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using CRC.WebPortal.BlazorWebApp.Models;

namespace CRC.WebPortal.BlazorWebApp.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private const string TokenKey = "authToken";
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>(TokenKey);
            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(_anonymous);
            }

            // Create basic authenticated claims when token exists
            // This ensures the user is considered authenticated for AuthorizeView
            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user"),
                new Claim(ClaimTypes.Name, "Authenticated User"),
                new Claim("authenticated", "true")
            }, "jwt");

            return new AuthenticationState(new ClaimsPrincipal(claims));
        }
        catch (Exception)
        {
            // Return anonymous state if any error occurs
            return new AuthenticationState(_anonymous);
        }
    }

    public void NotifyAuthenticationStateChanged(UserDto user)
    {
        var authState = Task.FromResult(user == null ?
            new AuthenticationState(_anonymous) :
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim("email", user.Email), // Add email claim for compatibility
                new Claim("FirstName", user.FirstName ?? ""),
                new Claim("LastName", user.LastName ?? "")
            }, "jwt"))));
        NotifyAuthenticationStateChanged(authState);
    }
}
