using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour,IMessage
{
    public void Receive(Message message)
    {
        if (message.type != Message.Type.Type_Game)
        {
            return;
        }
        switch (message.command)
        {
            case Message.Type.Game_MoveS:
                Move(message);
                break;
            case Message.Type.Game_AttackS:
                Attack(message);
                break;
        }
    }

    void Move(Message message)
    {
        int userid = message.GetContent<int>(0);
        float[] targetPos = message.GetContent<float[]>(1);
        //�õ���Ӧ�û�
        Debug.Log(userid);
        UserControl user = UserManager.idUserDic[userid];
        user.Move(targetPos);
    }

    void Attack(Message message) 
    {
        int attackid = message.GetContent<int>(0);
        int targetid = message.GetContent<int>(1);
        int targethp = message.GetContent<int>(2);
        UserManager.idUserDic[attackid].Attack(targetid);
        if(targethp <= 0)
        {
            UserManager.idUserDic[targetid].Die();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Client.Addlistener(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //����㵽����
                if (hit.collider.tag == "Ground")
                {
                    //����Ϣ Ҫ�ƶ�
                    Vector3 point = hit.point;
                    Message msg = new Message(Message.Type.Type_Game, Message.Type.Game_MoveC, UserManager.ID, new float[] { point.x, point.y, point.z });
                    Client.Send(msg);
                }
                //����㵽����
                if(hit.collider.tag == "User")
                {
                    //����������3m�ھ͹���
                    float dis = Vector3.Distance(hit.point, UserManager.User.transform.position);
                    if(dis > 0.2f && dis < 3f)
                    {
                        UserControl targetUser = hit.collider.GetComponent<UserControl>();
                        //������������Ϣ
                        Client.Send(new Message(Message.Type.Type_Game, Message.Type.Game_AttackC, targetUser.id));
                    }
                }
            }
        }
    }
}
