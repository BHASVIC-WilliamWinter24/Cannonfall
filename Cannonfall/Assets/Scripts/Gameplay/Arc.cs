using UnityEngine;
using System.Collections;

public class Arc : MonoBehaviour
{
    [SerializeField] float zapCycleTime;
    bool arcOn = true;
    private BoxCollider2D arcCollider;
    private Animator animator;


    void Awake()
    {
        arcCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

    }
    
    void Start()
    {
        StartCoroutine(zapCycle());
    }

    IEnumerator zapCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(zapCycleTime); // wait for this much time
            arcOn = !arcOn; // swap the current state of the arc
            if (arcOn) // if electric is on
            {
                animator.SetBool("arcOn", true); // set parameter in animator to true
                arcCollider.enabled = true; // turns collider on
            }
            else
            {
                animator.SetBool("arcOn", false); // set parameter in animator to false
                arcCollider.enabled = false; // turns collider off
            }
        }
    }
}
