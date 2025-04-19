using System;

namespace FilteringSortingApi.Models;

public class ProductFilter
{
    public int? CategoryId { get; set; }
    public string CategoryName { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? InStock { get; set; }
    public string? NameContains { get; set; }
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }
    public string SortBy { get; set; } = "Name";
    public string SortOrder { get; set; } = "asc";
}
