using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

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

    [Header("SetInSpawner")]
    // GameObjects
    public GameObject               spawner;                // ������ �� ������� (����������� ���������)
    public GameObject               player;                 // ������ �� ������ (����������� ���������)
    public GameObject               attackingZone;          // ������ �� ���� ����� (����������� ���������)
    // Classes
    public EnemyNavigation          navigation;             // ������ �� ������ ��������� (����������� ���������)
    public Enemy_Spawner_Scr        spawnerScr;             // ������ �� ������ �������� (����������� ���������)
    // ScriptableObjects
    public EnemyScriptableObject    enemyParametersSO;      // ������ �� �� ���������� (����������� ���������)
    public SoundsScriptableObject   sounds;                 // ������ �� �������� ���� (����������� ���������)

    [Header("SetInStart")]

    public Rigidbody2D              rigid2D;                // ������ �� Rigidbody2d �������
    private EnemyAttacking          _enemyAttacking;        // ������ �� ������ ����� (����������� � ������)
    public AudioSource              audioSource;            // ������ �� ���������, ��������������� �����

    [Header("SetDynamically")]
    public bool                     forcedGoalBool = false; // �������� �� ���� ������ ������������
    private Animator                animator;               // �������� ����������
    private GameObject              _targetGO;              // ������� ������ 

    public void Start ()
    {
        _enemyAttacking = GetComponent<EnemyAttacking>();               // ��������� ������ �� ������ �����
        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();          // ��������� ������ �� Rigidbody2d �������
        animator = GetComponentInChildren<Animator>();                  // ��������� ������ �� ��������

        //attackingZone.GetComponent<CircleCollider2D>().radius = enemyParametersSO.attackingZoneRadius;
        attackingZone.GetComponent<EnemyAttackingZone>().enemyScript = this;

        CREATE_AUDIO_SOURSE();
        StateRedactor();
    }

    public void CREATE_AUDIO_SOURSE ()      // ������� ���������, ��������������� ����� ����� � ����������� ���
    {
        this.gameObject.AddComponent<AudioSource>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
        audioSource.spatialBlend = 1;
    }

    public void PLAY_AUDIO (int aud)
    {
        if(audioSource.clip == null)
        {
            switch(aud)
            {
                case 0:     // ��������� ��� ��� ���-�� ��� 
                    audioSource.PlayOneShot(sounds.zombieWalkingSounds[Random.Range(0, sounds.zombieWalkingSounds.Length)]);
                    break;
                case 1:     // ����� ������
                    audioSource.PlayOneShot(sounds.zombieBiteSounds[Random.Range(0, sounds.zombieBiteSounds.Length)]);
                    break;
                case 2:     // ����� ��������� � ���� ��������
                    audioSource.PlayOneShot(sounds.zombieHitSounds[Random.Range(0, sounds.zombieHitSounds.Length)]);
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

    public void StateRedactor (GameObject target = null, bool destroy = false)
    {
        if(destroy)
        {
            state = enemyState.dead;
        } 
        else
        {
            if(target != null)
            {
                _targetGO = target;
                state = enemyState.attacking;                                                           // �������� ����� �����
            } 
            else
            {
                if(StaticGameManager.gameMode != "survival")
                {
                    state = enemyState.stay;
                }
                else
                {
                    state = enemyState.stalking;                                                            // �������� ����� �������� � ����
                }
            }
        }
        �hangeState();  // ��������� ��� �������� �� ������������ ���������
    }

    private void �hangeState () // �������� ��������� ���������� � ����������� �� ��� ������ ���������
    {

        switch(state)
        {
            case enemyState.stay:

                break;
            case enemyState.walk:
                RepeatTheSound();
                break;
            case enemyState.stalking:
                animator.SetBool("target", true);
                animator.SetBool("move", true);

                navigation.agent.speed =  enemyParametersSO.speedOfMovement;                            // ����������� �������� ���������
                navigation.InvokeRepeating("UpdatePlayerPosition", 0f, 1f);                             // ��������� ���� ����������� ���� � ����
                RepeatTheSound();
                break;

            case enemyState.attacking:
                animator.SetBool("target", true);
                animator.SetBool("move", false);

                navigation.agent.speed = 0;                                                             // �������� - ����
                _enemyAttacking.targetGO = _targetGO;                                                   // ��������� ���� ��� �����
                StartCoroutine(_enemyAttacking.AttackTimer());                                          // ��������� �������� �����
                break;

            case enemyState.dead:
                Destroy(gameObject);                                                                    // ���������� ������
                break;
        }
    }

    private void RepeatTheSound ()      // ��������� ��������� ����. ���� ��� ���� ��� ������������� � ��������
    {
        if (state == enemyState.walk || state == enemyState.walk)
        {
            PLAY_AUDIO(0);                                          // ������������� ���� �������
            Invoke("RepeatTheSound", Random.Range(4f, 20f));        // ��������� ���������� ����� ������ ����� ��������� ���������� �������
        }
    }
}
