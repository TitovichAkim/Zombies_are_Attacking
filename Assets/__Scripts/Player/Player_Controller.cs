using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    
public class Player_Controller : MonoBehaviour {

    public GameObject                       goal;               // Цель, на которую нацелен игрок
    public GameObject                       Foots;              // Дочерний объект с ногами

    public Rigidbody2D                      footsRigid;         // Риджитбоди ног

    [SerializeField] private Rigidbody2D    _rigidbody;         // Риджитбоди игрока
    [SerializeField] private FixedJoystick  _joystick;          // Ссылка на джойстик

    [SerializeField] private float _moveSpeed = 1.5f;                  // Скорость движения игрока

    public void Awake() {
        footsRigid = Foots.gameObject.GetComponent<Rigidbody2D>(); // Заполнить ссылку на риджитбоди ног
        }

    private void FixedUpdate(){
        // Движение персонажа по джойстику
        _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _joystick.Vertical * _moveSpeed);
        Foots.transform.position = this.gameObject.transform.position;

        // Измененение направления префаба в нужную сторону
        if (_joystick.Horizontal != 0 && _joystick.Vertical !=0) {
            _rigidbody.rotation = -Mathf.Atan2(_joystick.Horizontal, _joystick.Vertical) * Mathf.Rad2Deg;       // Изменить направление туловища
            footsRigid.rotation = -Mathf.Atan2(_joystick.Horizontal, _joystick.Vertical) * Mathf.Rad2Deg;       // Изменить направление ног

            Foots.SetActive(true);                                              
            Foots.GetComponent<Animator>().speed = Vector2.Distance(new Vector2(_joystick.Horizontal, _joystick.Vertical), Vector2.zero)*0.25f;

            } else {
            Foots.SetActive(false);                                                 // Выключить ноги, чтобы не торчали, когда игрок стоит
            }
        if(goal != null) {
            transform.up = goal.transform.position - transform.position;            // Повернуть body в сторону противника
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z);   // Вернуть координаты x и y в нулевое положение. Это самый настоящий костыль!!!
            }

        

    }
}
