using UnityEngine;

public class Cannonball : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Hazard"))
            Destroy(gameObject);
    }
}
