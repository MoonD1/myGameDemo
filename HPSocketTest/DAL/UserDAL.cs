using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPSocketTest
{
    public class UserDAL
    {
        //两种字典保存客户端所有的用户模型 —— 既可以通过连接IntPtr获取模型，也可以通过用户ID int获取
        private Dictionary<IntPtr, UserModel> ptrUserDic = new Dictionary<IntPtr, UserModel>();
        private Dictionary<int, UserModel> idUserDic = new Dictionary<int, UserModel>();
        //id
        private static int userid = 1;

        //添加一名角色
        public int AddUser(IntPtr ptr, int index)
        {
            UserModel userModel = new UserModel();
            userModel.ID = userid++;
            userModel.userInfo = UserInfo.userInfos[index - 1];
            userModel.HP = userModel.userInfo.MaxHP;
            ptrUserDic.Add(ptr, userModel);
            idUserDic.Add(userModel.ID, userModel);
            return userModel.ID;
        }

        //移除
        public void RemoveUser(IntPtr ptr)
        {
            idUserDic.Remove(ptrUserDic[ptr].ID);
            ptrUserDic.Remove(ptr);
        }

        //获取所有连接 —— ptrUserDic的key
        public IntPtr[] GetUserPtrs()
        {
            return ptrUserDic.Keys.ToArray();
        }

        //获取所有模型 —— 获取value
        public UserModel[] GetUserModels() 
        {
            return ptrUserDic.Values.ToArray();
        }

        //获取对应id的模型
        public UserModel GetUserModel(int id)
        {
            return idUserDic[id];
        }

        //获取对应连接的模型
        public UserModel GetUserModel(IntPtr ptr)
        {
            return ptrUserDic[ptr];
        }
    }
}
