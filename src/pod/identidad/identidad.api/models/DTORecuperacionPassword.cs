using Microsoft.AspNetCore.Identity;

namespace contabee.identity.api.models;

public class DTORecuperacionPassword
{    
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? TokenRecuperacion { get; set; }
}
