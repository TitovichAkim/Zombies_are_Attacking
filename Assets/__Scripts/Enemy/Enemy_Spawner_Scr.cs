using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Spawner_Scr:MonoBehaviour
{

    [Header("Set in Inspector")]
    public float                        preparationTime = 5;// Время на подготовку к бою
    public float                        spawnTime = 2;      // Время между моментами создания элемента врага

    public TextAsset                    enemyBase_tx;       // Ссылка на текстовую базу параметров противника
    public TextAsset                    spawnerBase_tx;     // Ссылка на текстовую базу координат спаунеров
    public TextAsset                    waveBase_tx;        // Ссылка на список врагов и их тип, которые будут в волне

    public EnemyScriptableObject[]      enemySOBase;        // База параметров противников
    public GameObject                   player;             // Ссылка на игрока
    public GameObject                   WaveTextBoard;      // Ссылка на доску с надписью какая нынче волна
    public GameObject                   ScoreTextBoard;     // Ссылка на доску с количеством очков
    public GameObject                   gameMenuGO;         // Ссылка на ГО игрового меню

    [Header("Set Dynamically")]
    public float                        startTime;          // Время активации сцены
    public Vector2[]                    spawnerBase;        // База координат точек спауна
    public List<int>                    waveBase;           // Для чтения врагов спаунером
    private int                         _score;             // Количество очков


    [Header("Для спауна врагов на сцене")]
    public List<GameObject>             _enemyList;         // Будет хранить объекты всех врагов, находящихся на карте
    public List<Vector2>                spawnList;          // Будет хранить [номер противника в базе противников, оставшееся количество противников в этой волне]
    public Vector2                      enemyesBySpawnList; // Динамическая переменная для создания списка данной волны
    public GameObject                   attackingZone;      // 
    public SoundsScriptableObject       soundsBase;         // База всех звуков

    public int                          waveNumber;         // Хранит номер волны
    private float                       spawnMoment;        // Момент последнего спауна
    public int                          spawnIndex;         // Индекс, необходимый для управления спауном
    private int                         maxSpawnObjects;    // Максимальное количество врагов на сцене в этой волне

    private int                         H;                  // Для считывания базы спауна
    private int                         E;                  // Для считывания базы спауна
    #region ФУНКЦИИ
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
                startTime = Time.time;            // Записать момент, когда закончилась волна
            }
        }
    }

    #endregion

    public void Start ()
    {
        enemyList = new List<GameObject>();                 // Инициализация списка врагов
        spawnerBase = new Vector2[10];                      // Инициализация списка координат спаунов
        waveBase = new List<int>();                         // Инициализация списка врагов в волне
        spawnList = new List<Vector2>();                    // Инициализация списка врагов в волне

        READ_SPAWNER_BASE();                                // Чтение текстового файла с координатами спаунов

        waveNumber = 0;                                     // При старте игры устанавливает значение волны 0, чтобы успеть подготовиться
        score = 0;
        spawnIndex = -1;                                    // При старте игры устанавливает индекс спауна -1

        startTime = Time.time;                              // Записать время старта сцены
    }

    public void FixedUpdate ()
    {
        if(spawnIndex > (-1) & spawnTime <= (Time.time - spawnMoment) & waveNumber > 0 & enemyList.Count < maxSpawnObjects)
        {        // Если База данной волны содержит элементы и время после последнего спауна больше необходимой задержки, номер волны больше нуля, количество врагов на сцене меньше максимального
            CREATE_ENEMY();                     // Создавать противников
        }

        if(enemyList.Count <= 0 & preparationTime <= (Time.time - startTime))
        { // Если список врагов на сцене равен или меньше нуля
            spawnList.Clear();                                                  // Очистить лист спауна
            waveNumber += 1;                                                    // Запустить следующую волну
            if(waveNumber > 20)
            {                                              // Если очередная волна больше чем 20
                gameMenuGO.GetComponent<Game_Menu_Scr>().PLAYER_WIN(score);     // Запустить сценарий победы игрока
            }
            else
            {
                WaveTextBoard.GetComponent<Text>().text = waveNumber.ToString();// Отобразить новую волну на экране
                READ_WAVE_BASE_TXT(waveNumber);                                 // Считать состав данной волны
            }
        }
    }

    // Читает базу существующих точек спауна и записывает координаты в List<>
    public void READ_SPAWNER_BASE ()
    {       // Считывает текстовый файл с координатами и заполняет базу Векторов
        string[] str = spawnerBase_tx.text.Split('\n');
        H = str.Length;
        for(int i = 0; i < H; i++)
        {
            string[] coord = str[i].Split(' ');
            spawnerBase[i] = new Vector2(int.Parse(coord[0]), int.Parse(coord[1]));
        }
    }

    // Читает базу волн и заполняет список текущей волны
    public void READ_WAVE_BASE_TXT (int wave)
    {
        string[] wavesStr = waveBase_tx.text.Split('\n');           // Порезать текстовый файл на строки
        E = wavesStr.Length;                                        // Присвоить значение количества этих строк
        string[] waveStr = wavesStr[wave].Split(' ');               // Порезать указанную строку (wave) на отдельные строки

        maxSpawnObjects = int.Parse(waveStr[0]);                    // Записать, сколько врагов может быть одновременно на сцене в этой волне
        for(int i = 1; i < waveStr.Length; i++)
        {                   // Перебрать полученные выше строки
            if(int.Parse(waveStr[i]) > 0)
            {                         // Если значение этой строки больше, чем "0"
                enemyesBySpawnList.x = i - 1;                       // Записать ячейку с номером этого противника в базе
                enemyesBySpawnList.y = int.Parse(waveStr[i]);       // Записать ячейку с количеством таких врагов в данной волне
                spawnIndex += (int)enemyesBySpawnList.y;            // Добавить эту цифру к индексу, чтобы посчитать, сколько всего врагов будет в волне
                spawnList.Add(Vector2.zero);                        // Создать пустую ячейку в List
                spawnList[spawnList.Count - 1] = enemyesBySpawnList;// Добавить указанную пару значений в пустую ячейку List
            }
        }
    }


    public void CREATE_ENEMY ()
    {
        int random;

        if(spawnList.Count > 1)
        {
            random = (int)Random.Range(0, spawnList.Count);         // Рандомно выбрать, какого врага сейчас создать
        }
        else
        {
            random = 0;
        }
        int enemyIndex = (int)spawnList[random].x;

        GameObject enemyGO = Instantiate(enemySOBase[enemyIndex].enemyPrefab);        // Создать копию врага на поле
        AddComponents(enemyGO);
        Enemy_Scr scriptEnemy = enemyGO.GetComponent<Enemy_Scr>();
        EnemyNavigation navigationEnemy = enemyGO.GetComponent<EnemyNavigation>();
        EnemyHealth enemyHealth = enemyGO.GetComponent<EnemyHealth>();
        EnemyAttacking enemyAttacking = enemyGO.GetComponent<EnemyAttacking>();

        enemyGO.transform.position = spawnerBase[Random.Range(0, spawnerBase.Length)];   // Разместить объект рандомно в точке спауна
        scriptEnemy.spawner = this.gameObject;          // Сделать ссылку на спаунер
        scriptEnemy.spawnerScr = this;                  // Сделать ссылку на скрипт спаунера
        scriptEnemy.player = player;                    // Сделать ссылку на игрока
        scriptEnemy.sounds = soundsBase;                // Сделать ссылку на базу всех звуков
        scriptEnemy.navigation = navigationEnemy;       // Сделать ссылку на скрипт навигации
        enemyHealth.spawner = this;                     // Сделать ссылку на скрипт спаунера
        enemyHealth.playerScript = player.GetComponent<Player_Scr>();// Сделать ссылку на игрока

        navigationEnemy.goal = player.transform;        // Сделать ссылку на трансформ игрока во вкладке "ЦЕЛЬ"
        
        enemyList.Add(enemyGO);

        scriptEnemy.enemyParametersSO = enemySOBase[enemyIndex];
        enemyHealth.enemyParametersSO = enemySOBase[enemyIndex];
        enemyAttacking.enemyParametersSO = enemySOBase[enemyIndex];
        navigationEnemy.enemyParametersSO = enemySOBase[enemyIndex];

        scriptEnemy.StateRedactor();                                    // Назначить новое состояние противника

        spawnList[random] = new Vector2(spawnList[random].x, spawnList[random].y - 1); // Отнять единицу от количества врагов используемого сейчас типв
        if(spawnList[random][1] == 0)                                   // Если количество врагов данного типа равно нулю
        {                                 
            spawnList.RemoveAt(random);                                 // Удалить его из списка врагов в этой волне
        }
        spawnIndex--;                                                   // Сократить на 1 индекс спауна
        spawnMoment = Time.time;                                        // Запомнить момент создания этого объекта

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
