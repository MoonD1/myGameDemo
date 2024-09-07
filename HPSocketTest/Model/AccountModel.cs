using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPSocketTest
{
    public class AccountModel
    {
        public int ID;
        public string Password;
        public string Account;
        public AccountModel() { }
        public AccountModel(int id, string account, string password)
        {
            ID = id;
            Password = password;
            Account = account;
        }
    }
}
