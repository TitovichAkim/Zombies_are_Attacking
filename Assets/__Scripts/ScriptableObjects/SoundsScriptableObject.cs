using UnityEngine;

[CreateAssetMenu(fileName = "Sounds", menuName = "Items/Sounds")]
public class SoundsScriptableObject:ScriptableObject
{
    public AudioClip[]      zombieWalkingSounds;        // �����, ������� ������������� ����� �� ����� ��������
    public AudioClip[]      zombieBiteSounds;           // �����, ������� ������������� ����� �� ����� ������
    public AudioClip[]      zombieHitSounds;            // �����, ������� ������������� ����� �� ����� ��������� �����
}

