using CRC.WebPortal.Application.Common.Dtos;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitTypes.Commands.UpdateUnitType;

public record UpdateUnitTypeCommand(UpdateUnitTypeRequest Request) : IRequest<BaseResponse<UnitTypeDto>>;
