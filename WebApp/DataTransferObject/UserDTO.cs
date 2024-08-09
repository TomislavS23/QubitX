namespace WebApp.DataTransferObjects;

public class UserDTO
{
    public int IdUser { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Username { get; set; }

    public string? HashedPassword { get; set; }

    public int? IdRole { get; set; }
}