namespace PicPaySimplify.Models.DTOs
{
    public class TransactionInfoUserDTO
    {
        public int Id { get; set; }
        public int PayerId { get; set; }
        public UserDTO? Payer { get; set; }
        public int ReceiverId { get; set; }
        public UserDTO? Receiver { get; set;}
        public double Value { get; set; }
    }
}
