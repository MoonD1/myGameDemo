using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPSocketTest
{
    public class SqliteManager
    {
        private static SqliteManager instance;
        public static SqliteManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SqliteManager();
                }
                return instance;
            }
        }

        //数据库连接类
        SQLiteConnection conn;
        //数据库命令类
        SQLiteCommand cmd;

        SqliteManager()
        {
            //数据库连接字符串 —— 放数据库目录
            string conStr = "Data Source=" + Directory.GetCurrentDirectory() + "\\test.db";
            //创建连接类
            conn = new SQLiteConnection(conStr);
            conn.Open();
            //创建表
            ExecuteCmd("create table if not exists ACCOUNT(id integer primary key autoincrement, account text, password text)");

        }
        ~SqliteManager()
        {
            //释放数据库连接
            conn.Dispose();
            //关闭数据库
            conn.Close();
        }
        //执行命令，除了查以外的命令（原因：其他命令只需要返回成功与否，查询需要返回查询内容）
        public int ExecuteCmd(string command)
        {
            cmd = new SQLiteCommand(command, conn);
            //这里的返回值为“影响常数”，代表有多少条数据受到了影响/添加了多少条数据/删除了多少条数据
            return cmd.ExecuteNonQuery();
        }
        //查询单个
        public object SearchOne(string command)
        {
            cmd = new SQLiteCommand(command, conn);
            //返回object类型
            return cmd.ExecuteScalar();
        }
        //查询多行
        public SQLiteDataReader Search(string command)
        {
            cmd = new SQLiteCommand(command, conn);
            return cmd.ExecuteReader();
        }
    }
}
