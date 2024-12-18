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
        //transform.position = new Vector3(-0.7525501f, -7.453267f, 41.16394f);
        //transform.position = new Vector3(0, 0, 0);
        Invoke("DestroySelf", 1);
    }
    private void DestroySelf()
    {
        // Debug.Log("Destory");
        pool.Release(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Flycutter");
        if (other != null && other.GetComponent<UserControl>().id != UserManager.ID)
        {
            Debug.Log("hurt it!");
            
            //发送被打中者id到服务端
            int id = other.GetComponent<UserControl>().id;
            Client.Send(new Message(Message.Type.Type_Game, Message.Type.Game_HurtByFlycutterC, id));
        }
        
    }

}
