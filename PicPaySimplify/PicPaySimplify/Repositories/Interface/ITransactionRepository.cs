using PicPaySimplify.Models;

namespace PicPaySimplify.Repositories.Interface
{
    public interface ITransactionRepository
    {
        Task<TransactionModel> CreateTransaction(TransactionModel newTransaction);
        Task<bool> ValidateTransaction(int payerId);
        Task<TransactionModel> UndoneTransaction(int transactionId);
    }
}
