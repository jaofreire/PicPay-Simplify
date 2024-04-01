using PicPaySimplify.Models;
using PicPaySimplify.Models.DTOs;

namespace PicPaySimplify.Repositories.Interface
{
    public interface ITransactionRepository
    {
        Task<List<TransactionInfoUserDTO>> GetAllTransactions();
        Task<TransactionInfoUserDTO> CreateTransaction(TransactionDTO newTransaction);
        Task<bool> ValidateTransaction(int payerId);
    }
}
