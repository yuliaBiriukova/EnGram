namespace EnGram.DB.Entities;

public class User
{
    public int Id { get; set; }

    public string Email { get; set; }

    public UserRole Role { get; set; }
}