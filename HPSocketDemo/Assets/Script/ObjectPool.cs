using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ObjectPool : MonoBehaviour
{
    public GameObject flycutter;
    private ObjectPool<GameObject> pool;
    // Start is called before the first frame update
    void Awake()
    {
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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            pool.Get();
        }
    }
}
