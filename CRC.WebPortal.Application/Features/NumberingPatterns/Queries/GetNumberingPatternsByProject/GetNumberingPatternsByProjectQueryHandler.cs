using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using CRC.WebPortal.Shared.Dtos;
using MediatR;

namespace CRC.WebPortal.Application.Features.NumberingPatterns.Queries.GetNumberingPatternsByProject;

public class GetNumberingPatternsByProjectQueryHandler : IRequestHandler<GetNumberingPatternsByProjectQuery, BaseResponse<List<UnitNumberingPatternDto>>>
{
    private readonly IRepository<NumberingPattern> _numberingPatternRepository;
    private readonly IRepository<NumberingPatternRow> _patternRowRepository;

    public GetNumberingPatternsByProjectQueryHandler(
        IRepository<NumberingPattern> numberingPatternRepository,
        IRepository<NumberingPatternRow> patternRowRepository)
    {
        _numberingPatternRepository = numberingPatternRepository;
        _patternRowRepository = patternRowRepository;
    }

    private static PatternRowDto ConvertToPatternRowDto(NumberingPatternRow pr)
    {
        var patternRow = new PatternRowDto
        {
            UnitType = pr.UnitType,
            FirstDigit = pr.FirstDigit,
            SecondDigit = pr.SecondDigit,
            ThirdDigit = pr.ThirdDigit,
            CustomFirstDigit = pr.CustomFirstDigit,
            CustomSecondDigit = pr.CustomSecondDigit,
            CustomThirdDigit = pr.CustomThirdDigit,
            Result = pr.Result,
            OriginalDigitCount = pr.OriginalDigitCount
        };
        
        // Parse dynamic digits from JSON
        if (!string.IsNullOrEmpty(pr.DigitsJson))
        {
            try
            {
                patternRow.Digits = System.Text.Json.JsonSerializer.Deserialize<List<string>>(pr.DigitsJson) ?? new List<string>();
            }
            catch
            {
                patternRow.Digits = new List<string>();
            }
        }
        
        if (!string.IsNullOrEmpty(pr.CustomDigitsJson))
        {
            try
            {
                patternRow.CustomDigits = System.Text.Json.JsonSerializer.Deserialize<List<string>>(pr.CustomDigitsJson) ?? new List<string>();
            }
            catch
            {
                patternRow.CustomDigits = new List<string>();
            }
        }
        
        return patternRow;
    }

    public async Task<BaseResponse<List<UnitNumberingPatternDto>>> Handle(GetNumberingPatternsByProjectQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all numbering patterns for the project
            var numberingPatterns = await _numberingPatternRepository.GetAllAsync(
                filter: np => np.ProjectId == request.ProjectId,
                orderBy: q => q.OrderBy(np => np.Id));

            var patternDtos = new List<UnitNumberingPatternDto>();

            foreach (var pattern in numberingPatterns)
            {
                // Get pattern rows for this numbering pattern
                var patternRows = await _patternRowRepository.GetAllAsync(
                    filter: pr => pr.NumberingPatternId == pattern.Id,
                    orderBy: q => q.OrderBy(pr => pr.Id));

                var patternDto = new UnitNumberingPatternDto
                {
                    StartFloorNumber = pattern.StartFloorNumber,
                    UnitNameDigits = pattern.UnitNameDigits,
                    ApplyTo = pattern.ApplyTo,
                    PatternRows = patternRows.Select(ConvertToPatternRowDto).ToList()
                };

                patternDtos.Add(patternDto);
            }

            return BaseResponse<List<UnitNumberingPatternDto>>.Success(patternDtos, "Numbering patterns retrieved successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<List<UnitNumberingPatternDto>>.Failure($"Error retrieving numbering patterns: {ex.Message}");
        }
    }
}
