using System;

namespace FilteringSortingApi.Models;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CategoryDto Category { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool InStock { get; set; }
}
