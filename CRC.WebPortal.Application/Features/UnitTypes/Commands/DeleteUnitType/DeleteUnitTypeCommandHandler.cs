using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitTypes.Commands.DeleteUnitType;

public class DeleteUnitTypeCommandHandler : IRequestHandler<DeleteUnitTypeCommand, BaseResponse<bool>>
{
    private readonly IRepository<UnitType> _unitTypeRepository;
    private readonly IRepository<Domain.Entities.Unit> _unitRepository;

    public DeleteUnitTypeCommandHandler(
        IRepository<UnitType> unitTypeRepository,
        IRepository<Domain.Entities.Unit> unitRepository)
    {
        _unitTypeRepository = unitTypeRepository;
        _unitRepository = unitRepository;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteUnitTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get existing unit type
            var unitType = await _unitTypeRepository.GetFirstOrDefaultAsync(
                ut => ut.Id == request.Id && !ut.IsDeleted);

            if (unitType == null)
            {
                return BaseResponse<bool>.Failure("Unit type not found.");
            }

            // Check if unit type is being used by any units
            var unitsUsingType = await _unitRepository.AnyAsync(u => u.UnitTypeId == request.Id && !u.IsDeleted);
            if (unitsUsingType)
            {
                return BaseResponse<bool>.Failure("Cannot delete unit type as it is being used by existing units.");
            }

            // Soft delete the unit type
            unitType.IsDeleted = true;
            unitType.UpdateTimestamps();

            await _unitTypeRepository.UpdateAsync(unitType);
            await _unitTypeRepository.SaveChangesAsync();

            return BaseResponse<bool>.Success(true, "Unit type deleted successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.Failure($"Error deleting unit type: {ex.Message}");
        }
    }
}
