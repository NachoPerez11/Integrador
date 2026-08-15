using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; }
    public string Name { get; private set; }
    public string PasswordHash { get; private set; }
    public string Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Constructor privado para EF Core
    private User()
    {
        Email = string.Empty;
        Name = string.Empty;
        PasswordHash = string.Empty;
        Role = "User";
    }

    // Factory method — única forma de crear un usuario válido
    public static User Create(string email, string name, string passwordHash, string role = "User")
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainRuleException("El email es obligatorio.");
            
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainRuleException("El nombre es obligatorio.");
            
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainRuleException("El hash de la contraseña es obligatorio.");

        return new User
        {
            Email = email.Trim(),
            Name = name.Trim(),
            PasswordHash = passwordHash,
            Role = string.IsNullOrWhiteSpace(role) ? "User" : role.Trim(),
            CreatedAt = DateTime.UtcNow
        };
    }

    public void ChangeRole(string newRole)
    {
        if (string.IsNullOrWhiteSpace(newRole))
            throw new DomainRuleException("El nuevo rol es obligatorio.");

        Role = newRole.Trim();
    }
}