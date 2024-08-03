using API.DTO;
using DAL.DAO;
using DAL.DB;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        AccountDAO _accountDao;
        ILogger<AccountsController> _logger;

        public AccountsController(AccountDAO accountDao, ILogger<AccountsController> logger)
        {
            _accountDao = accountDao;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);

            var account = _accountDao.Login(loginDto.Email, loginDto.Password);
            if (account == null)
            {
                _logger.LogWarning("Invalid login attempt for email: {Email}", loginDto.Email);
                return Unauthorized();
            }
            return Ok(account);
        }

        [HttpGet("{accountID}")]
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
            var account = new Account(newAccount.AccountID, newAccount.Email, newAccount.Password);
            var newAccountId = _accountDao.Add(account);
            account.AccountID = newAccountId;
            return CreatedAtAction(nameof(GetById), new { accountID = newAccountId }, account);
        }
    }
}



