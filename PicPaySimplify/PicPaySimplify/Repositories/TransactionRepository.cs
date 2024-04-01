using Microsoft.AspNetCore.Http.HttpResults;
using PicPaySimplify.Data;
using PicPaySimplify.Models;
using PicPaySimplify.Repositories.Interface;

namespace PicPaySimplify.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly PicPayDbContext _dbContext;
        private readonly IUserRepository _userRepository;

        public TransactionRepository(PicPayDbContext dbContext, IUserRepository userRepository = null)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }

        public async Task<TransactionModel> CreateTransaction(TransactionModel newTransaction)
        {
            if (ValidateTransaction(newTransaction.PayerId).Result)
            {
                var value = newTransaction.Value;
                var payer = await _userRepository.GetUserById(newTransaction.PayerId);
                var receiver = await _userRepository.GetUserById(newTransaction.ReceiverId);

                if (payer.Balance >= value)
                {
                    payer.Balance -= value;
                    receiver.Balance += value;

                    _dbContext.Users.UpdateRange(payer, receiver);

                    await _dbContext.Transactions.AddAsync(newTransaction);
                    await _dbContext.SaveChangesAsync();

                    return newTransaction;
                }

                throw new Exception("The balance of Payer is not enough");
            }

            throw new Exception("Its not possible a MERCHANT make an transaction");
        }

        public Task<TransactionModel> UndoneTransaction(int transactionId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateTransaction(int payerId)
        {
            var receiver = await _dbContext.Users.FindAsync(payerId);

            if (receiver.UserType == "MERCHANT") return false;

            return true;
        }
    }
}
