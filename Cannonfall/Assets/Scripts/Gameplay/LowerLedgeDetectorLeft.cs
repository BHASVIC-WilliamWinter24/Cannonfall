using System;
using UnityEngine;

public class LowerLedgeDetectorLeft : MonoBehaviour
{
    private GameObject parentEnemy;
    private bool ledge;

    void OnTriggerExit2D(Collider2D collision) // when exiting platform (no ledge)
    {
        if (collision.CompareTag("Ground"))
        {
            ledge = false; // there is no ledge below
        }
    }

    void OnTriggerStay2D(Collider2D collision) // when in platform
    {
        if (collision.CompareTag("Ground"))
        {
            ledge = true; // there is a ledge below
        }
    }

    void Update()
    {
        if (ledge)
            parentEnemy.GetComponent<Enemy>().ledgeBelowLeft = true; // there is a ledge below
        else
            parentEnemy.GetComponent<Enemy>().ledgeBelowLeft = false; // there is no ledge below
    }

    void Awake()
    {
        if (transform.parent != null) // if there is a parent enemy
            parentEnemy = transform.parent.gameObject; // get enemy that this is acting for
        parentEnemy.GetComponent<Enemy>().ledgeBelowLeft = true; // set true initally as it will spawn on a platform
    }
}
