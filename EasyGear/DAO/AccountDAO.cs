using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public interface AccountDAO
    {
        Account? Login(string email, string password);
        Account? GetById(int accountID);
        int Add(Account account);
    }
}


