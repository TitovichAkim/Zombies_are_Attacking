using System.Collections.Generic;
using UnityEngine;
public enum enemyState
{
    stay,       // ����� �� �����
    walk,       // ��������
    stalking,   // ����������
    attacking,  // �������
    dead        // �����
}
public class Enemy_Scr:MonoBehaviour
{
    public enemyState   state;
    [Header("Set in Inspector")]
    public float        HP;                 // ������� ��������
    public int          damage;             // ��������� ����
    public float        serialReload;       // ����� ����������� ����� ������
    public int          numSerial;          // ���������� ������ � �����
    public float        reload;             // ����� ����������� ������ �����
    public int          score;              // ���������� �����, ������� ������������� ������ �� �����������
    public int          coin;               // ���������� Coin, ������� ������������� ������ �� �����������
    public float        probalityBonus;     // ����������� ��������� ������ ��� �����������

    public List<AudioClip> randomSounds;    // ��������� ����� �����
    public List<AudioClip> biteSounds;      // ����� ������
    public List<AudioClip> hitSounds;       // ����� ��������� ������



    [Header("Set Dynamically")]
    public bool         attInd = false;         // ����� �����
    public bool         forcedGoalBool = false; // �������� �� ���� ������ ������������

    public float        attackMoment;           // ������ ���������� �����
    public float        theMomentOfGrowling;    // ������ ���������� �������
    public float        timerOfGrowling;        // ������ �������

    public EnemyNavigation navigation;          // ������ �� ������ ���������
    public Enemy_Spawner_Scr spawnerScr;        // ������ �� ������ ��������

    public GameObject   spawner;        // ������ �� �������
    public GameObject   player;         // ������ �� ������
    public GameObject   targetGO;       // ������� ������
    public Vector2      playerPos;      // ���������� ������� ������
    public Vector2      goPos;          // ���������� ������� �������

    public Rigidbody2D rigid2D;        // ������ �� Rigidbody2d �������
    public AudioSource audioSource;    // ������ �� ���������, ��������������� �����

    // ���������, ������� ������� �� ���� �������
    public float        moveSpeed;     // �������� ��������



    public void Start ()
    {
        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();      // ��������� ������ �� Rigidbody2d �������
        navigation = this.gameObject.GetComponent<EnemyNavigation>(); // ��������� ������ �� ������ ���������

        timerOfGrowling = Random.Range(4f, 20f);                    // ��� �������� ������� �������� ����� ������ �������
        theMomentOfGrowling = Time.time;                            // � ������� ������, ����� ����� ����� �� �����
        CREATE_AUDIO_SOURSE();
    }

    public void FixedUpdate ()
    {
        playerPos = player.transform.position;
        goPos = this.gameObject.transform.position;

        if (state == enemyState.attacking)
        {
            if(targetGO != null)
            {
                ATTACK(targetGO);       // ���������
            }
            else
            {
                StateRedactor(player);
            }
        }


        if(timerOfGrowling <= Time.time - theMomentOfGrowling)
        {   // ���� ������ ������� ������, ��� ������� ����� ������� �������� � �������� ���������� �������
            PLAY_AUDIO(0);                                          // ������������� ���� �������
            theMomentOfGrowling = Time.time;                        // �������� ������ ���������� �������
            timerOfGrowling = Random.Range(4f, 20f);                // ������� ����� ������ �� ���������� �������
        }

    }

