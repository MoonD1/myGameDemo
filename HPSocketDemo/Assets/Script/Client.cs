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
    //����
    public static Client Instance;
    //�ͻ������Ӷ���
    TcpClient tcpClient = new TcpClient();
    //�������м�����Ϣ�Ķ���
    public List<IMessage> messageListener = new List<IMessage>();
    //��Ϣ����
    Queue<Message> messageQueue = new Queue<Message>();

    void Awake()
    {
        Instance = this;
        //�����л�ʱ�õ�ǰ���岻������
        DontDestroyOnLoad(gameObject);
        //���÷�������ip�Ͷ˿�
        tcpClient.Address = "127.0.0.1";
        tcpClient.Port = Convert.ToUInt16(5567);
        //���շ���˷�������Ϣ
        tcpClient.OnReceive += TcpClient_OnReceive;
        tcpClient.Connect();

    }

    //��Ӽ���
    public static void Addlistener(IMessage messageLis)
    {
        Instance.messageListener.Add(messageLis);
    }

    //�Ƴ�����
    public void Removelistener(IMessage messageLis)
    {
        Instance.messageListener.Remove(messageLis);
    }

    //������Ϣ
    public static void Send(Message msg)
    {
        byte[] data = MessageTool.ToByte(msg);
        Instance.tcpClient.Send(data, data.Length);
    }
    private HandleResult TcpClient_OnReceive(IClient sender, byte[] data)
    {
        /*  test
        //����������յ��������Ϣ
        string msg = Encoding.UTF8.GetString(data);
        Debug.Log(msg);
        */
        //������ת��Ϣ
        Message message = MessageTool.ToObj(data) as Message;
        messageQueue.Enqueue(message);
        return HandleResult.Ok;
    }

    // Update is called once per frame
    void Update()
    {
        /*  test
        //���Կͻ��˷�����Ϣ
        if (Input.GetMouseButtonDown(0))
        {
            byte[] data = Encoding.UTF8.GetBytes("��ã����ǿͻ���");
            tcpClient.Send(data, data.Length);
        }
        */
        //�Ӷ���ȡ����Ϣ
        if (messageQueue.Count > 0)
        {
            Message msg = messageQueue.Dequeue();
            Debug.Log(msg.command);
            //��Ϣ���ݣ�����Ϣ���ݸ����м�����
            foreach(IMessage message in messageListener)
            {
                message.Receive(msg);
            }
        }
    }
}
