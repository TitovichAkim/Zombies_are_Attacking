using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum doorState
{
    intact, // ����������
    broken  // �������
}

public class Door_Scr: MonoBehaviour, IDamageabel
{
    private int             _doorHp = 30;           // �������� �����
    public BoxCollider2D    doorCollider;           // ������ �� ���������
    public SpriteRenderer   doorSprite;             // ������ �� ������

    public GameObject       came;                   // ������ �� ������� ������

    public AudioSource      audioSourse;            // ������ �� ����� �����


    doorState               state = doorState.intact;

    public int doorHp
    {
        get
        {
            return (_doorHp);
        }
        set
        {
            _doorHp = value;
            if(_doorHp <= 0)
            {
                DoorDestroy();
            }
        }
    }

    public void Start ()
    {
        this.gameObject.AddComponent<AudioSource>();                // ���������� ��������� ��������������� ������
        audioSourse = this.gameObject.GetComponent<AudioSource>();  // ������� ������� ������ �� ��������� ��������������� ������
        audioSourse.volume = 0.3f;                                  // ������� ������� ��������� ������ �� 30%
        audioSourse.spatialBlend = 1;                               // ������� ���� 3D

        doorSprite = GetComponentInParent<SpriteRenderer>();
        doorCollider = transform.parent.gameObject.GetComponent<BoxCollider2D>();
        came = GameObject.FindGameObjectWithTag("MainCamera");

    }
    public void OnTriggerStay2D (Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            if (state == doorState.broken && collision.gameObject.GetComponent<Player_Scr>().coin >= 1)      // ���� ����� ���� ��������� � � ������ ���� ������ �� ������
            {
                if(_doorHp <= 0)
                {
                    collision.gameObject.GetComponent<Player_Scr>().coin -= 1;
                    came.GetComponent<Enemy_Spawner_Scr>().score += 1;
                }
                _doorHp = 30;
                doorCollider.enabled = true;
                doorSprite.enabled = true;
                PLAY_AUDIO(1);                  // ������������� ���� �������
                state = doorState.intact;       // ��������� � ��������� "���������"
            }

        }
    }

    // ��������� �������� �������������
    public void PLAY_AUDIO (int ch)
    {
        switch(ch)
        {
            case 0: // ����� ����� ���������
                audioSourse.PlayOneShot(came.GetComponent<Create_Map_Scr>().doorFalls[0]);
                break;
            case 1: // ����� ����� ���������������
                audioSourse.PlayOneShot(came.GetComponent<Create_Map_Scr>().doorRepair[Random.Range(0, 1)]);
                break;
            case 2: // ����� ���� �� �����
                audioSourse.PlayOneShot(came.GetComponent<Create_Map_Scr>().doorHitting[Random.Range(0, came.GetComponent<Create_Map_Scr>().doorHitting.Count - 1)]);
                break;
        }
    }

    public void ApplyDamage (int damageValue)
    {
        doorHp -= damageValue;
        PLAY_AUDIO(2);
    }
    void DoorDestroy ()
    {
        _doorHp = 0;
        doorCollider.enabled = false;       // ��������� ���������
        doorSprite.enabled = false;         // ��������� ����������� �����

        if(state == doorState.intact)
        {    // ���� �� ����� ���� ��������� "����������"
            PLAY_AUDIO(0);                  // ���������� ���� ����������
            state = doorState.broken;       // �������� ��������� ����� �� "�������"
        }
    }

}
