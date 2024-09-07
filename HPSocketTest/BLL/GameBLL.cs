using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPSocketTest
{
    public class GameBLL : IMessageHandle
    {

        public void Server_OnClose(IntPtr connId)
        {
            
        }

        public void Server_OnReceive(IntPtr connId, Message msg)
        {
            switch (msg.command) 
            {
                case Message.Type.Game_MoveC:
                    Move(connId, msg);
                    break;
                case Message.Type.Game_AttackC:
                    Attack(connId, msg);
                    break;
            }
        }

        void Move(IntPtr connId, Message msg)
        {
            //获得要移动的用户和目标地点
            int userid = msg.GetContent<int>(0);
            float[] targetPos = msg.GetContent<float[]>(1);
            UserModel user = DALMenager.Instance.user.GetUserModel(userid);
            user.Points = targetPos;
            //通知所有客户端这个用户要移动到这个点
            foreach(IntPtr ptr in DALMenager.Instance.user.GetUserPtrs())
            {
                //Console.WriteLine("Test4");
                Server.Send(ptr, Message.Type.Type_Game, Message.Type.Game_MoveS, userid, targetPos);
            }
        }

        void Attack(IntPtr connId, Message msg)
        {
            int targetid = msg.GetContent<int>(0);
            UserModel targetuser = DALMenager.Instance.user.GetUserModel(targetid);
            UserModel attackuser = DALMenager.Instance.user.GetUserModel(connId);
            targetuser.HP -= attackuser.userInfo.Attack;
            //发给所有客户端
            foreach (IntPtr ptr in DALMenager.Instance.user.GetUserPtrs())
            {
                Server.Send(ptr, Message.Type.Type_Game, Message.Type.Game_AttackS, attackuser.ID, targetid, targetuser.HP);
            }

        }
    }
}
