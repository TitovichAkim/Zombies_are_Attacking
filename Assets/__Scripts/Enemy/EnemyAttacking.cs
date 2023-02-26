using UnityEngine;
using System.Collections;

public class EnemyAttacking:MonoBehaviour
{
    public EnemyScriptableObject    enemyParametersSO;  // Ссылка на СО противника

    private GameObject              _targetGO;           // Ссылка на целевой объект
    private Door_Scr                _door;               // Динамичная ссылка на скрипт двери
    private Enemy_Scr               _enemyScript;       // Ссылка на основной скрипт врага

    public GameObject targetGO
    {
        get
        {
            return (_targetGO);
        }
        set
        {
            _targetGO = value;
            if (_targetGO.layer == 13)
            {
                _door = _targetGO.GetComponent<Door_Scr>();
            }
        }
    }

    private void Awake ()
    {
        _enemyScript = GetComponent<Enemy_Scr>();       // Заполнить ссылку на основной скрипт врага
    }
    public IEnumerator AttackTimer ()
    {
        while(_enemyScript.state == enemyState.attacking)   // Пока действует режим атаки
        {
            StartCoroutine(SerialAttackTimer());            // Периодически выполнять серию атак
            yield return new WaitForSeconds(enemyParametersSO.serialReload);    // Делать это с указанным интервалом
        }
    }
    public IEnumerator SerialAttackTimer ()                 // А тут выполняется та самая серия атак
    {
        for (int i = 0; i < enemyParametersSO.numSerial; i++)   // В зависимости от количества ударов в серии
        {
            _Attack(targetGO);                                  // Выполнить удар внутри серии
            yield return new WaitForSeconds(enemyParametersSO.serialReload);    // С задержкой внутри серии
        }
    }

    private void _Attack (GameObject target)
    {
        target.TryGetComponent(out IDamageabel damagabel);
        damagabel.ApplyDamage(enemyParametersSO.damage);
        if(target.layer == 6)               // Если это игрок
        {
            _enemyScript.PLAY_AUDIO(1);     // Воспроизвести звук укуса
        }
        else if(target.layer == 13)         // Если это дверь
        {
            if(_door.doorHp <= 0)           // Если здоровье двери меньше или равно нулю
            {
                _enemyScript.StateRedactor();   // Запустить редактор состояния врага
            }
        }
    }
}