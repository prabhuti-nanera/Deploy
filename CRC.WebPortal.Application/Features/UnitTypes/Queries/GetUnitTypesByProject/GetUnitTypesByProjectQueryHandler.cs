using CRC.WebPortal.Application.Common.Dtos;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitTypes.Queries.GetUnitTypesByProject;

public class GetUnitTypesByProjectQueryHandler : IRequestHandler<GetUnitTypesByProjectQuery, BaseResponse<List<UnitTypeDto>>>
{
    private readonly IRepository<UnitType> _unitTypeRepository;

    public GetUnitTypesByProjectQueryHandler(IRepository<UnitType> unitTypeRepository)
    {
        _unitTypeRepository = unitTypeRepository;
    }

    public async Task<BaseResponse<List<UnitTypeDto>>> Handle(GetUnitTypesByProjectQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all unit types for the project
            var unitTypes = await _unitTypeRepository.GetAllAsync(
                filter: ut => ut.ProjectId == request.ProjectId && !ut.IsDeleted,
                orderBy: q => q.OrderBy(ut => ut.DisplayOrder).ThenBy(ut => ut.Name));

            // Convert to DTOs
            var unitTypeDtos = unitTypes.Select(ut => new UnitTypeDto
            {
                Id = ut.Id,
                Name = ut.Name,
                Description = ut.Description,
                DisplayOrder = ut.DisplayOrder,
                ProjectId = ut.ProjectId,
                CreatedAt = ut.CreatedAt,
                UpdatedAt = ut.UpdatedAt,
                IsActive = ut.IsActive
            }).ToList();

            return BaseResponse<List<UnitTypeDto>>.Success(unitTypeDtos, "Unit types retrieved successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<List<UnitTypeDto>>.Failure($"Error retrieving unit types: {ex.Message}");
        }
    }
}
