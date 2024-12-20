using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UserControl : MonoBehaviour
{
    public Camera camera;
    private static Vector3 cameraPos = new Vector3(-4.281778f, 3.132431f, 1.62461f);
    //��ɫid
    public int id;
    //�����������
    private Animator animator;
    private NavMeshAgent agent;

    //Ŀ��λ��
    private Vector3 targetPosition;
    //�Ƿ�����������������Ͳ�����£�
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
        //��Ŀ��λ�õľ���
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

    //�ƶ�����
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
        //���������򱻹�����
        this.transform.LookAt(targetuser.transform.position);
        //���Ź�������
        animator.SetTrigger("Attack");
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        isDie = true;
    }

}
