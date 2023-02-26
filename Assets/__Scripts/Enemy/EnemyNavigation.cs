using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyNavigation:MonoBehaviour
{    
    public Transform                goal;               // ����, ���� ��������
    public NavMeshAgent             agent;              // ������ �� ��������� NavMeshAgent
    public EnemyScriptableObject    enemyParametersSO;  //������ �� �� ����������

    private Enemy_Scr               _enemyScript;       // ������ �� Enemy_Scr

    private void Awake ()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start ()
    {
        _enemyScript = GetComponent<Enemy_Scr>();
        agent.speed = enemyParametersSO.speedOfMovement;
    }


    void UpdatePlayerPosition ()                        // ���������� � Enemy_Scr � ������ StateRedactor()
    {
        if(_enemyScript.state == enemyState.stalking && agent.isActiveAndEnabled)   // ���� ��������� ��������� � ����� ������� (�� �����)
        {
            agent.SetDestination(goal.position);        // �������� (���������) �������� ����� ��������
        } 
        else                                            // ���� ������ ���������
        {
            CancelInvoke("UpdatePlayerPosition");       // ���������� ��������� �������� ����� ��������
        }
    }
    #region ���-�� �� ������������. ��������, ���� �������
    public void SetTarget (Transform targetTransform)
    {
        agent.destination = targetTransform.position;
        agent.stoppingDistance = 0.3f;          // ��������� ��������� ���������
    }

    void ConfigureAgent ()
    {
        agent.updateRotation = false;           // ������ �� ��������� �������� �� ����
        agent.updateUpAxis = false;             // ������ ������� ����� UpAxis
        agent.stoppingDistance = 1.3f;          // ��������� ��������� ���������
    }
    #endregion
}
