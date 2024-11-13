using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Flycutter : MonoBehaviour
{
    public ObjectPool<GameObject> pool;
    // Start is called before the first frame update
    private void OnEnable()
    {
        transform.position = new Vector3(-0.7525501f, -7.453267f, 41.16394f);
        Invoke("DestroySelf", 1);
    }
    private void DestroySelf()
    {
        // Debug.Log("Destory");
        pool.Release(gameObject);
    }
}
