using CRC.WebPortal.Application.Common.Dtos;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitOwnerships.Commands.UpdateOwnership;

public class UpdateOwnershipCommandHandler : IRequestHandler<UpdateOwnershipCommand, BaseResponse<UnitOwnershipDto>>
{
    private readonly IRepository<UnitOwnership> _ownershipRepository;
    private readonly IRepository<CRC.WebPortal.Domain.Entities.Unit> _unitRepository;

    public UpdateOwnershipCommandHandler(
        IRepository<UnitOwnership> ownershipRepository,
        IRepository<CRC.WebPortal.Domain.Entities.Unit> unitRepository)
    {
        _ownershipRepository = ownershipRepository;
        _unitRepository = unitRepository;
    }

    public async Task<BaseResponse<UnitOwnershipDto>> Handle(UpdateOwnershipCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get existing ownership
            var ownership = await _ownershipRepository.GetByIdAsync(request.Request.Id);
            if (ownership == null || ownership.IsDeleted)
            {
                return BaseResponse<UnitOwnershipDto>.Failure("Unit ownership not found.");
            }

            // Get associated unit
            var unit = await _unitRepository.GetByIdAsync(ownership.UnitId);
            if (unit == null)
            {
                return BaseResponse<UnitOwnershipDto>.Failure("Associated unit not found.");
            }

            // Update ownership details
            ownership.OwnerName = request.Request.OwnerName.Trim();
            ownership.OwnerEmail = request.Request.OwnerEmail.Trim().ToLower();
            ownership.OwnerMobile = request.Request.OwnerMobile.Trim();
            ownership.SellingDetails = request.Request.SellingDetails?.Trim();
            ownership.SalePrice = request.Request.SalePrice;
            ownership.SaleDate = request.Request.SaleDate;
            ownership.SaleStatus = request.Request.SaleStatus;
            ownership.UpdateTimestamps();

            // Update unit ownership status if changed
            if (unit.OwnershipStatus != request.Request.SaleStatus)
            {
                unit.OwnershipStatus = request.Request.SaleStatus;
                unit.UpdateTimestamps();
                await _unitRepository.UpdateAsync(unit);
            }

            // Update ownership in database
            await _ownershipRepository.UpdateAsync(ownership);
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

            return BaseResponse<UnitOwnershipDto>.Success(ownershipDto, "Unit ownership updated successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<UnitOwnershipDto>.Failure($"Error updating unit ownership: {ex.Message}");
        }
    }
}
