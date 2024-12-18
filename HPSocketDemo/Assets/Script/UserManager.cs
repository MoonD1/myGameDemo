using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour, IMessage
{
    public Camera camera;
    //����������ҽ�ɫ
    public static Dictionary<int, UserControl> idUserDic = new Dictionary<int, UserControl>();
    //���浱ǰ�ͻ��˽�ɫid
    public static int ID;
    //��ȡ��ǰ��ɫ
    private static UserControl user;
    public static UserControl User
    {
        get 
        {
            if (user == null)
            {
                user = idUserDic[ID];
            }
            return user;
        }
    }

    public void Receive(Message message)
    {
        //Debug.Log("message");
        if (message.type == Message.Type.Type_User)
        {
            //Debug.Log("test4");
            switch (message.command) 
            {
                case Message.Type.User_CreateS:
                    //Debug.Log("test5");
                    //��Ⱦ������
                    createOtherUser(message);
                    break;
                case Message.Type.User_SelectS:
                    //Debug.Log("test3");
                    //�����Լ��Ľ�ɫ
                    selectMyUser(message);
                    break;
                case Message.Type.User_RemoveS:
                    //Debug.Log("test5");
                    removeOtherUser(message);
                    break;
            }
        }
    }
    void removeOtherUser(Message message) 
    {
        int userid = message.GetContent<int>(0);
        UserControl user = idUserDic[userid];
        idUserDic.Remove(userid);
        //�ӳ�����ɾ��
        Destroy(user.gameObject);
    }

    void selectMyUser(Message message)
    {
        //Debug.Log("test2");
        int userid = message.GetContent<int>(0);
        int modelid = message.GetContent<int>(1);
        float[] points = message.GetContent<float[]>(2);
        //Debug.Log(userid);
        //Debug.Log(modelid);
        if (userid > 0)
        {
            //������ɫ
            GameObject userPre = Resources.Load<GameObject>(modelid.ToString());
            //ʵ����
            GameObject userI = Instantiate(userPre, new Vector3(points[0], points[1], points[2]), Quaternion.identity);
            //�޸ı�ǩ
            //userI.tag = "Myself";
            //��ȡUserControl
            UserControl userControl = userI.GetComponent<UserControl>();
            userControl.id = userid;
            ID = userid;
            //���Լ��Ľ�ɫ��ӵ���ɫ�ֵ���
            idUserDic[userid] = userControl;

            userControl.camera = camera;
        }
        else
        {
            Debug.Log("������ɫʧ��");
        }
    }

    void createOtherUser(Message message)
    {
        //Debug.Log("test");
        int userid = message.GetContent<int>(0);
        int modelid = message.GetContent<int>(1);
        float[] points = message.GetContent<float[]>(2);
        //������ɫ
        GameObject userPre = Resources.Load<GameObject>(modelid.ToString());
        //ʵ����
        GameObject userI = Instantiate(userPre, new Vector3(points[0], points[1], points[2]), Quaternion.identity);
        UserControl userControl = userI.GetComponent<UserControl>();
        userControl.id = userid;
        //���Լ��Ľ�ɫ��ӵ���ɫ�ֵ���
        idUserDic[userid] = userControl;
    }

    // Start is called before the first frame update
    void Start()
    {
        Client.Addlistener(this);
    }


}
