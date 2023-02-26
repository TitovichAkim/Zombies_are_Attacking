using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Scr:MonoBehaviour
{

    private Rigidbody2D rigid2d;
    public Vector2      goalPos;                    // Координата объекта, в сторону которого отправили пулю
    public float        bulletSpeed;                // Скорость полета пули 
    public int          damage;                     // Наносимый урон

    public GameObject   playerGO;                   // Ссылка на объект игрока
    public Vector2      playerPos;                  // Позиция игрока в момент выстрела

    public float        createMoment;               // Момент создания пули

    public void Awake ()
    {
        rigid2d = this.GetComponent<Rigidbody2D>(); // Заполнение ссылки на Жесткое тело
        createMoment = Time.time;

    }

    void FixedUpdate ()
    {
        rigid2d.velocity = (goalPos - playerPos).normalized * bulletSpeed;  // Осуществляет движение пули

        if(Time.time - createMoment > 5)
        {          // Если разница между временем создания и текущим моментом больше 5 секунд
            Destroy(this.gameObject);               // Самоуничтожиться
        }

    }

    public void OnTriggerEnter2D (Collider2D collision)         // Если прикоснулся к чему-то
    {                                            
        if(collision.TryGetComponent(out IDamageabel damagabel) && collision.gameObject.layer != 6 && collision.gameObject.name != "Visibility_Area")    // Если это не игрок и не его зона обзора
        {
            damagabel.ApplyDamage(damage);      // Выполнить метод нанесения урона 
            Destroy(this.gameObject);           // Самоуничтожиться
        }
    }
}
