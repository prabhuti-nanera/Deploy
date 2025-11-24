using CRC.WebPortal.Shared.Dtos;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.NumberingPatterns.Queries.GetNumberingPatternsByProject;

public record GetNumberingPatternsByProjectQuery(int ProjectId) : IRequest<BaseResponse<List<UnitNumberingPatternDto>>>;
