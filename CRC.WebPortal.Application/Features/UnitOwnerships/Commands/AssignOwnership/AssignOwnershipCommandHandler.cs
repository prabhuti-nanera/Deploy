using CRC.WebPortal.Application.Common.Dtos;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitOwnerships.Commands.AssignOwnership;

public class AssignOwnershipCommandHandler : IRequestHandler<AssignOwnershipCommand, BaseResponse<UnitOwnershipDto>>
{
    private readonly IRepository<UnitOwnership> _ownershipRepository;
    private readonly IRepository<CRC.WebPortal.Domain.Entities.Unit> _unitRepository;

    public AssignOwnershipCommandHandler(
        IRepository<UnitOwnership> ownershipRepository,
        IRepository<CRC.WebPortal.Domain.Entities.Unit> unitRepository)
    {
        _ownershipRepository = ownershipRepository;
        _unitRepository = unitRepository;
    }

    public async Task<BaseResponse<UnitOwnershipDto>> Handle(AssignOwnershipCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate unit exists
            var unit = await _unitRepository.GetByIdAsync(request.Request.UnitId);
            if (unit == null)
            {
                return BaseResponse<UnitOwnershipDto>.Failure("Unit not found.");
            }

            // Check if unit is already sold
            var existingOwnership = await _ownershipRepository.GetFirstOrDefaultAsync(
                o => o.UnitId == request.Request.UnitId && !o.IsDeleted);

            if (existingOwnership != null)
            {
                return BaseResponse<UnitOwnershipDto>.Failure("Unit is already assigned to an owner.");
            }

            // Create ownership entity
            var ownership = new UnitOwnership
            {
                OwnerName = request.Request.OwnerName.Trim(),
                OwnerEmail = request.Request.OwnerEmail.Trim().ToLower(),
                OwnerMobile = request.Request.OwnerMobile.Trim(),
                SellingDetails = request.Request.SellingDetails?.Trim(),
                SalePrice = request.Request.SalePrice,
                SaleDate = request.Request.SaleDate.HasValue 
                    ? (request.Request.SaleDate.Value.Kind == DateTimeKind.Unspecified 
                        ? DateTime.SpecifyKind(request.Request.SaleDate.Value, DateTimeKind.Utc)
                        : request.Request.SaleDate.Value.ToUniversalTime())
                    : DateTime.UtcNow,
                SaleStatus = request.Request.SaleStatus,
                UnitId = request.Request.UnitId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            // Update unit ownership status
            unit.OwnershipStatus = request.Request.SaleStatus;
            unit.UpdatedAt = DateTime.UtcNow;

            // Add ownership to database
            await _ownershipRepository.AddAsync(ownership);
            await _unitRepository.UpdateAsync(unit);
            await _ownershipRepository.SaveChangesAsync();

            // Create DTO response
            var ownershipDto = new UnitOwnershipDto
            {
                Id = ownership.Id,
                OwnerName = ownership.OwnerName,
                OwnerEmail = ownership.OwnerEmail,
                OwnerMobile = ownership.OwnerMobile,
                SellingDetails = ownership.SellingDetails,
                SalePrice = ownership.SalePrice,
                SaleDate = ownership.SaleDate,
                SaleStatus = ownership.SaleStatus,
                UnitId = ownership.UnitId,
                CreatedAt = ownership.CreatedAt,
                UpdatedAt = ownership.UpdatedAt,
                IsActive = ownership.IsActive
            };

            return BaseResponse<UnitOwnershipDto>.Success(ownershipDto, "Unit ownership assigned successfully.");
        }
        catch (Exception ex)
        {
            var innerException = ex.InnerException?.Message ?? "No inner exception";
            var fullMessage = $"Error assigning unit ownership: {ex.Message}. Inner: {innerException}";
            return BaseResponse<UnitOwnershipDto>.Failure(fullMessage);
        }
    }
}
