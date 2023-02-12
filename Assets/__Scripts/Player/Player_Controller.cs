using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
    
public class Player_Controller : MonoBehaviour {

    public GameObject                       goal;               // ����, �� ������� ������� �����
    public GameObject                       Foots;              // �������� ������ � ������

    public Rigidbody2D                      footsRigid;         // ���������� ���

    [SerializeField] private Rigidbody2D    _rigidbody;         // ���������� ������
    [SerializeField] private FixedJoystick  _joystick;          // ������ �� ��������

    [SerializeField] private float _moveSpeed = 1.5f;                  // �������� �������� ������

    public void Awake() {
        footsRigid = Foots.gameObject.GetComponent<Rigidbody2D>(); // ��������� ������ �� ���������� ���
        }

    private void FixedUpdate(){
        // �������� ��������� �� ���������
        _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _joystick.Vertical * _moveSpeed);
        Foots.transform.position = this.gameObject.transform.position;

        // ����������� ����������� ������� � ������ �������
        if (_joystick.Horizontal != 0 && _joystick.Vertical !=0) {
            _rigidbody.rotation = -Mathf.Atan2(_joystick.Horizontal, _joystick.Vertical) * Mathf.Rad2Deg;       // �������� ����������� ��������
            footsRigid.rotation = -Mathf.Atan2(_joystick.Horizontal, _joystick.Vertical) * Mathf.Rad2Deg;       // �������� ����������� ���

            Foots.SetActive(true);                                              
            Foots.GetComponent<Animator>().speed = Vector2.Distance(new Vector2(_joystick.Horizontal, _joystick.Vertical), Vector2.zero)*0.25f;

            } else {
            Foots.SetActive(false);                                                 // ��������� ����, ����� �� �������, ����� ����� �����
            }
        if(goal != null) {
            transform.up = goal.transform.position - transform.position;            // ��������� body � ������� ����������
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z);   // ������� ���������� x � y � ������� ���������. ��� ����� ��������� �������!!!
            }

        

    }
}