    public void OnTriggerEnter2D (Collider2D collision)         // ���� ����� � ��������������� � �������� �������� (����, �����, �����)
    {

        if(collision.gameObject.layer == 8)                                                     // ���� ������������ ������ - ����
        {                                                                                       // ���� ��������� - bullet
            HP -= collision.gameObject.GetComponent<Bullet_Scr>().damage;                       // ������ HP ������ damage �� ����
            PLAY_AUDIO(1);                                                                      // ������������� ���� ��������� � �������� ����
            if(HP <= 0)
            {                                                                      // ���� HP ������ ��� ����� ����
                Destroy(this.gameObject);                                                       // ���������� ������
                spawnerScr.enemyList.Remove(this.gameObject);    // ������� ������ �� ������ ������, ����������� �� �����
                spawnerScr.score += score;                       // �������� ���� ��� �������� �����
                player.GetComponent<Player_Scr>().coin += coin;                                 // �������� Coin ��� �������� �����
                spawnerScr.SCORE_BOARD_RELOAD();                 // ������������� ����� �����
                if(spawnerScr.enemyList.Count == 0)              // ���� ������ ������ �� ����� �������
                {
                    spawnerScr.startTime = Time.time;            // �������� ������, ����� ����������� �����
                }
            }
        }
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 13)                // ���� �������� ������ ��� �����
        {
            StateRedactor(collision.gameObject);                                                // ������� �������� ���������
        }

    }
    /*
    public void OnTriggerStay2D (Collider2D collision)          // ���� ����� � ��������������� � ������
    {
        if(collision.gameObject.layer == 13 && reload <= Time.time - attackMoment && collision.gameObject.GetComponent<Door_Scr>().doorHp > 0)
        {
            navigation.SetTarget(collision.transform);
            attackMoment = Time.time;
            collision.gameObject.GetComponent<Door_Scr>().doorHp -= damage;
            collision.gameObject.GetComponent<Door_Scr>().PLAY_AUDIO(2);
        }
    }*/


    public void ATTACK (GameObject target)
    {
        attackMoment = Time.time;
        if (target.layer == 6)
        {
            target.GetComponent<Player_Scr>().hp -= damage;
            PLAY_AUDIO(1);  // ������������� ���� �����
        }
        else if (target.layer == 13)
        {
            target.GetComponent<Door_Scr>().doorHp -= damage;
            target.GetComponent<Door_Scr>().PLAY_AUDIO(2);
        }

        /*
        if(Vector3.Distance(playerPos, goPos) <= 1.1f & reload <= Time.time - attackMoment)
        {
            attackMoment = Time.time;
            player.GetComponent<Player_Scr>().hp -= damage;

            PLAY_AUDIO(1);  // ������������� ���� �����
        }*/
    }

    public void CREATE_AUDIO_SOURSE ()
    {
        this.gameObject.AddComponent<AudioSource>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
        audioSource.spatialBlend = 1;
    } // ������� ���������, ��������������� ����� ����� � ����������� ���

    public void PLAY_AUDIO (int aud)
    {
        if(audioSource.clip == null)
        {
            switch(aud)
            {
                case 0:     // ��������� ��� ��� ���-�� ��� 
                    audioSource.PlayOneShot(randomSounds[Random.Range(0, randomSounds.Count - 1)]);
                    break;
                case 1:     // ����� ������
                    audioSource.PlayOneShot(biteSounds[Random.Range(0, biteSounds.Count - 1)]);
                    break;
                case 2:     // ����� ��������� � ���� ��������
                    audioSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Count - 1)]);
                    break;
            }
        }
    }
    public void SELECT_AN_OBJECT ()
    {                                                                            // ��������� ������� ��������� ��� ������ ������������ ����
        if(forcedGoalBool == false)
        {                                                                           // ���� ��� ���� �� ������������
            if(player.GetComponent<Player_Scr>().forcedGoal != null)
            {                                             // ���� � ������ ���� ������ ������������ ����
                player.GetComponent<Player_Scr>().forcedGoal.GetComponent<Enemy_Scr>().forcedGoalBool = false;      // ����� ���������� � ������ ������������ ����
                player.GetComponent<Player_Scr>().forcedGoal.gameObject.GetComponent<SpriteRenderer>().enabled = false; // ��������� ������ "���������" �� ���������� ����������
            }
            player.GetComponent<Player_Scr>().forcedGoal = this.gameObject;                                         // �������� ��� ���� � ������ ��� ������������
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;                                          // �������� ������ "���������"
            forcedGoalBool = true;                                                                                  // ������� ���� ����, ��� ��� ������������
        }
        else
        {                                                                                            // ���� ��� ���� ���� ������������
            player.GetComponent<Player_Scr>().forcedGoal = null;                                                    // �������� ��������� � ������
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;                                         // ��������� ������ "���������"
            forcedGoalBool = false;                                                                                 // ������� ���� ����, ��� ��� ������ �� ������������
        }
    }   // ��� ������� �� ���� ������, ������� ������ ����������� � ��� ������� (��������)

    public void OnMouseDown ()
    {
        SELECT_AN_OBJECT();         // ����������� ���������� ����
    }

    public void StateRedactor (GameObject target = null)
    {

        if (target == null)
        {
            state = enemyState.stay;
        }

        if (target != null)
        {
            if (Vector3.Distance(target.transform.position, this.gameObject.transform.position) > 1.5f) // ���� ��������� �� ���� �� � �������� 1.5 ������
            {
                state = enemyState.stalking;                                                            // �������� ����� �������� � ����
                navigation.agent.speed = moveSpeed;                                                     // ����������� �������� ���������
            }
            else
            {
                                                                                                        // ���� ��������� �� ���� �� � �������� 1.5 ������
            }
            {
                state = enemyState.attacking;                                                           // �������� ����� �����
                navigation.agent.speed = 0;                                                             // �������� - ����
            }
            targetGO = target;                                                                          // ������� ������� ������ - ����������� ����
        }
        
    }
}
