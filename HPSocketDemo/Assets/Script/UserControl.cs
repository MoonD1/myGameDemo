using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UserControl : MonoBehaviour
{
    public Camera camera;
    private static Vector3 cameraPos = new Vector3(-4.281778f, 3.132431f, 1.62461f);
    //角色id
    public int id;
    //动画导航组件
    private Animator animator;
    private NavMeshAgent agent;

    //目标位置
    private Vector3 targetPosition;
    //是否死亡（如果死亡，就不会更新）
    public bool isDie = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(camera != null)
        {
            camera.transform.position = transform.position + cameraPos;
        }
        if (isDie)
        {
            return;
        }
        //离目标位置的距离
        float dis = Vector3.Distance(transform.position, targetPosition);
        if(dis > 0.5f)
        {
            agent.isStopped = false;
            agent.SetDestination(targetPosition);
            
            animator.SetBool("IsRun", true);
        }
        else
        {
            agent.isStopped = true;
            animator.SetBool("IsRun", false);
        }
    }

    //移动方法
    public void Move(Vector3 target)
    {
        this.targetPosition = target;
    }

    public void Move(float[] target)
    {
        this.targetPosition = new Vector3(target[0], target[1], target[2]);
    }

    public void Attack(int targetid)
    {
        UserControl targetuser = UserManager.idUserDic[targetid];
        //攻击者面向被攻击者
        this.transform.LookAt(targetuser.transform.position);
        //播放攻击动画
        animator.SetTrigger("Attack");
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        isDie = true;
    }

}
