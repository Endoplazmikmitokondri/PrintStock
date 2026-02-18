using PrintStock.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PrintStock.Application.DTOs;

public class AdjustStockDto
{
    [Range(0.01, 100000, ErrorMessage = "Please enter a value between 0.01 and 100,000")]
    public double Amount { get; set; }
    
    public WeightUnit Unit { get; set; }
}

