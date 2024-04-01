using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PicPaySimplify.Data;
using PicPaySimplify.Models;
using PicPaySimplify.Models.DTOs;
using PicPaySimplify.Repositories.Interface;

namespace PicPaySimplify.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly PicPayDbContext _dbContext;
        private readonly IUserRepository _userRepository;

        public TransactionRepository(PicPayDbContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
        }

        public async Task<TransactionInfoUserDTO> CreateTransaction(TransactionDTO newTransaction)
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

                    var transactionModel = ConvertToTransactionModel(newTransaction);

                    _dbContext.Users.UpdateRange(payer, receiver);
                    await _dbContext.Transactions.AddAsync(transactionModel);
                    await _dbContext.SaveChangesAsync();

                    var transactionInfoUserDTO = ConvertToTransactionDTO(transactionModel, payer, receiver);

                    return transactionInfoUserDTO;
                }

                throw new Exception("The balance of Payer is not enough");
            }

            throw new Exception("Its not possible a MERCHANT make an transaction");
        }

        public TransactionInfoUserDTO ConvertToTransactionDTO(TransactionModel transactionModel, UserModel payer, UserModel receiver)
        {
            
            var transactionInfoUserDTO = new TransactionInfoUserDTO()
            {
                Id = transactionModel.Id,
                PayerId = payer.Id,
                Payer = new UserDTO()
                {
                    Id = payer.Id,
                    Name = payer.Name,
                    Email = payer.Email,
                    UserType = payer.UserType,
                    Balance = payer.Balance,
                },
                ReceiverId = receiver.Id,
                Receiver = new UserDTO()
                {
                    Id = receiver.Id,
                    Name = receiver.Name,
                    Email = receiver.Email,
                    UserType = receiver.UserType,
                    Balance = receiver.Balance,
                },
                Value = transactionModel.Value
            };

            return transactionInfoUserDTO;
        }

        public TransactionModel ConvertToTransactionModel(TransactionDTO newTransaction)
        {
            var transactionModel = new TransactionModel()
            {
                PayerId = newTransaction.PayerId,
                ReceiverId = newTransaction.ReceiverId,
                Value = newTransaction.Value
            };

            return transactionModel;
        }

        public async Task<List<TransactionInfoUserDTO>> GetAllTransactions()
        {
            return await _dbContext.Transactions.Select(x => new TransactionInfoUserDTO()
            {
                Id = x.Id,
                PayerId = x.PayerId,
                Payer = new UserDTO()
                {
                    Id = x.PayerId,
                    Name = x.Payer.Name,
                    Email = x.Payer.Email,
                    UserType = x.Payer.UserType,
                    Balance = x.Payer.Balance
                },
                ReceiverId = x.ReceiverId,
                Receiver = new UserDTO()
                {
                    Id = x.ReceiverId,
                    Name = x.Receiver.Name,
                    Email = x.Receiver.Email,
                    UserType = x.Receiver.UserType,
                    Balance = x.Receiver.Balance
                },
                Value = x.Value
            }).ToListAsync();

        }

        public async Task<bool> ValidateTransaction(int payerId)
        {
            var receiver = await _dbContext.Users.FindAsync(payerId);

            if (receiver.UserType == "MERCHANT") return false;

            return true;
        }
    }
}
