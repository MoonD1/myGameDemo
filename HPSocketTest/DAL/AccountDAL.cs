using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPSocketTest
{
    public class AccountDAL
    {
        //保存账号的列表
        private List<AccountModel> accounts = new List<AccountModel>();
        //保存登录成功的账号的列表 IntPtr连接
        private Dictionary<IntPtr, AccountModel> ptrAccountDict = new Dictionary<IntPtr, AccountModel>();

        private static int id = 0;

        //加载数据库的账号密码到列表中
        public AccountDAL()
        {
            SQLiteDataReader reader = SqliteManager.Instance.Search("select * from ACCOUNT");
            //读取一行数据
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string account = reader.GetString(1);
                string password = reader.GetString(2);
                AccountModel model = new AccountModel(id, account, password);
                accounts.Add(model);
            }
        }


        //注册账号 1：成功   -1：失败
        public int Add(string account, string password)
        {
            foreach (AccountModel model in accounts)
            {
                if(model.Account == account)
                {
                    return -1;
                }
            }
            lock (this)
            {
                // 添加账号到数据库
                int res = SqliteManager.Instance.ExecuteCmd($"insert into ACCOUNT(account, password) values('{account}', '{password}')");
                if (res != 1)
                {
                    return -1;
                }
            }
            //创建新账号
            AccountModel accountModel = new AccountModel(id++, account, password);
            accounts.Add(accountModel);
            return 1;
        }

        //登录 成功则返回用户id，失败返回-1
        public int Login(IntPtr ptr, string account, string password)
        {
            foreach (AccountModel model in ptrAccountDict.Values)
            {
                if (model.Account == account)
                {
                    return -1;
                }
            }
            foreach (AccountModel model in accounts)
            {
                if (model.Account == account && model.Password == password)
                {
                    ptrAccountDict[ptr] = model;
                    return model.ID;
                }
            }
            return -1;
        }

        //账号下线
        public void Logout(IntPtr ptr)
        {
            if (ptrAccountDict.ContainsKey(ptr))
            {
                ptrAccountDict.Remove(ptr);
            }
        }

    }
}
