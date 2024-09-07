using HPSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IMessageHandle
{
    void Server_OnReceive(IntPtr connId, Message msg);

    void Server_OnClose(IntPtr connId);
}