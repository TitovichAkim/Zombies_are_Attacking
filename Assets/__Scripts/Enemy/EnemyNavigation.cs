using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation:MonoBehaviour
{

    public float        moveSpeed;          // Скорость передвижения
    public float        rotationSpeed;      // Скорость поворота
    bool                playerGoal;         // Текущая цель - игрок?

    public Transform    goal;               // Цель, куда движется
    public NavMeshAgent agent;              // Ссылка на компонент NavMeshAgent

    private void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }
    private void Update ()
    {
        SetPlayerGoal();                    // Если условия совпадают, назначает целью - игрока
    }

    public void SetTarget (Transform targetTransform)
    {
        agent.destination = targetTransform.position;
        agent.stoppingDistance = 0.3f;          // Настроить дистанцию остановки
    }


    void ConfigureAgent ()
    {
        agent = GetComponent<NavMeshAgent>();   // Заполнить ссылку на NavMeshAgent
        agent.updateRotation = false;           // Запрет на изменение поворота по осям
        agent.updateUpAxis = false;             // Запрет движени через UpAxis
        agent.stoppingDistance = 1.3f;          // Настроить дистанцию остановки
    }

    void SetPlayerGoal ()
    {
        if(!agent.hasPath)
        {
            playerGoal = true;
            agent.stoppingDistance = 1.3f;          // Настроить дистанцию остановки
        }
        if(playerGoal)
        {
            agent.destination = goal.position;      // Назначить конечную точку движения
        }
    }


}
