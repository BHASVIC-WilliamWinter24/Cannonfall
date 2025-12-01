using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, startPos) > 2)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Cannonball"))
            Destroy(gameObject);
        if (collision.CompareTag("Enemy"))
            collision.GetComponent<Enemy>().EnemyDeath();
    }
}
