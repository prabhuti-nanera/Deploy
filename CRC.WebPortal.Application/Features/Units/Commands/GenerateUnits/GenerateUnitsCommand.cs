using CRC.WebPortal.Shared.Requests;
using CRC.WebPortal.Application.Common.Dtos;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Units.Commands.GenerateUnits;

public record GenerateUnitsCommand(CRC.WebPortal.Shared.Requests.GenerateUnitsRequest Request) : IRequest<BaseResponse<List<UnitDto>>>;
