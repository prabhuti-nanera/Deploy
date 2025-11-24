using CRC.WebPortal.Shared.Dtos;
using CRC.WebPortal.Shared.Requests;
using CRC.Common.Models;
using System.Net.Http.Json;

namespace CRC.WebPortal.BlazorWebApp.Services;

public class UnitTypeService : IUnitTypeService
{
    private readonly HttpClient _httpClient;

    public UnitTypeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BaseResponse<List<UnitTypeDto>>> GetUnitTypesByProjectAsync(int projectId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<List<UnitTypeDto>>>($"api/UnitType/project/{projectId}");
            return response ?? BaseResponse<List<UnitTypeDto>>.Failure("Failed to retrieve unit types");
        }
        catch (Exception ex)
        {
            return BaseResponse<List<UnitTypeDto>>.Failure($"Error: {ex.Message}");
        }
    }

    public async Task<BaseResponse<UnitTypeDto>> GetUnitTypeByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<UnitTypeDto>>($"api/UnitType/{id}");
            return response ?? BaseResponse<UnitTypeDto>.Failure("Failed to retrieve unit type");
        }
        catch (Exception ex)
        {
            return BaseResponse<UnitTypeDto>.Failure($"Error: {ex.Message}");
        }
    }

    public async Task<BaseResponse<UnitTypeDto>> CreateUnitTypeAsync(CreateUnitTypeRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/UnitType", request);
            var result = await response.Content.ReadFromJsonAsync<BaseResponse<UnitTypeDto>>();
            return result ?? BaseResponse<UnitTypeDto>.Failure("Failed to create unit type");
        }
        catch (Exception ex)
        {
            return BaseResponse<UnitTypeDto>.Failure($"Error: {ex.Message}");
        }
    }

    public async Task<BaseResponse<UnitTypeDto>> UpdateUnitTypeAsync(UpdateUnitTypeRequest request)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/UnitType/{request.Id}", request);
            var result = await response.Content.ReadFromJsonAsync<BaseResponse<UnitTypeDto>>();
            return result ?? BaseResponse<UnitTypeDto>.Failure("Failed to update unit type");
        }
        catch (Exception ex)
        {
            return BaseResponse<UnitTypeDto>.Failure($"Error: {ex.Message}");
        }
    }

    public async Task<BaseResponse<bool>> DeleteUnitTypeAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/UnitType/{id}");
            var result = await response.Content.ReadFromJsonAsync<BaseResponse<bool>>();
            return result ?? BaseResponse<bool>.Failure("Failed to delete unit type");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.Failure($"Error: {ex.Message}");
        }
    }

    public async Task<BaseResponse<UnitTypeDto>> CloneUnitTypeAsync(CloneUnitTypeRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/UnitType/clone", request);
            var result = await response.Content.ReadFromJsonAsync<BaseResponse<UnitTypeDto>>();
            return result ?? BaseResponse<UnitTypeDto>.Failure("Failed to clone unit type");
        }
        catch (Exception ex)
        {
            return BaseResponse<UnitTypeDto>.Failure($"Error: {ex.Message}");
        }
    }
}
