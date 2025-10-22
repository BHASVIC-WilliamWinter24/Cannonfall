using UnityEngine;

public class JumpDetector : MonoBehaviour
{
    private GameObject parentEnemy;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            parentEnemy.GetComponent<Enemy>().jumpLedge(); // call jumpLedge() in enemy
        }
    }

    void Awake()
    {
        if (transform.parent != null) // if there is a parent enemy
            parentEnemy = transform.parent.gameObject; // get enemy that this is acting for
    }
}
