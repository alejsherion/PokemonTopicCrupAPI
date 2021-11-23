using System.ComponentModel.DataAnnotations;

namespace WebAPICrudPokemon.DTO;

public class UserDTO
{
    [Required]
    [RegularExpression(@"^[a-z0-9_.!#$%&'*+-/=?^_`{|}~]{1,64}@[a-z0-9_.-]+\.[a-z]{1,4}$", 
        ErrorMessage = "Email must meet requirements")]
    public string Email { get; set; }

    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[@#?\]]).{10,20}$", 
        ErrorMessage = @"Password must meet requirements
Length:10-20
Contains: 1 Capital letter, 1 Lower letter, 1 Special caracter like ! @ # ? ]")]
    public string Password { get; set; }

    public string? Token { get; set; }
}
