using UnityEngine;

public class Cannonball : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Hazard") || collision.CompareTag("Enemy"))
            Destroy(gameObject);
        if (collision.CompareTag("Enemy"))
            collision.GetComponent<Enemy>().EnemyDeath();
    }
}
