using System.ComponentModel.DataAnnotations;

namespace Domain.Models;
public class Dog
{
    [Key]
    public string Name { get; set; }

    [Required]
    public string Color { get; set; }

    [Required]
    public int TailLength { get; set; }

    [Required]
    public int Weight { get; set; }
}
