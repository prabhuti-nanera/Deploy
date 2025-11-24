using CRC.WebPortal.Application.Common.Dtos;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitTypes.Queries.GetUnitTypesByProject;

public record GetUnitTypesByProjectQuery(int ProjectId) : IRequest<BaseResponse<List<UnitTypeDto>>>;
