using CRC.WebPortal.Application.Common.Dtos;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitTypes.Commands.CloneUnitType;

public class CloneUnitTypeCommandHandler : IRequestHandler<CloneUnitTypeCommand, BaseResponse<UnitTypeDto>>
{
    private readonly IRepository<UnitType> _unitTypeRepository;

    public CloneUnitTypeCommandHandler(IRepository<UnitType> unitTypeRepository)
    {
        _unitTypeRepository = unitTypeRepository;
    }

    public async Task<BaseResponse<UnitTypeDto>> Handle(CloneUnitTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get the source unit type to clone
            var sourceUnitType = await _unitTypeRepository.GetFirstOrDefaultAsync(
                ut => ut.Id == request.Request.SourceUnitTypeId && !ut.IsDeleted);

            if (sourceUnitType == null)
            {
                return BaseResponse<UnitTypeDto>.Failure("Source unit type not found.");
            }

            // Check if unit type name already exists for this project
            var existingUnitType = await _unitTypeRepository.GetFirstOrDefaultAsync(
                ut => ut.ProjectId == sourceUnitType.ProjectId && 
                      ut.Name.ToLower() == request.Request.NewName.ToLower() && 
                      !ut.IsDeleted);

            if (existingUnitType != null)
            {
                return BaseResponse<UnitTypeDto>.Failure("Unit type with this name already exists for this project.");
            }

            // Get the highest display order for the project to place the cloned unit type at the end
            var maxDisplayOrder = await _unitTypeRepository.GetAllAsync(
                filter: ut => ut.ProjectId == sourceUnitType.ProjectId && !ut.IsDeleted);
            var nextDisplayOrder = maxDisplayOrder.Any() ? maxDisplayOrder.Max(ut => ut.DisplayOrder) + 1 : 1;

            // Create cloned unit type entity
            var clonedUnitType = new UnitType
            {
                Name = request.Request.NewName.Trim(),
                Description = request.Request.NewDescription?.Trim() ?? sourceUnitType.Description,
                DisplayOrder = nextDisplayOrder,
                ProjectId = sourceUnitType.ProjectId
            };

            // Add cloned unit type to database
            await _unitTypeRepository.AddAsync(clonedUnitType);
            await _unitTypeRepository.SaveChangesAsync();

            // Create DTO response
            var unitTypeDto = new UnitTypeDto
            {
                Id = clonedUnitType.Id,
                Name = clonedUnitType.Name,
                Description = clonedUnitType.Description,
                DisplayOrder = clonedUnitType.DisplayOrder,
                ProjectId = clonedUnitType.ProjectId,
                CreatedAt = clonedUnitType.CreatedAt,
                UpdatedAt = clonedUnitType.UpdatedAt,
                IsActive = clonedUnitType.IsActive
            };

            return BaseResponse<UnitTypeDto>.Success(unitTypeDto, "Unit type cloned successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<UnitTypeDto>.Failure($"Error cloning unit type: {ex.Message}");
        }
    }
}
