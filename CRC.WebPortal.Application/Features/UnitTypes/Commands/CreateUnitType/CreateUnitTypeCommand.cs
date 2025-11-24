using CRC.WebPortal.Application.Common.Dtos;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitTypes.Commands.CreateUnitType;

public record CreateUnitTypeCommand(CreateUnitTypeRequest Request) : IRequest<BaseResponse<UnitTypeDto>>;
