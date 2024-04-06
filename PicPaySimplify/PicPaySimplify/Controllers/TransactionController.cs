using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PicPaySimplify.Models;
using PicPaySimplify.Models.DTOs;
using PicPaySimplify.Repositories.Interface;

namespace PicPaySimplify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpPost("/transaction/create/")]
        public async Task<ActionResult<TransactionInfoUserDTO>> CreateNewTransaction(TransactionDTO transaction)
        {
            try
            {
                return await _transactionRepository.CreateTransaction(transaction);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/transaction/getAll")]
        public async Task<ActionResult<List<TransactionInfoUserDTO>>> GetAll()
        {
            try
            {
                return await _transactionRepository.GetAllTransactions();

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
