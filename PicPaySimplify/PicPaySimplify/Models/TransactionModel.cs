using PicPaySimplify.Models.DTOs;

namespace PicPaySimplify.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }
        public int PayerId { get; set; }
        public UserModel? Payer { get; set; }
        public int ReceiverId { get; set; }
        public UserModel? Receiver { get; set; }
        public double Value { get; set; }
    
    }
}
