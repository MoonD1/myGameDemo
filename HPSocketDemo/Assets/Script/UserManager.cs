using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour, IMessage
{
    public Camera camera;
    //保存所有玩家角色
    public static Dictionary<int, UserControl> idUserDic = new Dictionary<int, UserControl>();
    //保存当前客户端角色id
    public static int ID;
    //获取当前角色
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
                    //渲染其他人
                    createOtherUser(message);
                    break;
                case Message.Type.User_SelectS:
                    //Debug.Log("test3");
                    //创建自己的角色
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
        //从场景中删除
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
            //创建角色
            GameObject userPre = Resources.Load<GameObject>(modelid.ToString());
            //实例化
            GameObject userI = Instantiate(userPre, new Vector3(points[0], points[1], points[2]), Quaternion.identity);
            //修改标签
            //userI.tag = "Myself";
            //获取UserControl
            UserControl userControl = userI.GetComponent<UserControl>();
            userControl.id = userid;
            ID = userid;
            //将自己的角色添加到角色字典中
            idUserDic[userid] = userControl;

            userControl.camera = camera;
        }
        else
        {
            Debug.Log("创建角色失败");
        }
    }

    void createOtherUser(Message message)
    {
        //Debug.Log("test");
        int userid = message.GetContent<int>(0);
        int modelid = message.GetContent<int>(1);
        float[] points = message.GetContent<float[]>(2);
        //创建角色
        GameObject userPre = Resources.Load<GameObject>(modelid.ToString());
        //实例化
        GameObject userI = Instantiate(userPre, new Vector3(points[0], points[1], points[2]), Quaternion.identity);
        UserControl userControl = userI.GetComponent<UserControl>();
        userControl.id = userid;
        //将自己的角色添加到角色字典中
        idUserDic[userid] = userControl;
    }

    // Start is called before the first frame update
    void Start()
    {
        Client.Addlistener(this);
    }


}
