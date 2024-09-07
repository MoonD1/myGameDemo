using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginControl : MonoBehaviour, IMessage
{
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;

    void Start()
    {
        //ע����Ϣ����
        Client.Addlistener(this);
    }

    //�����¼����
    public void Login()
    {
        if (usernameField.text.Length > 3 && passwordField.text.Length > 3)
        {
            Message msg = new Message(Message.Type.Type_Account, Message.Type.Account_LoginC, usernameField.text, passwordField.text);
            Client.Send(msg);
        }
    }

    //����ע������
    public void Reg()
    {
        if(usernameField.text.Length > 3 && passwordField.text.Length > 3)
        {
            Message msg = new Message(Message.Type.Type_Account, Message.Type.Account_RegistC, usernameField.text, passwordField.text);
            Client.Send(msg);
        }
    }

    public void Receive(Message message)
    {
        //�����Ϣ���Ͳ����˺žͲ�����
        if(message.type != Message.Type.Type_Account)
        {
            return;
        }
        //ע��
        if(message.command == Message.Type.Account_RegistS)
        {
            int res = message.GetContent<int>(0);
            if(res == 1)
            {
                //Debug.Log("ע��ɹ�");
                TipControl.Instance.Show("ע��ɹ�");
            }
            else
            {
                //Debug.Log("ע��ʧ��");
                TipControl.Instance.Show("ע��ʧ��");
            }
        }

        //��¼
        if (message.command == Message.Type.Account_LoginS)
        {
            int res = message.GetContent<int>(0);
            if (res != -1)
            {
                //Debug.Log("ע��ɹ�");
                gameObject.SetActive(false);
            }
            else
            {
                //Debug.Log("ע��ʧ��");
                TipControl.Instance.Show("��¼ʧ��");
            }
        }
    }
}
