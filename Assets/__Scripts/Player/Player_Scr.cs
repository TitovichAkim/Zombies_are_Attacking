using System.Collections.Generic;
using UnityEngine;

public class Player_Scr:MonoBehaviour
{

    // �������������� ��������� 
    public int              hp = 100;               // ���� ��������
    public int              coin;                   // ���������� ������

    // ������ ������������ ����������


    // ���������� ��������

    public GameObject       bulletPrefab;           // ������ �� ������ ����
    public GameObject       goal;                   // ������ �� ������ �������� ����������
    public GameObject       forcedGoal;             // �������������� ����, ������� �������� �������� �� ����������
    public GameObject       mainCamera;             // ������ �� ������� ������
    public GameObject       gameMenu;               // ������ �� ������� ����
    public AudioSource      weaponAudioSourse;      // ������ �� ������ ��������������� ������ ��������


    public List<GameObject> bullDirections;         // �������� �������� ������� ����������� ��������
    public GameObject[]     weaponCase = new GameObject[2]; // �������� ������ �� ������ ���������� ������

    private float           shotTime;               // ������ ���������� ��������

    #region ���������� �������� 
    [SerializeField]private PlayerInventory inventory;              // ������ �� ���������� ������
    [SerializeField]private AudioClip       shotAudio;              // ���� �������
    [SerializeField]private AudioClip       reloadAudio;            // ���� �����������
    public int              storeMagazine;          // 0 ������� ��������
    private float           reloadMagazine;         // 1 �������� ����������� ��������
    private float           reloadTime;             // 2 ����� ����� ����������
    private int             weaponDamage;           // 3 ��������� ����
    private int             numberOfBullets;        // 4 ���������� ����, ������� ���������� �������
    private int             bulletSpeed;            // 5 �������� ����

    private bool            _shotCondition;         // ��������� ��������

    public int              activeWeapon = 0;       // ������ ���������� �� �������� ������ ������ (0, 1)
    public Vector2          loadMagazine;           // ������� �������� ��������
    #endregion

    public List<GameObject> GoalsList;              // ������ ����������� � ���� ���������

    public bool             adBool = false;         // ����������, ������������� �� ��� ������� ��� �����������

    void Start ()
    {
        TakeAWeapon(0);     // �������� �������������� ������� ������
        CREATE_BULL_DIR();
        //inventory = this.gameObject.GetComponent<PlayerInventory>();
        GoalsList = new List<GameObject>();        // ���������������� ���� ��� �������� ���������� ������

        weaponAudioSourse = GetComponent<AudioSource>();    // ��������� ������ �� ������ ��������������� ������ ��������

        coin = 0;
    }
    void Update ()
    {

        GO_SHOT();          // ��������� �������� ��������

        if(GoalsList.Count <= 0)
        {
            this.gameObject.GetComponent<Player_Controller>().goal = null;
        }

        if(hp <= 0)
        {
            if(adBool == false)
            {
                gameMenu.GetComponent<Game_Menu_Scr>().OPEN_AD_FORHP_ANSWER("YES");
            }
            else
            {
                gameMenu.GetComponent<Game_Menu_Scr>().OPEN_AD_FORHP_ANSWER("NO");
            }
        }
    }

