using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPSocketTest
{
    public class DALMenager
    {
        private static DALMenager instance;
        public static DALMenager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DALMenager();
                }
                return instance;
            }
        }
        public AccountDAL account = new AccountDAL();
        public UserDAL user = new UserDAL();
    }
}
