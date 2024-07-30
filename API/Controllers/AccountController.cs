using API.DTO;
using DAL.DAO;
using DAL.DB;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountDAO _accountDao;

        public AccountsController(AccountDAO accountDao)
        {
            _accountDao = accountDao;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var account = _accountDao.Login(loginDto.Email, loginDto.Password);
            if (account == null)
            {
                return Unauthorized();
            }
            return Ok(account);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int accountID)
        {
            var account = _accountDao.GetById(accountID);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Account newAccount)
        {
            var account = new Account(newAccount.AccountID, newAccount.UserName, newAccount.Email, newAccount.Password);
            var newAccountId = _accountDao.Add(account);
            account.AccountID = newAccountId;
            return CreatedAtAction(nameof(GetById), new { id = newAccountId }, account);
        }
    }
}
