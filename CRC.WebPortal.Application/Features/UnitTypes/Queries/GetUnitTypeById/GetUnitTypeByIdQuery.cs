using CRC.WebPortal.Application.Common.Dtos;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitTypes.Queries.GetUnitTypeById;

public record GetUnitTypeByIdQuery(int Id) : IRequest<BaseResponse<UnitTypeDto>>;
