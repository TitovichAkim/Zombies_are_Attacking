using UnityEngine;

public class EnemyAttackingZone : MonoBehaviour
{
    public Enemy_Scr        enemyScript;

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if(collision.gameObject.CompareTag("damagabelDoor") || collision.gameObject.CompareTag("player"))
        {
            enemyScript.StateRedactor(collision.gameObject);
        }
    }
    private void OnTriggerExit2D (Collider2D collision)
    {
        if(collision.gameObject.CompareTag("damagabelDoor") || collision.gameObject.CompareTag("player"))
        {
            enemyScript.StateRedactor();
        }
    }
}
