using UnityEngine;
[CreateAssetMenu(fileName = "EnemyesData", menuName = "Items/Enemyes")]
public class EnemyScriptableObject: ScriptableObject
{
    public string       nameEnemy;          // ������������ �����
    public float        HP;                 // ������� ��������
    public float        speedOfMovement;    // �������� ��������
    public float        speedRotation;      // �������� ��������
    public int          damage;             // ��������� ����
    public float        reload;             // ����� ����������� ����� ������
    public int          numSerial;          // ���������� ������ � �����
    public float        serialReload;       // ����� ����������� ������ �����
    public int          score;              // ���������� �����, ������� ������������� ������ �� �����������
    public int          coin;               // ���������� Coin, ������� ������������� ������ �� �����������
    public float        probalityBonus;     // ����������� ��������� ������ ��� �����������
    public float        attackingZoneRadius;// ������ ��������� ���������� ���� �����

    public GameObject   enemyPrefab;        // ������ ���������
}
