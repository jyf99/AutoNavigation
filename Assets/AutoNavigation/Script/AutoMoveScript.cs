using UnityEngine;
using UnityEngine.AI;

public class AutoMoveScript : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // �����Զ��ƶ��������֮���
        // �����ƶ������������ڽӽ�Ŀ���ʱ
        // ������٣���
        agent.autoBraking = false;

        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // ���δ�����κε㣬�򷵻�
        if (points.Length == 0)
            return;

        //����������Ϊǰ����ǰѡ����Ŀ�ꡣ
        agent.destination = points[destPoint].position;

        //ѡ�������е���һ������ΪĿ�꣬
        // ���б�Ҫ��ѭ������ʼ��
        destPoint = (destPoint + 1) % points.Length;
    }


    // Update is called once per frame
    void Update()
    {
        //������ӽ���ǰĿ���ʱ��
        // ѡ����һ��Ŀ��㡣
        if (!agent.pathPending && agent.remainingDistance < 2f)
            GotoNextPoint();
    }
}
