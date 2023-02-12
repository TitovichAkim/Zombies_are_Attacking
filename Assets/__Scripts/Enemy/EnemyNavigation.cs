using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation:MonoBehaviour
{

    public float        moveSpeed;          // �������� ������������
    public float        rotationSpeed;      // �������� ��������
    bool                playerGoal;         // ������� ���� - �����?

    public Transform    goal;               // ����, ���� ��������
    public NavMeshAgent agent;              // ������ �� ��������� NavMeshAgent

    private void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }
    private void Update ()
    {
        SetPlayerGoal();                    // ���� ������� ���������, ��������� ����� - ������
    }

    public void SetTarget (Transform targetTransform)
    {
        agent.destination = targetTransform.position;
        agent.stoppingDistance = 0.3f;          // ��������� ��������� ���������
    }


    void ConfigureAgent ()
    {
        agent = GetComponent<NavMeshAgent>();   // ��������� ������ �� NavMeshAgent
        agent.updateRotation = false;           // ������ �� ��������� �������� �� ����
        agent.updateUpAxis = false;             // ������ ������� ����� UpAxis
        agent.stoppingDistance = 1.3f;          // ��������� ��������� ���������
    }

    void SetPlayerGoal ()
    {
        if(!agent.hasPath)
        {
            playerGoal = true;
            agent.stoppingDistance = 1.3f;          // ��������� ��������� ���������
        }
        if(playerGoal)
        {
            agent.destination = goal.position;      // ��������� �������� ����� ��������
        }
    }


}
