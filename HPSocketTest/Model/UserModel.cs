using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPSocketTest
{
    //角色信息类 —— 不同职业的属性
    public class UserInfo
    {
        public int ModelID;
        public int MaxHP;
        public int Attack;
        public int StartPoint;

        public UserInfo(int modelID, int maxHP, int attack, int startPoint)
        {
            ModelID = modelID;
            MaxHP = maxHP;
            Attack = attack;
            StartPoint = startPoint;
        }

        public static UserInfo[] userInfos = new UserInfo[] {
            new UserInfo(1, 100, 25, 1),
            new UserInfo(2, 70, 50, 2),
        };
    }
    public class UserModel
    {
        public int ID;
        public int HP;
        public float[] Points;
        public UserInfo userInfo;

    }
}
