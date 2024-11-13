using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MessageTool
{
    //对象转二进制
    public static byte[] ToByte(object obj)
    {
        MemoryStream ms = new MemoryStream();
        new BinaryFormatter().Serialize(ms, obj);
        byte[] data = new byte[ms.Length];
        //第一个0代表拷贝的其实位置，第二个0代表偏移量，ms.Length初始为long类型，所以要转int
        Buffer.BlockCopy(ms.GetBuffer(), 0, data, 0, (int)ms.Length);
        ms.Close();
        return data;
    }
    //二进制转对象
    public static Message ToObj(byte[] bytes)
    {
        MemoryStream ms = new MemoryStream(bytes);
        object obj = new BinaryFormatter().Deserialize(ms);
        ms.Close();
        return obj as Message;
    }
}

