using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlycutterControl : MonoBehaviour
{
    private static FlycutterControl instance;
    public static FlycutterControl Instance
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


    // 发射飞刀
    public void getFlyCutter(UserControl user, Vector3 mousePos)
    {
        if (user != null)
        {
            // Debug.Log("test");
            // 生成飞刀
            GameObject flycutter = ObjectPool.Instance.pool.Get();

            // 设置飞刀初始位置
            Vector3 userPos = user.transform.position;
            Vector3 flyCutterPos = new Vector3(userPos.x, userPos.y + 0.8f, userPos.z);
            // Vector3 flyCutterPos = user.transform.position;
            // flyCutterPos.y += 0.8f;
            flycutter.transform.position = flyCutterPos;

            // 计算发射方向
            Vector3 direction = new Vector3(mousePos.x - flyCutterPos.x, 0, mousePos.z - flyCutterPos.z).normalized;
            
            //调整朝向
            flycutter.transform.LookAt(mousePos);
            flycutter.transform.Rotate(0, -90, 0, Space.Self);

            // 发射
            flycutter.GetComponent<Rigidbody>().velocity = direction * 5;

        }
    }
}
