using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private GameObject parentEnemy;

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            parentEnemy.GetComponent<Enemy>().reachedEdge(); // call reachedEdge() in enemy
        }
    }

    void OnTriggerEnter2D(Collider2D collision) // when landing on ground
    {
        if (collision.CompareTag("Ground"))
        {
            parentEnemy.GetComponent<Enemy>().jumping = false; // set jumping to false
        }
    }

    void Awake()
    {
        if (transform.parent != null) // if there is a parent enemy
            parentEnemy = transform.parent.gameObject; // get enemy that this is acting for
    }
}
