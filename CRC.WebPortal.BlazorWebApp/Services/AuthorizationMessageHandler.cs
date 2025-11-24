using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System;

namespace CRC.WebPortal.BlazorWebApp.Services;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private const string TokenKey = "authToken";

    public AuthorizationMessageHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            // Get the token from localStorage
            var token = await _localStorage.GetItemAsync<string>(TokenKey);
            
            if (!string.IsNullOrEmpty(token))
            {
                // Add the Authorization header with Bearer token
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        catch (Exception)
        {
            // If there's an error getting the token, continue without it
            // This handles cases where localStorage might not be available yet
        }

        Console.WriteLine($"HTTP {request.Method} {request.RequestUri}");
        var response = await base.SendAsync(request, cancellationToken);
        Console.WriteLine($"HTTP {request.Method} {request.RequestUri} => {(int)response.StatusCode} {response.StatusCode}");
        return response;
    }
}
