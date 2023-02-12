using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement_Scr:MonoBehaviour
{

    [Header("Изменяются в зависимости от типа врага")]
    public float                    moveSpeed;          // Скорость движения
    public float                    rotationSpeed;      // Скорость поворота

    [Header("Изменяются динамически")]

    public Rigidbody2D              rigid2D;            // Ссылка на rigidbody2D
    public Vector2                  moveDirection;      // Направление движения
    public GameObject               noseGO;             // Ссылка на нос противника
    public GameObject               playerGO;           // Ссылка на игрока

    // Информация для луча
    public GameObject[] lasers = new GameObject[3];         // Массив, хранящий точки, необходимые для направления луча
    public Vector3 barrier;                                 // Координата точки возможного столкновения
    public int povorot;                                     // Значение, куда поворачивать
    public int barrierDirection;                            // Будет хранить направление, в котором была обнаружена преграда
    public bool moveMode = false;                           // Особый режим для движения корабля при нахождении менее чем в одном пункте от противника
    public float moveModeTime;                              // Время, которе будет держаться особый режим движения
    public float moveModeFixTime;							// Момент, в который был включен особый режим движения
    public GameObject barrierGO;                            // Барьер, от которого уворачивается объект


    public void Awake ()
    {
        noseGO = new GameObject("Nose");                            // Создать объект "Нос"
        noseGO.transform.parent = this.transform;                   // Сделать его дочерним данному объекту
        noseGO.transform.localPosition = new Vector2(0, 0.1f);      // Разместить его относительно родителя
        noseGO.layer = 7;

        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();      // Заполнить ссылку на rigidbody2D
    }

    public void Start ()
    {
        rotationSpeed = 10;         // Скорость поворота - 10
        CREATE_LASERS();            // Создать точки для лазеров
    }

    public void FixedUpdate ()
    {
        CREATE_RAY();                                                                                   // Отправить лучи
        if(Vector3.Distance(playerGO.transform.position, this.gameObject.transform.position) >= 1f)
        {  // Едистанция до игрока больше едиицы
            MOVE();                                                                                     // Идти
        }
        ROTATION();                                                                                     // Повернуть
    }

    public void ROTATION ()
    {
        float rotZk;
        Vector3 difference = playerGO.transform.position - this.transform.position;                 // Рассчитать направление, где находится противник
        if(difference.magnitude >= 1 && barrierGO == null)
        {                                        // Если дистанция до противника больше одного пункта и нет барьеров
            rotZk = 90;                                                                             // Двигаться прямо на него
        }
        else
        {                                                                                // Если меньше одного пункта или есть барьер
            rotZk = Random.Range(0, 1) * 180;                                                       // Повернуться к нему одним или другим боком 
        }
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;                       // Рассчитать тангенс угла поворота (наверное, уже не помню)
        Quaternion rotation = Quaternion.AngleAxis(rotZ - rotZk, Vector3.forward);                  // Назначить значение переменной поворота

        if(barrier == Vector3.zero)
        {
            rigid2D.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * 0.01f); // Повернуть объект в сторону движения
        }
        else
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime * povorot * 10);
        }
        barrierGO = null;
    }
    public void MOVE ()
    {
        rigid2D.velocity = (noseGO.transform.position - this.gameObject.transform.position).normalized * moveSpeed; // Двигаться вперед
    }

    public void CREATE_LASERS ()
    {        // Создать объекты для направления лучей, определяющих возможные столкновения	
        GameObject leftLaser = new GameObject("Left Laser");                // Создать объект "Левый лазер"
        leftLaser.transform.SetParent(transform);                           // Присвоить родителя - этот объект
        leftLaser.transform.localPosition = new Vector3(-0.07f, 0.07f, 0);  // Настроить положение 45 градусов влево
        leftLaser.layer = 7;
        lasers[0] = leftLaser;                                              // Записать в массив лазеров для удобства обращения

        GameObject rightLaser = new GameObject("Right Laser");              // Создать объект "Правый лазер"
        rightLaser.transform.SetParent(transform);                          // Присвоить родителя - этот объект
        rightLaser.transform.localPosition = new Vector3(0.07f, 0.07f, 0);  // Настроить положение 45 градусов вправо
        rightLaser.layer = 7;
        lasers[2] = rightLaser;                                             // Записать в массив лазеров для удобства обращения

        lasers[1] = noseGO;                                                 // Записать объект nose как центральный лазер
    }

    public void CREATE_RAY ()
    {           // Выпустить луч и проанализировать, нет ли объектов впереди
        RaycastHit2D[] hits;                                    // Хранит массив объектов, которые встретил луч
        bool barrierParam = false;                              // Переменная, которая покажет, встречал ли луч объекты типа барьер на всем своем пути
        if(barrier == Vector3.zero)
        {                               // Если значение барьера на данный момент равно нулю, включить все лазеры для определения барьеров


            Vector3[] barriersLR = new Vector3[3];                  // Этот массив поможет определить направление поворота, если преграда впереди

            for(int i = -1; i <= 1; i++)
            {                          // Перечисление, чтобы по очереди проверить лазеры слева направо

                hits = Physics2D.RaycastAll(transform.position, (lasers[i + 1].transform.position - transform.position), 2.5f);           // Выпустить луч и записать все коллайдеры (столкновения)

                foreach(RaycastHit2D barrierHit in hits)
                {
                    // Если
                    if(barrierHit.collider.gameObject.layer != 6 & barrierHit.collider.gameObject.layer != 7 & barrierHit.collider.gameObject.layer != 8 & barrierHit.collider.gameObject.layer != 13 & barrierHit.collider.gameObject.name != "Visibility_Area")// Лэйер объекта не 6, 7, 8                    && (barrier == null || barrier == Vector3.zero      // и barrier пустой
                                                                                                                                                                                                                                                                 //             || (transform.position - barrier).magnitude > (new Vector2(transform.position.x, transform.position.y) - barrierHit.point).magnitude) // или новый встреченный объект ближе чем предыдущий
                    {
                        barrier = barrierHit.point;                     // Назначить точку возможного столкновения
                        barriersLR[i + 1] = barrier;                    // Заполнить на случай, если сработает луч впереди
                        barrierDirection = i;                           // Определить, в какой стороне был обнаружен барьер
                        barrierParam = true;                            // Параметр барьера - присутствует
                        povorot = i;                                    // Назначить сторону, в которую поворачивать
                        barrierGO = barrierHit.collider.gameObject;


                    }
                }
            }

            if(barrierDirection == 0 & barrier != Vector3.zero)
            {                                   // Если барьер обнаружен впереди, сравнить левый 
                if(barriersLR[0].magnitude <= barriersLR[2].magnitude)
                {                            // и правый луч. Где больше расстояние до преграды
                                             // или ее нет, туда и повернуть
                    povorot = 1;
                }
                else
                {
                    povorot = -1;
                }
            }

        }
        else
        {                                        // Если барьер существовал ранее
                                                 // Выпустить луч и записать все коллайдеры в направлении, где был обнаружен барьер впервые
            hits = Physics2D.RaycastAll(transform.position, (lasers[barrierDirection + 1].transform.position - transform.position), 1f);
            foreach(RaycastHit2D barrierHit in hits)
            {
                if(barrierHit.collider.gameObject.layer != 6 & barrierHit.collider.gameObject.layer != 7 & barrierHit.collider.gameObject.layer != 8 & barrierHit.collider.gameObject.layer != 13 & barrierHit.collider.gameObject.name != "Visibility_Area")
                {                        // Если на пути этого лазера все-таки встречается барьер
                    barrierParam = true;
                    barrierGO = barrierHit.collider.gameObject;
                }
            }

            if(barrierParam != true)
            {
                barrier = Vector3.zero;
                povorot = 0;
            }
        }
    }
}
