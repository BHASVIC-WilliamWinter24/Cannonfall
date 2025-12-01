using UnityEngine;

public class PlayerGrounder : MonoBehaviour
{
    private GameObject player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            player.GetComponent<Player>().groundBelow = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)    
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            player.GetComponent<Player>().groundBelow = false;
        }
    }
}
