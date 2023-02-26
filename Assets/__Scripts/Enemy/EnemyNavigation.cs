using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyNavigation:MonoBehaviour
{    
    public Transform                goal;               // Цель, куда движется
    public NavMeshAgent             agent;              // Ссылка на компонент NavMeshAgent
    public EnemyScriptableObject    enemyParametersSO;  //Ссылка на СО противника

    private Enemy_Scr               _enemyScript;       // Ссылка на Enemy_Scr

    private void Awake ()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start ()
    {
        _enemyScript = GetComponent<Enemy_Scr>();
        agent.speed = enemyParametersSO.speedOfMovement;
    }


    void UpdatePlayerPosition ()                        // Вызывается в Enemy_Scr в методе StateRedactor()
    {
        if(_enemyScript.state == enemyState.stalking && agent.isActiveAndEnabled)   // Если состояние сталкинга и агент активен (не мертв)
        {
            agent.SetDestination(goal.position);        // Обновить (назначить) конечную точку движения
        } 
        else                                            // Если другое состояние
        {
            CancelInvoke("UpdatePlayerPosition");       // Прекратить обновлять конечную точку движения
        }
    }
    #region Что-то не используемое. Возможно, надо удалить
    public void SetTarget (Transform targetTransform)
    {
        agent.destination = targetTransform.position;
        agent.stoppingDistance = 0.3f;          // Настроить дистанцию остановки
    }

    void ConfigureAgent ()
    {
        agent.updateRotation = false;           // Запрет на изменение поворота по осям
        agent.updateUpAxis = false;             // Запрет движени через UpAxis
        agent.stoppingDistance = 1.3f;          // Настроить дистанцию остановки
    }
    #endregion
}
