using CRC.WebPortal.Shared.Dtos;
using CRC.WebPortal.Shared.Requests;
using CRC.Common.Models;

namespace CRC.WebPortal.BlazorWebApp.Services;

public interface IUnitTypeService
{
    Task<BaseResponse<List<UnitTypeDto>>> GetUnitTypesByProjectAsync(int projectId);
    Task<BaseResponse<UnitTypeDto>> GetUnitTypeByIdAsync(int id);
    Task<BaseResponse<UnitTypeDto>> CreateUnitTypeAsync(CreateUnitTypeRequest request);
    Task<BaseResponse<UnitTypeDto>> UpdateUnitTypeAsync(UpdateUnitTypeRequest request);
    Task<BaseResponse<bool>> DeleteUnitTypeAsync(int id);
    Task<BaseResponse<UnitTypeDto>> CloneUnitTypeAsync(CloneUnitTypeRequest request);
}
