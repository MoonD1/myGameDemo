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
                case Message.Type.Game_FlycutterC:
                    Flycutter(connId, msg);
                    break;
                case Message.Type.Game_HurtByFlycutterC:
                    //Console.Out.WriteLine("hurt");
                    HurtByFlycutter(connId, msg);
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

        void Flycutter(IntPtr connId, Message msg)
        {
            int userid = msg.GetContent<int>(0);
            float[] targetPos = msg.GetContent<float[]>(1);
            //如果这个用户已经死亡了，就直接返回
            UserModel user = DALMenager.Instance.user.GetUserModel(userid);
            if(user.HP <= 0)
            {
                return;
            }


            //通知所有客户端，这个用户要向目标位置使用飞刀
            foreach (IntPtr ptr in DALMenager.Instance.user.GetUserPtrs())
            {
                Server.Send(ptr, Message.Type.Type_Game, Message.Type.Game_FlycutterS, userid, targetPos);
            }
        }

        void HurtByFlycutter(IntPtr connId, Message msg)
        {
            int targetid = msg.GetContent<int>(0);
            UserModel targetuser = DALMenager.Instance.user.GetUserModel(targetid);

            // 对于已死亡用户，不继续进行通讯和计算，减少负担
            if(targetuser.HP <= 0)
            {
                return;
            }

            targetuser.HP -= 20;
            //Console.WriteLine(targetuser.HP);
            //通知所有客户端，这个用户受伤了
            foreach (IntPtr ptr in DALMenager.Instance.user.GetUserPtrs())
            {
                Server.Send(ptr, Message.Type.Type_Game, Message.Type.Game_HurtByFlycutterS, targetid, targetuser.HP);
            }
        }
    }
}
