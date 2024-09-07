using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectControl : MonoBehaviour
{
    public Transform[] startPoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SelectHero(int modelid)
    {
        //������������Ϣ��Ҫ������ɫ
        Vector3 pos = startPoints[modelid - 1].position;
        Message message = new Message(Message.Type.Type_User, Message.Type.User_SelectC, modelid,new float[] {pos.x, pos.y, pos.z });
        Client.Send(message);
        Destroy(gameObject);
    }
}
