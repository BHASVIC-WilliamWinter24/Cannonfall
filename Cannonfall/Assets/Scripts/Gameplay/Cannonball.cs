using UnityEngine;

public class Cannonball : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    public bool upgradeActive = false;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Hazard") || collision.CompareTag("Enemy")) 
        {
            if (upgradeActive && !collision.CompareTag("Enemy")) // if explosive upgrade and not enemy
            {
                GameObject exp = Instantiate(explosion);
                exp.transform.position = transform.position; // move to here
            }
            Destroy(gameObject);
        }
        if (collision.CompareTag("Enemy")) // if hits enemy
            collision.GetComponent<Enemy>().EnemyDeath(); // kill enemy
    }
}
