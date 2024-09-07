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
        //注册消息监听
        Client.Addlistener(this);
    }

    //发起登录请求
    public void Login()
    {
        if (usernameField.text.Length > 3 && passwordField.text.Length > 3)
        {
            Message msg = new Message(Message.Type.Type_Account, Message.Type.Account_LoginC, usernameField.text, passwordField.text);
            Client.Send(msg);
        }
    }

    //发起注册请求
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
        //如果消息类型不是账号就不处理
        if(message.type != Message.Type.Type_Account)
        {
            return;
        }
        //注册
        if(message.command == Message.Type.Account_RegistS)
        {
            int res = message.GetContent<int>(0);
            if(res == 1)
            {
                //Debug.Log("注册成功");
                TipControl.Instance.Show("注册成功");
            }
            else
            {
                //Debug.Log("注册失败");
                TipControl.Instance.Show("注册失败");
            }
        }

        //登录
        if (message.command == Message.Type.Account_LoginS)
        {
            int res = message.GetContent<int>(0);
            if (res != -1)
            {
                //Debug.Log("注册成功");
                gameObject.SetActive(false);
            }
            else
            {
                //Debug.Log("注册失败");
                TipControl.Instance.Show("登录失败");
            }
        }
    }
}
