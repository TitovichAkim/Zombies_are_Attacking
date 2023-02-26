using UnityEngine;
using System.Collections;

public class EnemyAttacking:MonoBehaviour
{
    public EnemyScriptableObject    enemyParametersSO;  // ������ �� �� ����������

    private GameObject              _targetGO;           // ������ �� ������� ������
    private Door_Scr                _door;               // ���������� ������ �� ������ �����
    private Enemy_Scr               _enemyScript;       // ������ �� �������� ������ �����

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
        _enemyScript = GetComponent<Enemy_Scr>();       // ��������� ������ �� �������� ������ �����
    }
    public IEnumerator AttackTimer ()
    {
        while(_enemyScript.state == enemyState.attacking)   // ���� ��������� ����� �����
        {
            yield return new WaitForSeconds(enemyParametersSO.serialReload);    // ������ ��� � ��������� ����������
            StartCoroutine(SerialAttackTimer());            // ������������ ��������� ����� ����
        }
    }
    public IEnumerator SerialAttackTimer ()                 // � ��� ����������� �� ����� ����� ����
    {
        for (int i = 0; i < enemyParametersSO.numSerial; i++)   // � ����������� �� ���������� ������ � �����
        {
            _Attack(targetGO);                                  // ��������� ���� ������ �����
            yield return new WaitForSeconds(enemyParametersSO.serialReload);    // � ��������� ������ �����
        }
    }

    private void _Attack (GameObject target)
    {
        target.TryGetComponent(out IDamageabel damagabel);
        damagabel.ApplyDamage(enemyParametersSO.damage);
        if(target.layer == 6)               // ���� ��� �����
        {
            _enemyScript.PLAY_AUDIO(1);     // ������������� ���� �����
        }
        else if(target.layer == 13)         // ���� ��� �����
        {
            if(_door.doorHp <= 0)           // ���� �������� ����� ������ ��� ����� ����
            {
                _enemyScript.StateRedactor();   // ��������� �������� ��������� �����
            }
        }
    }
}