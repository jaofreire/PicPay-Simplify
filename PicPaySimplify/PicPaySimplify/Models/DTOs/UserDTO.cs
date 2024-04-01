namespace PicPaySimplify.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? UserType { get; set; }
        public double Balance { get; set; }
    }
}
