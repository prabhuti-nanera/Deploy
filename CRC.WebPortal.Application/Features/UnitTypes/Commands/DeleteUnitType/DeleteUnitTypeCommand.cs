using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitTypes.Commands.DeleteUnitType;

public record DeleteUnitTypeCommand(int Id) : IRequest<BaseResponse<bool>>;
