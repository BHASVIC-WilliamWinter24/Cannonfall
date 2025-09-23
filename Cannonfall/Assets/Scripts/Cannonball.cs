using UnityEngine;

public class Cannonball : MonoBehaviour
{
    /*
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            Destroy(gameObject);
    }*/
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
            Destroy(gameObject);
    }
}
