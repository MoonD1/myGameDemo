using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ObjectPool : MonoBehaviour
{
    public GameObject flycutter;
    public ObjectPool<GameObject> pool;
    private static ObjectPool instance;
    public static ObjectPool Instance
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
        pool = new ObjectPool<GameObject>(
            () =>
            {
                GameObject obj = Instantiate(flycutter, transform);
                obj.GetComponent<Flycutter>().pool = pool;
                return obj;
            },
            obj =>
            {
                // �Ӷ���ػ�ȡ��Ʒ
                obj.SetActive(true);
            },
            obj =>
            {
                // ��������б�����Ʒ
                obj.SetActive(false);
            },
            obj =>
            {
                Destroy(obj);
            },
            true, 100, 1000
           );
    }

}
