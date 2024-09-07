using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPSocketTest
{
    public class BLLMenager
    {
        private static BLLMenager instance;
        public static BLLMenager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BLLMenager();
                }
                return instance;
            }
        }
        public AccountBLL account = new AccountBLL();
        public UserBLL user = new UserBLL();
        public GameBLL game = new GameBLL();

        
    }
}
