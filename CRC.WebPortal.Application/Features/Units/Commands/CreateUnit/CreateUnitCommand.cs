using CRC.WebPortal.Shared.Requests;
using CRC.WebPortal.Application.Common.Dtos;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Units.Commands.CreateUnit;

public record CreateUnitCommand(CRC.WebPortal.Shared.Requests.CreateUnitRequest Request) : IRequest<BaseResponse<UnitDto>>;
