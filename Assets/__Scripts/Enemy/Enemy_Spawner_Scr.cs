using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Spawner_Scr:MonoBehaviour
{

    [Header("Set in Inspector")]
    public float                        preparationTime = 5;// ����� �� ���������� � ���
    public float                        spawnTime = 2;      // ����� ����� ��������� �������� �������� �����

    public TextAsset                    enemyBase_tx;       // ������ �� ��������� ���� ���������� ����������
    public TextAsset                    spawnerBase_tx;     // ������ �� ��������� ���� ��������� ���������
    public TextAsset                    waveBase_tx;        // ������ �� ������ ������ � �� ���, ������� ����� � �����

    public EnemyScriptableObject[]      enemySOBase;        // ���� ���������� �����������
    public GameObject                   player;             // ������ �� ������
    public GameObject                   WaveTextBoard;      // ������ �� ����� � �������� ����� ����� �����
    public GameObject                   ScoreTextBoard;     // ������ �� ����� � ����������� �����
    public GameObject                   gameMenuGO;         // ������ �� �� �������� ����

    [Header("Set Dynamically")]
    public float                        startTime;          // ����� ��������� �����
    public Vector2[]                    spawnerBase;        // ���� ��������� ����� ������
    public List<int>                    waveBase;           // ��� ������ ������ ���������
    private int                         _score;             // ���������� �����


    [Header("��� ������ ������ �� �����")]
    public List<GameObject>             _enemyList;         // ����� ������� ������� ���� ������, ����������� �� �����
    public List<Vector2>                spawnList;          // ����� ������� [����� ���������� � ���� �����������, ���������� ���������� ����������� � ���� �����]
    public Vector2                      enemyesBySpawnList; // ������������ ���������� ��� �������� ������ ������ �����
    public GameObject                   attackingZone;      // 
    public SoundsScriptableObject       soundsBase;         // ���� ���� ������

    public int                          waveNumber;         // ������ ����� �����
    private float                       spawnMoment;        // ������ ���������� ������
    public int                          spawnIndex;         // ������, ����������� ��� ���������� �������
    private int                         maxSpawnObjects;    // ������������ ���������� ������ �� ����� � ���� �����

    private int                         H;                  // ��� ���������� ���� ������
    private int                         E;                  // ��� ���������� ���� ������
    #region �������
    public int score
    {
        get
        {
            return (_score);
        }
        set
        {
            _score = value;
            ScoreTextBoard.GetComponent<Text>().text = score.ToString();
        }
    }
    public List<GameObject> enemyList
    {
        get
        {
            return (_enemyList);
        }
        set
        {
            _enemyList = value;
            if (_enemyList.Count == 0)
            {
                startTime = Time.time;            // �������� ������, ����� ����������� �����
            }
        }
    }

    #endregion

    public void Start ()
    {
        enemyList = new List<GameObject>();                 // ������������� ������ ������
        spawnerBase = new Vector2[10];                      // ������������� ������ ��������� �������
        waveBase = new List<int>();                         // ������������� ������ ������ � �����
        spawnList = new List<Vector2>();                    // ������������� ������ ������ � �����

        READ_SPAWNER_BASE();                                // ������ ���������� ����� � ������������ �������

        waveNumber = 0;                                     // ��� ������ ���� ������������� �������� ����� 0, ����� ������ �������������
        score = 0;
        spawnIndex = -1;                                    // ��� ������ ���� ������������� ������ ������ -1

        startTime = Time.time;                              // �������� ����� ������ �����
    }

    public void FixedUpdate ()
    {
        if(spawnIndex > (-1) & spawnTime <= (Time.time - spawnMoment) & waveNumber > 0 & enemyList.Count < maxSpawnObjects)
        {        // ���� ���� ������ ����� �������� �������� � ����� ����� ���������� ������ ������ ����������� ��������, ����� ����� ������ ����, ���������� ������ �� ����� ������ �������������
            CREATE_ENEMY();                     // ��������� �����������
        }

        if(enemyList.Count <= 0 & preparationTime <= (Time.time - startTime))
        { // ���� ������ ������ �� ����� ����� ��� ������ ����
            spawnList.Clear();                                                  // �������� ���� ������
            waveNumber += 1;                                                    // ��������� ��������� �����
            if(waveNumber > 20)
            {                                              // ���� ��������� ����� ������ ��� 20
                gameMenuGO.GetComponent<Game_Menu_Scr>().PLAYER_WIN(score);     // ��������� �������� ������ ������
            }
            else
            {
                WaveTextBoard.GetComponent<Text>().text = waveNumber.ToString();// ���������� ����� ����� �� ������
                READ_WAVE_BASE_TXT(waveNumber);                                 // ������� ������ ������ �����
            }
        }
    }

    // ������ ���� ������������ ����� ������ � ���������� ���������� � List<>
    public void READ_SPAWNER_BASE ()
    {       // ��������� ��������� ���� � ������������ � ��������� ���� ��������
        string[] str = spawnerBase_tx.text.Split('\n');
        H = str.Length;
        for(int i = 0; i < H; i++)
        {
            string[] coord = str[i].Split(' ');
            spawnerBase[i] = new Vector2(int.Parse(coord[0]), int.Parse(coord[1]));
        }
    }

    // ������ ���� ���� � ��������� ������ ������� �����
    public void READ_WAVE_BASE_TXT (int wave)
    {
        string[] wavesStr = waveBase_tx.text.Split('\n');           // �������� ��������� ���� �� ������
        E = wavesStr.Length;                                        // ��������� �������� ���������� ���� �����
        string[] waveStr = wavesStr[wave].Split(' ');               // �������� ��������� ������ (wave) �� ��������� ������

        maxSpawnObjects = int.Parse(waveStr[0]);                    // ��������, ������� ������ ����� ���� ������������ �� ����� � ���� �����
        for(int i = 1; i < waveStr.Length; i++)
        {                   // ��������� ���������� ���� ������
            if(int.Parse(waveStr[i]) > 0)
            {                         // ���� �������� ���� ������ ������, ��� "0"
                enemyesBySpawnList.x = i - 1;                       // �������� ������ � ������� ����� ���������� � ����
                enemyesBySpawnList.y = int.Parse(waveStr[i]);       // �������� ������ � ����������� ����� ������ � ������ �����
                spawnIndex += (int)enemyesBySpawnList.y;            // �������� ��� ����� � �������, ����� ���������, ������� ����� ������ ����� � �����
                spawnList.Add(Vector2.zero);                        // ������� ������ ������ � List
                spawnList[spawnList.Count - 1] = enemyesBySpawnList;// �������� ��������� ���� �������� � ������ ������ List
            }
        }
    }


    public void CREATE_ENEMY ()
    {
        int random;

        if(spawnList.Count > 1)
        {
            random = (int)Random.Range(0, spawnList.Count);         // �������� �������, ������ ����� ������ �������
        }
        else
        {
            random = 0;
        }
        int enemyIndex = (int)spawnList[random].x;

        GameObject enemyGO = Instantiate(enemySOBase[enemyIndex].enemyPrefab);        // ������� ����� ����� �� ����
        AddComponents(enemyGO);
        Enemy_Scr scriptEnemy = enemyGO.GetComponent<Enemy_Scr>();
        EnemyNavigation navigationEnemy = enemyGO.GetComponent<EnemyNavigation>();
        EnemyHealth enemyHealth = enemyGO.GetComponent<EnemyHealth>();
        EnemyAttacking enemyAttacking = enemyGO.GetComponent<EnemyAttacking>();

        enemyGO.transform.position = spawnerBase[Random.Range(0, spawnerBase.Length)];   // ���������� ������ �������� � ����� ������
        scriptEnemy.spawner = this.gameObject;          // ������� ������ �� �������
        scriptEnemy.spawnerScr = this;                  // ������� ������ �� ������ ��������
        scriptEnemy.player = player;                    // ������� ������ �� ������
        scriptEnemy.sounds = soundsBase;                // ������� ������ �� ���� ���� ������
        scriptEnemy.navigation = navigationEnemy;       // ������� ������ �� ������ ���������
        enemyHealth.spawner = this;                     // ������� ������ �� ������ ��������
        enemyHealth.playerScript = player.GetComponent<Player_Scr>();// ������� ������ �� ������

        navigationEnemy.goal = player.transform;        // ������� ������ �� ��������� ������ �� ������� "����"
        
        enemyList.Add(enemyGO);

        scriptEnemy.enemyParametersSO = enemySOBase[enemyIndex];
        enemyHealth.enemyParametersSO = enemySOBase[enemyIndex];
        enemyAttacking.enemyParametersSO = enemySOBase[enemyIndex];
        navigationEnemy.enemyParametersSO = enemySOBase[enemyIndex];

        scriptEnemy.StateRedactor();                                    // ��������� ����� ��������� ����������

        spawnList[random] = new Vector2(spawnList[random].x, spawnList[random].y - 1); // ������ ������� �� ���������� ������ ������������� ������ ����
        if(spawnList[random][1] == 0)                                   // ���� ���������� ������ ������� ���� ����� ����
        {                                 
            spawnList.RemoveAt(random);                                 // ������� ��� �� ������ ������ � ���� �����
        }
        spawnIndex--;                                                   // ��������� �� 1 ������ ������
        spawnMoment = Time.time;                                        // ��������� ������ �������� ����� �������

    }

    void AddComponents (GameObject enemy)
    {
        enemy.AddComponent<Enemy_Scr>();
        enemy.AddComponent<EnemyNavigation>();
        enemy.AddComponent<EnemyHealth>();
        enemy.AddComponent<EnemyAttacking>();

        GameObject zone = Instantiate(attackingZone, enemy.transform);
        enemy.GetComponent<Enemy_Scr>().attackingZone = zone;
    }
}
