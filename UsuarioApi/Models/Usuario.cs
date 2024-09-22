namespace UsuarioApi.Models;

public class Usuario
{
    public string Email { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "Admin";
}