using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HPSocketTest
{
    public class UserBLL : IMessageHandle
    {
        public void Server_OnClose(IntPtr connId)
        {
            //用户下线
            //得到下线用户
            UserModel model = DALMenager.Instance.user.GetUserModel(connId);
            //移除角色
            DALMenager.Instance.user.RemoveUser(connId);
            //通知所有客户端，这个用户下线了
            foreach(IntPtr client in DALMenager.Instance.user.GetUserPtrs())
            {
                Server.Send(client, Message.Type.Type_User, Message.Type.User_RemoveS, model.ID);
            }
        }

        public void Server_OnReceive(IntPtr connId, Message msg)
        {
            switch (msg.command)
            {
                case Message.Type.User_SelectC:
                    //进入游戏前需要渲染其他玩家
                    CreateOtherUser(connId, msg);
                    //创建自己
                    CreateSelfUser(connId, msg);
                    //告诉其他玩家创建自己
                    CreateUserByOthers(connId, msg);
                    break;
            }
        }
        void CreateOtherUser(IntPtr ptr, Message msg)
        {
            //Console.WriteLine("test1");
            //遍历其他玩家并创建
            foreach(UserModel model in DALMenager.Instance.user.GetUserModels())
            {
                Server.Send(ptr, Message.Type.Type_User, Message.Type.User_CreateS, model.ID, model.userInfo.ModelID, model.Points);
                Thread.Sleep(500);
            }
        }

        void CreateSelfUser(IntPtr ptr, Message msg)
        {
            int modelid = msg.GetContent<int>(0);
            //创建玩家模型
            int userid = DALMenager.Instance.user.AddUser(ptr, modelid);
            //Console.WriteLine(userid);
            //Console.WriteLine(modelid);
            //初始化位置坐标
            UserModel model = DALMenager.Instance.user.GetUserModel(userid);
            model.Points = msg.GetContent<float[]>(1);
            //Console.WriteLine("test2");
            Server.Send(ptr, Message.Type.Type_User, Message.Type.User_SelectS, userid, modelid, model.Points);
        }

        void CreateUserByOthers(IntPtr ptr, Message msg)
        {
            //Console.WriteLine("test3");
            UserModel model = DALMenager.Instance.user.GetUserModel(ptr);
            foreach(IntPtr client in DALMenager.Instance.user.GetUserPtrs())
            {
                if(client == ptr)
                {
                    //如果是自己就不创建
                    continue;
                }
                Server.Send(client, Message.Type.Type_User, Message.Type.User_CreateS, model.ID, model.userInfo.ModelID, model.Points);
            }
        }
    }
}
