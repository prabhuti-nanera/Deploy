using CRC.WebPortal.Shared.Dtos;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Units.Queries.GetUnitsByProject;

public record GetUnitsByProjectQuery(int ProjectId) : IRequest<BaseResponse<List<UnitDto>>>;
