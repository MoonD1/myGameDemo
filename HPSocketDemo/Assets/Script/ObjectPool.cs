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
                // 从对象池获取物品
                obj.SetActive(true);
            },
            obj =>
            {
                // 往对象池中保存物品
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
