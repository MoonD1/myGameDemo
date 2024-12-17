using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HPSocket;
using HPSocket.Tcp;


namespace HPSocketTest
{
    public class Server
    {
        public static TcpServer server = new TcpServer();

        public Server() {
            server.Address = "127.0.0.1";
            server.Port = Convert.ToUInt16(5567);
            //接收到客户端连接
            server.OnAccept += Server_OnAccept;
            //客户端断开连接
            server.OnClose += Server_OnClose;
            //收到客户端发来的消息
            server.OnReceive += Server_OnReceive;

            server.Start();
            Console.WriteLine("服务端开启");
            
        }
        
        //当收到客户端消息时调用，data即为消息
        private HandleResult Server_OnReceive(IServer sender, IntPtr connId, byte[] data)
        {
            /*  test
            //测试接受客户端消息
            string msg = Encoding.UTF8.GetString(data);
            Console.WriteLine(msg);
            //给客户端回复一个消息
            byte[] data2 = Encoding.UTF8.GetBytes("服务端收到了");
            //测试给该客户端回消息
            server.Send(connId, data2, data2.Length);
            */
            Message message = MessageTool.ToObj(data);
            switch (message.type)
            {
                case Message.Type.Type_Account:
                    //分发到账号模块
                    BLLMenager.Instance.account.Server_OnReceive(connId, message);
                    break;
                case Message.Type.Type_User:
                    BLLMenager.Instance.user.Server_OnReceive(connId, message);
                    break;
                case Message.Type.Type_Game:
                    BLLMenager.Instance.game.Server_OnReceive (connId, message);
                    break;
            }
            return HandleResult.Ok;
        }

        //发送消息
        public static void Send(IntPtr ptr, byte type, int command, params object[] objects)
        {
            Message msg = new Message(type, command, objects);
            byte[] data = MessageTool.ToByte(msg);
            server.Send(ptr, data, data.Length);

        }

        private HandleResult Server_OnClose(IServer sender, IntPtr connId, SocketOperation socketOperation, int errorCode)
        {
            Console.WriteLine("有客户端关闭");
            //如果一个客户端关闭了，对所有模块都分发（关闭不区分模块，能关全关）
            BLLMenager.Instance.account.Server_OnClose(connId);
            BLLMenager.Instance.user.Server_OnClose(connId);
            BLLMenager.Instance.game.Server_OnClose(connId);
            return HandleResult.Ok;
        }

        private HandleResult Server_OnAccept(IServer sender, IntPtr connId, IntPtr client)
        {
            
            Console.WriteLine("有客户端成功连接");
            
            return HandleResult.Ok;
        }
    }
}
