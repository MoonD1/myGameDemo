using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlycutterManager : MonoBehaviour, IMessage
{
    public void Receive(Message message)
    {
        if (message.type != Message.Type.Type_Game)
        {
            return;
        }
        switch (message.command)
        {
            case Message.Type.Game_FlycutterS:
                ThrowFlycutter(message);
                break;
        }
    }

    private void ThrowFlycutter(Message message)
    {
        int id = message.GetContent<int>(0);
        float[] targetPos = message.GetContent<float[]>(1);
        //�����ӷɵ��ĺ���
        UserControl user = UserManager.idUserDic[id];
        Vector3 mousePos = new Vector3(targetPos[0], targetPos[1], targetPos[2]);
        getFlyCutter(user, mousePos);
    }

    private static FlycutterManager instance;
    public static FlycutterManager Instance
    {
        get
        {
            return instance;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Client.Addlistener(this);
    }


    // ����ɵ�
    public void getFlyCutter(UserControl user, Vector3 mousePos)
    {
        if (user != null)
        {
            // Debug.Log("test");

            // �޸�״̬Ϊ�ӷɵ���״̬/������û������

            // ���ɷɵ�
            GameObject flycutter = ObjectPool.Instance.pool.Get();
            Flycutter flycutterComponent = flycutter.GetComponent<Flycutter>();
            flycutterComponent.ChangeUser(user);

            // ���÷ɵ���ʼλ��
            Vector3 userPos = user.transform.position;
            Vector3 flyCutterPos = new Vector3(userPos.x, userPos.y + 0.8f, userPos.z);
            // Vector3 flyCutterPos = user.transform.position;
            // flyCutterPos.y += 0.8f;
            flycutter.transform.position = flyCutterPos;

            // ���㷢�䷽��
            Vector3 direction = new Vector3(mousePos.x - flyCutterPos.x, 0, mousePos.z - flyCutterPos.z).normalized;

            //��������
            flycutter.transform.LookAt(mousePos);
            flycutter.transform.Rotate(0, -90, 0, Space.Self);

            // ����
            flycutter.GetComponent<Rigidbody>().velocity = direction * 5;

        }
    }
}
