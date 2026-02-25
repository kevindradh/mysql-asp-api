using System.ComponentModel.DataAnnotations;

namespace CobaMySql.Models;

public class Category
{
    public int Id { get; set; }

    [Required] 
    public string Name { get; set; } = "";

    public string? Description { get; set; } = "";
    
    public string Slug { get; set; } = "";
    
    public List<Product> Products { get; set; } = [];
}