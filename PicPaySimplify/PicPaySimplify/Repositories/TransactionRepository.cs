using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PicPaySimplify.Data;
using PicPaySimplify.Models;
using PicPaySimplify.Models.DTOs;
using PicPaySimplify.Repositories.Interface;
using PicPaySimplify.Services.AWSServices;
using System.ComponentModel.DataAnnotations;

namespace PicPaySimplify.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly PicPayDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly SESWrapper _ses;

        public TransactionRepository(PicPayDbContext dbContext, IUserRepository userRepository, SESWrapper ses)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _ses = ses;
        }

        public async Task<TransactionInfoUserDTO> CreateTransaction(TransactionDTO newTransaction)
        {
            if (ValidateTransaction(newTransaction.PayerId).Result)
            {
                var value = newTransaction.Value;
                var payer = await _userRepository.GetUserById(newTransaction.PayerId);
                var receiver = await _userRepository.GetUserById(newTransaction.ReceiverId);
                List<string> emailsList = [payer.Email];
               
                if (payer.Balance >= value)
                {
                    payer.Balance -= value;
                    receiver.Balance += value;

                    var transactionModel = ConvertToTransactionModel(newTransaction);

                    _dbContext.Users.UpdateRange(payer, receiver);
                    await _dbContext.Transactions.AddAsync(transactionModel);
                    await _dbContext.SaveChangesAsync();

                    var transactionInfoUserDTO = ConvertToTransactionDTO(transactionModel, payer, receiver);
                    SendEmailTransaction(emailsList,"Transaction Successfully", payer.Name, receiver.Name, value, transactionInfoUserDTO.Id);

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

        public async void SendEmailTransaction(List<string>emailTo, string subject, string payerName, string receiverName, double value, int transactionId)
        {
            var body = "Hello " + payerName + ", your transaction for " + receiverName + " with value " + "R$ " + value + " is successfully!!" + " Transaction ID : " + transactionId;
            var emailFrom = "joao.gabriel18872@gmail.com";

            var emailRequest = _ses.EmailRequest(emailFrom, emailTo, subject, body);
            await _ses.SendEmail(emailRequest);
        }

        public async Task<bool> ValidateTransaction(int payerId)
        {
            var receiver = await _dbContext.Users.FindAsync(payerId);

            if (receiver.UserType == "MERCHANT") return false;

            return true;
        }
    }
}
