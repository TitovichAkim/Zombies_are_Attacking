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

    public List<GameObject>             enemyBase = new List<GameObject>(); // ������ ������ �� ������� ���� ������
    public GameObject                   player;             // ������ �� ������
    public GameObject                   WaveTextBoard;      // ������ �� ����� � �������� ����� ����� �����
    public GameObject                   ScoreTextBoard;     // ������ �� ����� � ����������� �����
    public GameObject                   gameMenuGO;         // ������ �� �� �������� ����

    [Header("Set Dynamically")]
    public float                        startTime;          // ����� ��������� �����
    public Vector2[]                    spawnerBase;        // ���� ��������� ����� ������
    public List<int>                    waveBase;           // ��� ������ ������ ���������
    public int                          score;              // ���������� �����


    [Header("��� ������ ������ �� �����")]
    public List<GameObject>             enemyList;          // ����� ������� ������� ���� ������, ����������� �� �����
    public List<Vector2>                spawnList;          // ����� ������� [����� ���������� � ���� �����������, ���������� ���������� ����������� � ���� �����]
    public Vector2                      enemyesBySpawnList; // ������������ ���������� ��� �������� ������ ������ �����

    public int                          waveNumber;         // ������ ����� �����
    private float                       spawnMoment;        // ������ ���������� ������
    public int                          spawnIndex;         // ������, ����������� ��� ���������� �������
    private int                         maxSpawnObjects;    // ������������ ���������� ������ �� ����� � ���� �����

    private int                         H;                  // ��� ���������� ���� ������
    private int                         E;                  // ��� ���������� ���� ������

    [Header("��������� �����")]
    public string[]                     enemyesNames = new string[13];      // ������ ����� ���� ��������� ����������
    public int[]                        enemyesHP = new int[13];            // ������ ���������� HP ���� ��������� ����������
    public float[]                      enemyesSpeed = new float[13];       // ������ �������� ���� ��������� ����������
    public float[]                      enemyesRotationSpeed = new float[13];// ������ �������� �������� ���� ��������� ����������
    public int[]                        enemyesDamage = new int[13];        // ������ ��������� ���� ���� ��������� ����������
    public float[]                      enemyesSerialSpeed = new float[13]; // ������ ����� ����� ������� ���� ���� ��������� ����������
    public int[]                        enemyesNumSerial = new int[13];     // ������ ���������� ������ � ����� ���� ���� ��������� ����������
    public float[]                      enemyesAttackSpeed = new float[13]; // ������ �������� ����� � ����� ���� ��������� ����������
    public int[]                        enemyesScore = new int[13];         // ������ ���������� Score, ������� �������� ��� ����������� ���� ��������� ����������
    public int[]                        enemyesCoin = new int[13];          // ������ ���������� Coin, ������� �������� ��� ����������� ���� ��������� ����������
    public float[]                      enemyesProbalityBonus = new float[13];// ������ ����������� ��������� ������ ��� ����������� ���� ��������� ����������

    public void Start ()
    {
        enemyList = new List<GameObject>();                 // ������������� ������ ������
        spawnerBase = new Vector2[10];                      // ������������� ������ ��������� �������
        waveBase = new List<int>();                         // ������������� ������ ������ � �����
        spawnList = new List<Vector2>();                    // ������������� ������ ������ � �����

        READ_ENEMY_BASE();                                  // ������ ���������� ����� � ����������� ���� �����������
        READ_SPAWNER_BASE();                                // ������ ���������� ����� � ������������ �������

        waveNumber = 0;                                     // ��� ������ ���� ������������� �������� ����� 0, ����� ������ �������������
        score = 0;
        spawnIndex = -1;                                    // ��� ������ ���� ������������� ������ ������ -1

        SCORE_BOARD_RELOAD();

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
    // ������ �������������� ����������� ���� ��������, �������� ����� � ������ 
    public void READ_ENEMY_BASE ()
    {
        string[] eBase = enemyBase_tx.text.Split('\n');
        for(int i = 0; i < eBase.Length; i++)
        {
            string[] oneEnemyBase = eBase[i].Split(' ');
            enemyesNames[i] = oneEnemyBase[0];
            enemyesHP[i] = int.Parse(oneEnemyBase[1]);
            enemyesSpeed[i] = float.Parse(oneEnemyBase[2]);
            enemyesRotationSpeed[i] = float.Parse(oneEnemyBase[3]);
            enemyesDamage[i] = int.Parse(oneEnemyBase[4]);
            enemyesSerialSpeed[i] = float.Parse(oneEnemyBase[5]);
            enemyesNumSerial[i] = int.Parse(oneEnemyBase[6]);
            enemyesAttackSpeed[i] = float.Parse(oneEnemyBase[7]);
            enemyesScore[i] = int.Parse(oneEnemyBase[8]);
            enemyesCoin[i] = int.Parse(oneEnemyBase[9]);
            enemyesProbalityBonus[i] = float.Parse(oneEnemyBase[10]);

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
        GameObject enemyGO = Instantiate(enemyBase[enemyIndex]);        // ������� ����� ����� �� ����
        Enemy_Scr scriptEnemy = enemyGO.GetComponent<Enemy_Scr>();
        EnemyNavigation navigationEnemy = enemyGO.GetComponent<EnemyNavigation>();
        enemyGO.transform.position = spawnerBase[Random.Range(0, spawnerBase.Length)];   // ���������� ������ �������� � ����� ������
        scriptEnemy.spawner = this.gameObject;          // ������� ������ �� �������
        scriptEnemy.spawnerScr = this;                  // ������� ������ �� ������ ��������
        scriptEnemy.player = player;                    // ������� ������ �� ������
        navigationEnemy.goal = player.transform;        // ������� ������ �� ��������� ������ �� ������� "����"          
        enemyList.Add(enemyGO);

        // ��������� ���������� ������ ���������
        enemyGO.name = enemyesNames[enemyIndex];
        scriptEnemy.HP = enemyesHP[enemyIndex];
        navigationEnemy.moveSpeed = enemyesSpeed[enemyIndex];
        navigationEnemy.rotationSpeed = enemyesRotationSpeed[enemyIndex];
        scriptEnemy.damage = enemyesDamage[enemyIndex];
        scriptEnemy.serialReload = enemyesSerialSpeed[enemyIndex];
        scriptEnemy.numSerial = enemyesNumSerial[enemyIndex];
        scriptEnemy.reload = enemyesAttackSpeed[enemyIndex];
        scriptEnemy.score = enemyesScore[enemyIndex];
        scriptEnemy.coin = enemyesCoin[enemyIndex];
        scriptEnemy.probalityBonus = enemyesProbalityBonus[enemyIndex];

        scriptEnemy.state = enemyState.stalking;                        // ������ ���������� - �������������

        spawnList[random] = new Vector2(spawnList[random].x, spawnList[random].y - 1); // ������ ������� �� ���������� ������ ������������� ������ ����
        if(spawnList[random][1] == 0)                                   // ���� ���������� ������ ������� ���� ����� ����
        {                                 
            spawnList.RemoveAt(random);                                 // ������� ��� �� ������ ������ � ���� �����
        }
        spawnIndex--;                                                   // ��������� �� 1 ������ ������
        spawnMoment = Time.time;                                        // ��������� ������ �������� ����� �������

    }

    public void SCORE_BOARD_RELOAD ()
    {
        ScoreTextBoard.GetComponent<Text>().text = score.ToString();
    }
}
