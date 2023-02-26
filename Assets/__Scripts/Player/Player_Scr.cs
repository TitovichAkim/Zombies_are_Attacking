using System.Collections.Generic;
using UnityEngine;

public class Player_Scr:MonoBehaviour
{

    // Характеристики персонажа 
    public int              hp = 100;               // Очки здоровья
    public int              coin;                   // Количество коинов

    // Другие стационарные переменные


    // Переменные стрельбы

    public GameObject       bulletPrefab;           // Ссылка на префаб пули
    public GameObject       goal;                   // Ссылка на объект целевого противника
    public GameObject       forcedGoal;             // Принудительная цель, которую вызывают нажатием на противника
    public GameObject       mainCamera;             // Ссылка на главную камеру
    public GameObject       gameMenu;               // Ссылка на игровое меню
    public AudioSource      weaponAudioSourse;      // Ссылка на объект воспроизведения звуков выстрела


    public List<GameObject> bullDirections;         // Содержит дочерние объекты направлений стрельбы
    public GameObject[]     weaponCase = new GameObject[2]; // Содержит ссылки на кнопки имеющегося оружия

    private float           shotTime;               // Момент последнего выстрела

    #region Переменные стрельбы 
    [SerializeField]private PlayerInventory inventory;              // Ссылка на инвернтарь игрока
    [SerializeField]private AudioClip       shotAudio;              // Звук высрела
    [SerializeField]private AudioClip       reloadAudio;            // Звук перезарядки
    public int              storeMagazine;          // 0 Ёмкость магазина
    private float           reloadMagazine;         // 1 Скорость перезарядки магазина
    private float           reloadTime;             // 2 Время между выстрелами
    private int             weaponDamage;           // 3 Наносимый урон
    private int             numberOfBullets;        // 4 Количество пуль, которые производит выстрел
    private int             bulletSpeed;            // 5 Скорость пули

    private bool            _shotCondition;         // Состояние стрельбы

    public int              activeWeapon = 0;       // Хранит информацию об активной ячейке оружия (0, 1)
    public Vector2          loadMagazine;           // Текущая загрузка магазина
    #endregion

    public List<GameObject> GoalsList;              // Список противников в зоне видимости

    public bool             adBool = false;         // Показывает, запрашивалась ли уже реклама для возрождения

    void Start ()
    {
        TakeAWeapon(0);     // Получить характеристики первого оружия
        CREATE_BULL_DIR();
        //inventory = this.gameObject.GetComponent<PlayerInventory>();
        GoalsList = new List<GameObject>();        // Инициализировать лист для хранения окружающих врагов

        weaponAudioSourse = GetComponent<AudioSource>();    // Заполнить ссылку на объект воспроизведения звуков выстрела

        coin = 0;
    }
    void Update ()
    {

        GO_SHOT();          // Запускать алгоритм стрельбы

        if(GoalsList.Count <= 0)
        {
            this.gameObject.GetComponent<Player_Controller>().goal = null;
        }
    }

    #region СОВЕРШИТЬ ВЫСТРЕЛ
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
            SelectAGoal(); // Выбрать цель
            for(int i = 0; i < numberOfBullets; i++)
            {
                GameObject bullEx = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(bullDirections[i].transform.position));
                //bullEx.transform.up = bullDirections[i].transform.position - transform.position;
                bullEx.GetComponent<Bullet_Scr>().playerPos = this.gameObject.transform.position;
                bullEx.GetComponent<Bullet_Scr>().goalPos = bullDirections[i].transform.position;
                bullEx.GetComponent<Bullet_Scr>().damage = weaponDamage;
                bullEx.GetComponent<Bullet_Scr>().bulletSpeed = bulletSpeed;
            }       // Создает объект(ы) пуль (дроби)

            PLAY_SHOT_AUDIO(shotAudio);             // Озвучить выстрел соответствующим звуком

            shotTime = Time.time;                   // Запомнить момент выстрела
            loadMagazine[activeWeapon] -= 1;        // Отнять из количества патронов в магазине одну единицу
            if(loadMagazine[activeWeapon] == 0)
            {
                shotTime += reloadMagazine;
                PLAY_SHOT_AUDIO(reloadAudio);       // Озвучить перезарядку соответствующим звуком
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
    }   // Создает направляющие для дробовиков

    public void PLAY_SHOT_AUDIO (AudioClip clip)
    {
        weaponAudioSourse.PlayOneShot(clip);
    } // Воспроизводит звуки

    public void RELOAD_MAGAZINE ()
    {
        Debug.Log("Перезаряжаю");
        shotTime = Time.time + reloadMagazine;
        loadMagazine[activeWeapon] = 0;
        PLAY_SHOT_AUDIO(reloadAudio);                       // Озвучить перезарядку соответствующим звуком
    }

    public void TakeAWeapon (int newWeaponNumber)
    {
        WeaponsScriptableObject weapon = inventory.weaponBelt[newWeaponNumber];
        int oldActiveWeapon = activeWeapon;
        activeWeapon = newWeaponNumber;

        shotAudio = weapon.shotAudio;                     // Звук выстрела
        reloadAudio = weapon.reloadWeapAudio;             // Звук перезарядки оружия
        storeMagazine = weapon.storeMagazine;             // 0 Ёмкость магазина
        reloadMagazine = weapon.reloadMagazine;           // 1 Скорость перезарядки магазина
        reloadTime = weapon.reloadTime;                   // 2 Время между выстрелами
        weaponDamage = weapon.weaponDamage;               // 3 Наносимый урон
        numberOfBullets = weapon.numberOfBullets;         // 4 Количество пуль, которые производит выстрел
        bulletSpeed = weapon.bulletSpeed;                 // 5 Скорость пули

        if(oldActiveWeapon == activeWeapon)               // Если активный слот не изменился, значит, это была покупка
        {
            loadMagazine[activeWeapon] = storeMagazine;   // Сразу записать новое количество заряженных патронов
        }
    }
    private void SelectAGoal ()
    {
        if(forcedGoal == null)  // Если приоритетная цель отсутствует, выполнить стандартную выборку приоритета
        {
            foreach(GameObject go in GoalsList)
            {
                if(goal == null || Vector2.Distance(go.transform.position, this.transform.position) < Vector2.Distance(goal.transform.position, this.transform.position))
                {
                    goal = go;
                    this.gameObject.GetComponent<Player_Controller>().goal = go;

                    transform.up = go.transform.position - transform.position;          // После определения, в какого противника стрелять, моментально повернуть морду игрока к этому противнику
                }
            }
        }
        else
        {                // Если есть приоритетная цель
            goal = forcedGoal;      // Цель - приоритетная цель
            this.gameObject.GetComponent<Player_Controller>().goal = forcedGoal;    // Цель - приоритетная цель
            transform.up = goal.transform.position - transform.position;            // Повернуться к приоритетной цели
        }
    }
}
