using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    private Vector3 checkpointPosition; // position of checkpoint
    private Vector3 playerPosition; // postition of player
    [SerializeField] float activationDistance; // distance that the checkpoint will activate from
    private bool activeCheckpoint = false;
    [SerializeField] GameObject interactPopUp;
    bool enteredRange = false;
    private Animator animator;

    void Awake()
    {
        checkpointPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            playerPosition = GameObject.FindWithTag("Player").transform.position; // gets the position of the player
            float distance = Vector3.Distance(checkpointPosition, playerPosition); // distance between the two
            if (distance < activationDistance)
            {
                enteredRange = true;
                if (!activeCheckpoint) // if not active checkpoint
                {
                    StopCoroutine(popup(false)); // stop any coroutine hiding the popup
                    StartCoroutine(popup(true)); // start showing the popup
                }
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F)) // if press E or F
                {
                    activateCheckpoint();
                }
            }
            else
            {
                if (enteredRange) // if has been within range and then exited
                {
                    enteredRange = false; // reset
                    StopCoroutine(popup(true)); // stop any coroutine showing the popup
                    StartCoroutine(popup(false)); // start hiding the popup
                }
            }
            Vector3 currentCheckpoint = GameObject.FindWithTag("Player").GetComponent<Player>().getRespawnPosition(); // get current respawn position
            if (currentCheckpoint != checkpointPosition)
            {
                activeCheckpoint = false;
                animator.SetBool("activeCheckpoint", false);
            }
        }
    }

    IEnumerator popup(bool show)
    {
        if (show)
        {
            while (interactPopUp.transform.position.x < 960f)
            {
                interactPopUp.transform.position += new Vector3(5f, 0f, 0f); 
                if (interactPopUp.transform.position.x > 960f)
                {
                    interactPopUp.transform.position = new Vector3(960f, interactPopUp.transform.position.y, 0f);
                    yield return null;
                }
            }
        }
        else
        {
            while (interactPopUp.transform.position.x > 590f)
            {
                interactPopUp.transform.position -= new Vector3(5f, 0f, 0f);
                if (interactPopUp.transform.position.x < 590f)
                {
                    interactPopUp.transform.position = new Vector3(590f, interactPopUp.transform.position.y, 0f);
                    yield return null;
                }
            }
        }
    }

    public void activateCheckpoint()
    {
        StopCoroutine(popup(true)); // stop any coroutine showing the popup
        StartCoroutine(popup(false)); // start hiding the popup
        GameObject.FindWithTag("Player").GetComponent<Player>().setActiveCheckpoint(gameObject); // set active checkpoint to this object
        activeCheckpoint = true;
        animator.SetBool("activeCheckpoint", true);
        SaveSystem.Save(); // save!
    }

    public Vector3 getPosition()
    {
        return checkpointPosition;
    }
}