using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageabel
{
    public Player_Scr               playerScript;       // ������ �� ������ ������
    public Enemy_Spawner_Scr        spawner;            // ������ �� ������ ��������
    public EnemyScriptableObject    enemyParametersSO;  // ������ �� �� ����������

    private float       _hp;                            // ������� ��������
    private Enemy_Scr   _enemyScript;                   // ������ �� �������� ������

    public float hp
    {
        get
        {
            return (_hp);
        }
        set
        {
            _hp = value;
            if (_hp <= 0)
            {
                DestroyEnemy();
                _enemyScript.StateRedactor(null, true);
            }
        }
    }

    private void Awake ()
    {
        _enemyScript = GetComponent<Enemy_Scr>();
    }
    private void Start ()
    {
        hp = enemyParametersSO.HP;      // ���������������� �������� ��������
    }
    public void ApplyDamage (int damageValue)
    {
        hp -= damageValue;              // �������� ����
        _enemyScript.PLAY_AUDIO(1);     // ������������� ���� ��������� �����
    }

    private void DestroyEnemy ()
    {
        spawner.enemyList.Remove(gameObject);       // ������ ����� ����� �� ������ ������
        spawner.score += enemyParametersSO.score;   // ��������� ���� �� �����������
        playerScript.coin += enemyParametersSO.coin;// ��������� ����� �� �����������
    }
}
