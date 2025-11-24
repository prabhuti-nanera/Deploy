using CRC.WebPortal.BlazorWebApp.Models;
using CRC.Common.Models;

namespace CRC.WebPortal.BlazorWebApp.Services;

public interface IAuthService
{
    Task<AuthResponse> SignUpAsync(SignupRequest signupData);
    Task<AuthResponse> SignInAsync(SigninRequest signinData);
    Task<AuthResponse> SendOtpAsync(SendOtpRequest request);
    Task<AuthResponse> VerifyOtpAndResetPasswordAsync(VerifyOtpRequest request);
    Task LogoutAsync();
}