    #region ��������� �������
    public void Shot (bool shot)
    {
        _shotCondition = shot;
    }
    private void GO_SHOT ()
    {
        if(loadMagazine[activeWeapon] == 0 && reloadTime <= (Time.time - shotTime))
        {
            loadMagazine[activeWeapon] = storeMagazine;
        }
        if(reloadTime < Time.time - shotTime && _shotCondition && GoalsList.Count != 0 && loadMagazine[activeWeapon] >= 0)
        {
            SelectAGoal(); // ������� ����
            for(int i = 0; i < numberOfBullets; i++)
            {
                GameObject bullEx = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(bullDirections[i].transform.position));
                //bullEx.transform.up = bullDirections[i].transform.position - transform.position;
                bullEx.GetComponent<Bullet_Scr>().playerPos = this.gameObject.transform.position;
                bullEx.GetComponent<Bullet_Scr>().goalPos = bullDirections[i].transform.position;
                bullEx.GetComponent<Bullet_Scr>().damage = weaponDamage;
                bullEx.GetComponent<Bullet_Scr>().bulletSpeed = bulletSpeed;
            }       // ������� ������(�) ���� (�����)

            PLAY_SHOT_AUDIO(shotAudio);             // �������� ������� ��������������� ������

            shotTime = Time.time;                   // ��������� ������ ��������
            loadMagazine[activeWeapon] -= 1;        // ������ �� ���������� �������� � �������� ���� �������
            if(loadMagazine[activeWeapon] == 0)
            {
                shotTime += reloadMagazine;
                PLAY_SHOT_AUDIO(reloadAudio);       // �������� ����������� ��������������� ������
            }
        }
    }
    #endregion

    public void CREATE_BULL_DIR ()
    {
        bullDirections = new List<GameObject>();
        float[] goalRedact = new float[7] { 0, 0.2f, -0.2f, 0.3f, -0.3f, 0.4f, -0.4f };
        for(int i = 0; i < 7; i++)
        {
            GameObject bullDir = new GameObject("bullDir"+i);
            bullDir.transform.parent = transform;
            bullDir.transform.position = new Vector2(transform.localPosition.x + goalRedact[i], transform.localPosition.y + 1);
            bullDirections.Add(bullDir.gameObject);
        }
    }   // ������� ������������ ��� ����������

    public void PLAY_SHOT_AUDIO (AudioClip clip)
    {
        weaponAudioSourse.PlayOneShot(clip);
    } // ������������� �����

    public void RELOAD_MAGAZINE ()
    {
        Debug.Log("�����������");
        shotTime = Time.time + reloadMagazine;
        loadMagazine[activeWeapon] = 0;
        PLAY_SHOT_AUDIO(reloadAudio);                       // �������� ����������� ��������������� ������
    }

    public void TakeAWeapon (int newWeaponNumber)
    {
        WeaponsScriptableObject weapon = inventory.weaponBelt[newWeaponNumber];
        int oldActiveWeapon = activeWeapon;
        activeWeapon = newWeaponNumber;

        shotAudio = weapon.shotAudio;                     // ���� ��������
        reloadAudio = weapon.reloadWeapAudio;             // ���� ����������� ������
        storeMagazine = weapon.storeMagazine;             // 0 ������� ��������
        reloadMagazine = weapon.reloadMagazine;           // 1 �������� ����������� ��������
        reloadTime = weapon.reloadTime;                   // 2 ����� ����� ����������
        weaponDamage = weapon.weaponDamage;               // 3 ��������� ����
        numberOfBullets = weapon.numberOfBullets;         // 4 ���������� ����, ������� ���������� �������
        bulletSpeed = weapon.bulletSpeed;                 // 5 �������� ����

        if(oldActiveWeapon == activeWeapon)               // ���� �������� ���� �� ���������, ������, ��� ���� �������
        {
            loadMagazine[activeWeapon] = storeMagazine;   // ����� �������� ����� ���������� ���������� ��������
        }
    }
    private void SelectAGoal ()
    {
        if(forcedGoal == null)  // ���� ������������ ���� �����������, ��������� ����������� ������� ����������
        {
            foreach(GameObject go in GoalsList)
            {
                if(goal == null || Vector2.Distance(go.transform.position, this.transform.position) < Vector2.Distance(goal.transform.position, this.transform.position))
                {
                    goal = go;
                    this.gameObject.GetComponent<Player_Controller>().goal = go;

                    transform.up = go.transform.position - transform.position;          // ����� �����������, � ������ ���������� ��������, ����������� ��������� ����� ������ � ����� ����������
                }
            }
        }
        else
        {                // ���� ���� ������������ ����
            goal = forcedGoal;      // ���� - ������������ ����
            this.gameObject.GetComponent<Player_Controller>().goal = forcedGoal;    // ���� - ������������ ����
            transform.up = goal.transform.position - transform.position;            // ����������� � ������������ ����
        }
    }
}
