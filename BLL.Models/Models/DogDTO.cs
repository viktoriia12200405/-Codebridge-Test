using Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models.Models;
public class DogDTO : IValidatableObject
{
    public string Name { get; set; }
    
    public string Color { get; set; }
    
    public int Tail_length { get; set; }

    public int Weight { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Name.IsNullOrEmpty())
            yield return new ValidationResult("Name is required.", new[] { nameof(Name) });

        if (Color.IsNullOrEmpty())
            yield return new ValidationResult("Color is required.", new[] { nameof(Color) });

        if (!Tail_length.IsPositive())
            yield return new ValidationResult("Tail length must be a number and greater than 0.", new[] { nameof(Tail_length) });

        if (!Weight.IsPositive())
            yield return new ValidationResult("Weigth must be a number and greater than 0.", new[] { nameof(Weight) });
    }
}
