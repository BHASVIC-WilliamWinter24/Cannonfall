using System;
using UnityEngine;

public class LowerLedgeDetectorRight : MonoBehaviour
{
    private GameObject parentEnemy;
    private bool ledge;

    void OnTriggerExit2D(Collider2D collision) // when exiting
    {
        if (collision.CompareTag("Ground"))
        {
            ledge = false; // there is no ledge below
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            ledge = true; // there is a ledge below
        }
    }

    void Update()
    {
        if (ledge)
            parentEnemy.GetComponent<Enemy>().ledgeBelowRight = true; // there is a ledge below
        else
            parentEnemy.GetComponent<Enemy>().ledgeBelowRight = false; // there is no ledge below
    }

    void Awake()
    {
        if (transform.parent != null) // if there is a parent enemy
            parentEnemy = transform.parent.gameObject; // get enemy that this is acting for
        parentEnemy.GetComponent<Enemy>().ledgeBelowRight = true; // set true initally as it will spawn on a platform
    }
}
