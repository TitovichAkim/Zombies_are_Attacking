using UnityEngine;

interface IDamageabel
{
    void ApplyDamage(int damageValue);
}
interface IRestoringHealth
{
    void RestoreHealth(int healthValue);
} 

public class Interfaces : MonoBehaviour
{

}
