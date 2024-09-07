using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPSocketTest
{
    public class AccountBLL : IMessageHandle
    {
        public void Server_OnClose(IntPtr connId)
        {
            DALMenager.Instance.account.Logout(connId);
        }

        public void Server_OnReceive(IntPtr connId, Message msg)
        {
            switch (msg.command)
            {
                case Message.Type.Account_LoginC:
                    Login(connId, msg);
                    break;
                case Message.Type.Account_RegistC:
                    Register(connId, msg);
                    break;
            }
        }
        //注册
        void Register(IntPtr connId, Message message)
        {
            int res = DALMenager.Instance.account.Add(message.GetContent<string>(0), message.GetContent<string>(1));
            //给客户端相应 res:1成功
            Server.Send(connId, Message.Type.Type_Account, Message.Type.Account_RegistS, res);
        }
        //登录
        void Login(IntPtr connId, Message message)
        {
            int id = DALMenager.Instance.account.Login(connId, message.GetContent<string>(0), message.GetContent<string>(1));
            Server.Send(connId, Message.Type.Type_Account, Message.Type.Account_LoginS, id);
        }
    }
}
