using ECommerce.Domain.Entities;

namespace ECommerce.Application.Contracts.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}