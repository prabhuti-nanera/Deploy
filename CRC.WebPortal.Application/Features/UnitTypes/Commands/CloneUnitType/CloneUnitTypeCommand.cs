using CRC.WebPortal.Application.Common.Dtos;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitTypes.Commands.CloneUnitType;

public record CloneUnitTypeCommand(CloneUnitTypeRequest Request) : IRequest<BaseResponse<UnitTypeDto>>;
