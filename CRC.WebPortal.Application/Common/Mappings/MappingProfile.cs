using AutoMapper;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Application.Features.Auth.Commands.SignIn;
using CRC.WebPortal.Application.Features.Auth.Commands.SignUp;
using CRC.WebPortal.Application.Features.Auth.Commands.SendOtp;
using CRC.WebPortal.Application.Features.Auth.Commands.VerifyOtp;
using CRC.WebPortal.Domain.Entities;
using CRC.Common.Models;

namespace CRC.WebPortal.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entity to DTO mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

        // DTO to Command mappings
        CreateMap<SignupRequest, SignUpCommand>();
        CreateMap<SigninRequest, SignInCommand>();
        CreateMap<SendOtpRequest, SendOtpCommand>();
        CreateMap<VerifyOtpRequest, VerifyOtpCommand>();
    }
}
