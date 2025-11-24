using CRC.WebPortal.Application.Common.Dtos;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitTypes.Commands.CreateUnitType;

public class CreateUnitTypeCommandHandler : IRequestHandler<CreateUnitTypeCommand, BaseResponse<UnitTypeDto>>
{
    private readonly IRepository<UnitType> _unitTypeRepository;
    private readonly IRepository<Project> _projectRepository;

    public CreateUnitTypeCommandHandler(
        IRepository<UnitType> unitTypeRepository,
        IRepository<Project> projectRepository)
    {
        _unitTypeRepository = unitTypeRepository;
        _projectRepository = projectRepository;
    }

    public async Task<BaseResponse<UnitTypeDto>> Handle(CreateUnitTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate project exists
            var projectExists = await _projectRepository.AnyAsync(p => p.Id == request.Request.ProjectId);
            if (!projectExists)
            {
                return BaseResponse<UnitTypeDto>.Failure("Project not found.");
            }

            // Check if unit type name already exists for this project
            var existingUnitType = await _unitTypeRepository.GetFirstOrDefaultAsync(
                ut => ut.ProjectId == request.Request.ProjectId && 
                      ut.Name.ToLower() == request.Request.Name.ToLower() && 
                      !ut.IsDeleted);

            if (existingUnitType != null)
            {
                return BaseResponse<UnitTypeDto>.Failure("Unit type with this name already exists for this project.");
            }

            // Create unit type entity
            var unitType = new UnitType
            {
                Name = request.Request.Name.Trim(),
                Description = request.Request.Description?.Trim(),
                DisplayOrder = request.Request.DisplayOrder,
                ProjectId = request.Request.ProjectId
            };

            // Add unit type to database
            await _unitTypeRepository.AddAsync(unitType);
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

            return BaseResponse<UnitTypeDto>.Success(unitTypeDto, "Unit type created successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<UnitTypeDto>.Failure($"Error creating unit type: {ex.Message}");
        }
    }
}
