using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;

//序列化标记，加了标记后才可以转成二进制
[Serializable]
public class Message
{
    //类型
    public byte type;
    //命令
    public int command;
    //参数
    public object content;

    //构造方法
    public Message(byte type, int command, params object[] contents) {
        this.type = type;
        this.command = command;
        this.content = contents;
    }

    //获取第几个参数
    public T GetContent<T>(int index)
    {
        object[] objs = (object[])content;
        return (T)objs[index];
    }

    [Serializable]
    public class Type
    {
        //类型
        public const byte Type_Account = 1;
        public const byte Type_User = 2;
        public const byte Type_Game = 3;
        //命令
        //注册账号 1账号 2密码
        public const int Account_RegistC = 100;
        //1：注册成功 -1：注册失败
        public const int Account_RegistS = 101;
        //登录账号 1账号 2密码
        public const int Account_LoginC = 102;
        //返回ID：登录成功 -1：登录失败
        public const int Account_LoginS = 103;

        //角色
        //选择角色（模型id）（出生位置坐标），客户端告知服务端选择哪个角色
        public const int User_SelectC = 104;
        //选择角色（用户id）（模型id）（位置float[]），服务端告知该客户端在哪里生成什么
        public const int User_SelectS = 105;
        //创建角色，发给其他客户端，告知生成（用户id，模型id，位置float[]）
        public const int User_CreateS = 106;
        //移除角色，下线（用户id）
        public const int User_RemoveS = 107;

        //游戏
        //移动（用户id，位置）
        public const int Game_MoveC = 108;
        public const int Game_MoveS = 109;
        //攻击（目标id）
        public const int Game_AttackC = 110;
        //攻击 （攻击者id） （目标id） （目标剩余血量）
        public const int Game_AttackS = 111;

    }
}
