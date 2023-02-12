using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Scr : MonoBehaviour {

    private Rigidbody2D rigid2d;
    public Vector2      goalPos;                    // ���������� �������, � ������� �������� ��������� ����
    public float        bulletSpeed;                // �������� ������ ���� 
    public float        damage;                     // ��������� ����

    public GameObject   playerGO;                   // ������ �� ������ ������
    public Vector2      playerPos;                  // ������� ������ � ������ ��������

    public float        createMoment;               // ������ �������� ����

    public void Awake() {
        rigid2d = this.GetComponent<Rigidbody2D>(); // ���������� ������ �� ������� ����
        createMoment = Time.time;

        }

    void FixedUpdate() {
        rigid2d.velocity = (goalPos - playerPos).normalized * bulletSpeed;  // ������������ �������� ����

        if(Time.time - createMoment > 5) {          // ���� ������� ����� �������� �������� � ������� �������� ������ 5 ������
            Destroy(this.gameObject);               // ����������������
            }
        
    }

    public void OnTriggerEnter2D(Collider2D collision) {                                            // ���� ����������� � ����-��
        if (collision.gameObject.layer != 6 & collision.gameObject.name != "Visibility_Area") {     // ���� ��� �� ����� � �� ��� ���� ������
            Destroy(this.gameObject);                                                               // ����������������
            }
        }
    }
