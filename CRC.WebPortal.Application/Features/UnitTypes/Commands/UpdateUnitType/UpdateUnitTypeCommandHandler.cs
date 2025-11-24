using CRC.WebPortal.Application.Common.Dtos;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitTypes.Commands.UpdateUnitType;

public class UpdateUnitTypeCommandHandler : IRequestHandler<UpdateUnitTypeCommand, BaseResponse<UnitTypeDto>>
{
    private readonly IRepository<UnitType> _unitTypeRepository;

    public UpdateUnitTypeCommandHandler(IRepository<UnitType> unitTypeRepository)
    {
        _unitTypeRepository = unitTypeRepository;
    }

    public async Task<BaseResponse<UnitTypeDto>> Handle(UpdateUnitTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get existing unit type
            var unitType = await _unitTypeRepository.GetFirstOrDefaultAsync(
                ut => ut.Id == request.Request.Id && !ut.IsDeleted);

            if (unitType == null)
            {
                return BaseResponse<UnitTypeDto>.Failure("Unit type not found.");
            }

            // Check if unit type name already exists for this project (excluding current unit type)
            var existingUnitType = await _unitTypeRepository.GetFirstOrDefaultAsync(
                ut => ut.ProjectId == unitType.ProjectId && 
                      ut.Name.ToLower() == request.Request.Name.ToLower() && 
                      ut.Id != request.Request.Id &&
                      !ut.IsDeleted);

            if (existingUnitType != null)
            {
                return BaseResponse<UnitTypeDto>.Failure("Unit type with this name already exists for this project.");
            }

            // Update unit type properties
            unitType.Name = request.Request.Name.Trim();
            unitType.Description = request.Request.Description?.Trim();
            unitType.DisplayOrder = request.Request.DisplayOrder;
            unitType.UpdateTimestamps();

            // Update unit type in database
            await _unitTypeRepository.UpdateAsync(unitType);
            await _unitTypeRepository.SaveChangesAsync();

            // Create DTO response
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

            return BaseResponse<UnitTypeDto>.Success(unitTypeDto, "Unit type updated successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<UnitTypeDto>.Failure($"Error updating unit type: {ex.Message}");
        }
    }
}
