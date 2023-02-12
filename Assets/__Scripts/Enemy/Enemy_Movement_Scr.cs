using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement_Scr:MonoBehaviour
{

    [Header("���������� � ����������� �� ���� �����")]
    public float                    moveSpeed;          // �������� ��������
    public float                    rotationSpeed;      // �������� ��������

    [Header("���������� �����������")]

    public Rigidbody2D              rigid2D;            // ������ �� rigidbody2D
    public Vector2                  moveDirection;      // ����������� ��������
    public GameObject               noseGO;             // ������ �� ��� ����������
    public GameObject               playerGO;           // ������ �� ������

    // ���������� ��� ����
    public GameObject[] lasers = new GameObject[3];         // ������, �������� �����, ����������� ��� ����������� ����
    public Vector3 barrier;                                 // ���������� ����� ���������� ������������
    public int povorot;                                     // ��������, ���� ������������
    public int barrierDirection;                            // ����� ������� �����������, � ������� ���� ���������� ��������
    public bool moveMode = false;                           // ������ ����� ��� �������� ������� ��� ���������� ����� ��� � ����� ������ �� ����������
    public float moveModeTime;                              // �����, ������ ����� ��������� ������ ����� ��������
    public float moveModeFixTime;							// ������, � ������� ��� ������� ������ ����� ��������
    public GameObject barrierGO;                            // ������, �� �������� ������������� ������


    public void Awake ()
    {
        noseGO = new GameObject("Nose");                            // ������� ������ "���"
        noseGO.transform.parent = this.transform;                   // ������� ��� �������� ������� �������
        noseGO.transform.localPosition = new Vector2(0, 0.1f);      // ���������� ��� ������������ ��������
        noseGO.layer = 7;

        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();      // ��������� ������ �� rigidbody2D
    }

    public void Start ()
    {
        rotationSpeed = 10;         // �������� �������� - 10
        CREATE_LASERS();            // ������� ����� ��� �������
    }

    public void FixedUpdate ()
    {
        CREATE_RAY();                                                                                   // ��������� ����
        if(Vector3.Distance(playerGO.transform.position, this.gameObject.transform.position) >= 1f)
        {  // ���������� �� ������ ������ ������
            MOVE();                                                                                     // ����
        }
        ROTATION();                                                                                     // ���������
    }

    public void ROTATION ()
    {
        float rotZk;
        Vector3 difference = playerGO.transform.position - this.transform.position;                 // ���������� �����������, ��� ��������� ���������
        if(difference.magnitude >= 1 && barrierGO == null)
        {                                        // ���� ��������� �� ���������� ������ ������ ������ � ��� ��������
            rotZk = 90;                                                                             // ��������� ����� �� ����
        }
        else
        {                                                                                // ���� ������ ������ ������ ��� ���� ������
            rotZk = Random.Range(0, 1) * 180;                                                       // ����������� � ���� ����� ��� ������ ����� 
        }
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;                       // ���������� ������� ���� �������� (��������, ��� �� �����)
        Quaternion rotation = Quaternion.AngleAxis(rotZ - rotZk, Vector3.forward);                  // ��������� �������� ���������� ��������

        if(barrier == Vector3.zero)
        {
            rigid2D.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * 0.01f); // ��������� ������ � ������� ��������
        }
        else
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime * povorot * 10);
        }
        barrierGO = null;
    }
    public void MOVE ()
    {
        rigid2D.velocity = (noseGO.transform.position - this.gameObject.transform.position).normalized * moveSpeed; // ��������� ������
    }

    public void CREATE_LASERS ()
    {        // ������� ������� ��� ����������� �����, ������������ ��������� ������������	
        GameObject leftLaser = new GameObject("Left Laser");                // ������� ������ "����� �����"
        leftLaser.transform.SetParent(transform);                           // ��������� �������� - ���� ������
        leftLaser.transform.localPosition = new Vector3(-0.07f, 0.07f, 0);  // ��������� ��������� 45 �������� �����
        leftLaser.layer = 7;
        lasers[0] = leftLaser;                                              // �������� � ������ ������� ��� �������� ���������

        GameObject rightLaser = new GameObject("Right Laser");              // ������� ������ "������ �����"
        rightLaser.transform.SetParent(transform);                          // ��������� �������� - ���� ������
        rightLaser.transform.localPosition = new Vector3(0.07f, 0.07f, 0);  // ��������� ��������� 45 �������� ������
        rightLaser.layer = 7;
        lasers[2] = rightLaser;                                             // �������� � ������ ������� ��� �������� ���������

        lasers[1] = noseGO;                                                 // �������� ������ nose ��� ����������� �����
    }

    public void CREATE_RAY ()
    {           // ��������� ��� � ����������������, ��� �� �������� �������
        RaycastHit2D[] hits;                                    // ������ ������ ��������, ������� �������� ���
        bool barrierParam = false;                              // ����������, ������� �������, �������� �� ��� ������� ���� ������ �� ���� ����� ����
        if(barrier == Vector3.zero)
        {                               // ���� �������� ������� �� ������ ������ ����� ����, �������� ��� ������ ��� ����������� ��������


            Vector3[] barriersLR = new Vector3[3];                  // ���� ������ ������� ���������� ����������� ��������, ���� �������� �������

            for(int i = -1; i <= 1; i++)
            {                          // ������������, ����� �� ������� ��������� ������ ����� �������

                hits = Physics2D.RaycastAll(transform.position, (lasers[i + 1].transform.position - transform.position), 2.5f);           // ��������� ��� � �������� ��� ���������� (������������)

                foreach(RaycastHit2D barrierHit in hits)
                {
                    // ����
                    if(barrierHit.collider.gameObject.layer != 6 & barrierHit.collider.gameObject.layer != 7 & barrierHit.collider.gameObject.layer != 8 & barrierHit.collider.gameObject.layer != 13 & barrierHit.collider.gameObject.name != "Visibility_Area")// ����� ������� �� 6, 7, 8                    && (barrier == null || barrier == Vector3.zero      // � barrier ������
                                                                                                                                                                                                                                                                 //             || (transform.position - barrier).magnitude > (new Vector2(transform.position.x, transform.position.y) - barrierHit.point).magnitude) // ��� ����� ����������� ������ ����� ��� ����������
                    {
                        barrier = barrierHit.point;                     // ��������� ����� ���������� ������������
                        barriersLR[i + 1] = barrier;                    // ��������� �� ������, ���� ��������� ��� �������
                        barrierDirection = i;                           // ����������, � ����� ������� ��� ��������� ������
                        barrierParam = true;                            // �������� ������� - ������������
                        povorot = i;                                    // ��������� �������, � ������� ������������
                        barrierGO = barrierHit.collider.gameObject;


                    }
                }
            }

            if(barrierDirection == 0 & barrier != Vector3.zero)
            {                                   // ���� ������ ��������� �������, �������� ����� 
                if(barriersLR[0].magnitude <= barriersLR[2].magnitude)
                {                            // � ������ ���. ��� ������ ���������� �� ��������
                                             // ��� �� ���, ���� � ���������
                    povorot = 1;
                }
                else
                {
                    povorot = -1;
                }
            }

        }
        else
        {                                        // ���� ������ ����������� �����
                                                 // ��������� ��� � �������� ��� ���������� � �����������, ��� ��� ��������� ������ �������
            hits = Physics2D.RaycastAll(transform.position, (lasers[barrierDirection + 1].transform.position - transform.position), 1f);
            foreach(RaycastHit2D barrierHit in hits)
            {
                if(barrierHit.collider.gameObject.layer != 6 & barrierHit.collider.gameObject.layer != 7 & barrierHit.collider.gameObject.layer != 8 & barrierHit.collider.gameObject.layer != 13 & barrierHit.collider.gameObject.name != "Visibility_Area")
                {                        // ���� �� ���� ����� ������ ���-���� ����������� ������
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
