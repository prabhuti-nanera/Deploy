using CRC.WebPortal.Application.Common.Dtos;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitTypes.Queries.GetUnitTypeById;

public class GetUnitTypeByIdQueryHandler : IRequestHandler<GetUnitTypeByIdQuery, BaseResponse<UnitTypeDto>>
{
    private readonly IRepository<UnitType> _unitTypeRepository;

    public GetUnitTypeByIdQueryHandler(IRepository<UnitType> unitTypeRepository)
    {
        _unitTypeRepository = unitTypeRepository;
    }

    public async Task<BaseResponse<UnitTypeDto>> Handle(GetUnitTypeByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get unit type by ID
            var unitType = await _unitTypeRepository.GetFirstOrDefaultAsync(
                ut => ut.Id == request.Id && !ut.IsDeleted);

            if (unitType == null)
            {
                return BaseResponse<UnitTypeDto>.Failure("Unit type not found.");
            }

            // Convert to DTO
            var unitTypeDto = new UnitTypeDto
            {
                Id = unitType.Id,
                Name = unitType.Name,
                Description = unitType.Description,
                DisplayOrder = unitType.DisplayOrder,
                ProjectId = unitType.ProjectId,
                CreatedAt = unitType.CreatedAt,
                UpdatedAt = unitType.UpdatedAt,
                IsActive = unitType.IsActive
            };

            return BaseResponse<UnitTypeDto>.Success(unitTypeDto, "Unit type retrieved successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<UnitTypeDto>.Failure($"Error retrieving unit type: {ex.Message}");
        }
    }
}
