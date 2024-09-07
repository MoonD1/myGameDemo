using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HPSocket;
using HPSocket.Tcp;
using System;
using System.Text;

public interface IMessage
{
    void Receive(Message message);

}
public class Client : MonoBehaviour
{
    //单例
    public static Client Instance;
    //客户端连接对象
    TcpClient tcpClient = new TcpClient();
    //保存所有监听消息的对象
    public List<IMessage> messageListener = new List<IMessage>();
    //消息队列
    Queue<Message> messageQueue = new Queue<Message>();

    void Awake()
    {
        Instance = this;
        //场景切换时让当前物体不被销毁
        DontDestroyOnLoad(gameObject);
        //设置服务器的ip和端口
        tcpClient.Address = "127.0.0.1";
        tcpClient.Port = Convert.ToUInt16(5567);
        //接收服务端发来的消息
        tcpClient.OnReceive += TcpClient_OnReceive;
        tcpClient.Connect();

    }

    //添加监听
    public static void Addlistener(IMessage messageLis)
    {
        Instance.messageListener.Add(messageLis);
    }

    //移除监听
    public void Removelistener(IMessage messageLis)
    {
        Instance.messageListener.Remove(messageLis);
    }

    //发送消息
    public static void Send(Message msg)
    {
        byte[] data = MessageTool.ToByte(msg);
        Instance.tcpClient.Send(data, data.Length);
    }
    private HandleResult TcpClient_OnReceive(IClient sender, byte[] data)
    {
        /*  test
        //测试如果接收到服务端消息
        string msg = Encoding.UTF8.GetString(data);
        Debug.Log(msg);
        */
        //二进制转消息
        Message message = MessageTool.ToObj(data) as Message;
        messageQueue.Enqueue(message);
        return HandleResult.Ok;
    }

    // Update is called once per frame
    void Update()
    {
        /*  test
        //测试客户端发送消息
        if (Input.GetMouseButtonDown(0))
        {
            byte[] data = Encoding.UTF8.GetBytes("你好，我是客户端");
            tcpClient.Send(data, data.Length);
        }
        */
        //从队列取出消息
        if (messageQueue.Count > 0)
        {
            Message msg = messageQueue.Dequeue();
            Debug.Log(msg.command);
            //消息传递，把消息传递给所有监听者
            foreach(IMessage message in messageListener)
            {
                message.Receive(msg);
            }
        }
    }
}
