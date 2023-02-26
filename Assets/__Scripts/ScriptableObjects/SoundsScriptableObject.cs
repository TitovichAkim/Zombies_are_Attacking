using UnityEngine;

[CreateAssetMenu(fileName = "Sounds", menuName = "Items/Sounds")]
public class SoundsScriptableObject:ScriptableObject
{
    public AudioClip[]      zombieWalkingSounds;        // Звуки, который воспроизводит зомби во время движения
    public AudioClip[]      zombieBiteSounds;           // Звуки, который воспроизводит зомби во время укусов
    public AudioClip[]      zombieHitSounds;            // Звуки, который воспроизводит зомби во время получения урона
}

