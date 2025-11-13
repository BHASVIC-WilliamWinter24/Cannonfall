using UnityEngine;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    private Vector3 playerPosition; // postition of player
    [SerializeField] float activationDistance; // distance that the checkpoint will activate from
    [SerializeField] GameObject interactPopUp;
    bool enteredRange = false;

    void Update()
    {
        if (Time.timeScale != 0)
        {
            playerPosition = GameObject.FindWithTag("Player").transform.position; // gets the position of the player
            float distance = Vector3.Distance(transform.position, playerPosition); // distance between the two
            if (distance < activationDistance)
            {
                enteredRange = true;
                StopCoroutine(popup(false)); // stop any coroutine hiding the popup
                StartCoroutine(popup(true)); // start showing the popup
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F)) // if press E or F
                {
                    if (SceneManager.GetActiveScene().name == "Level 1")
                        //SceneManager.LoadScene("Level 2");
                        Debug.Log("Level 2");
                    if (SceneManager.GetActiveScene().name == "Level 2")
                        SceneManager.LoadScene("Level 3");
                    if (SceneManager.GetActiveScene().name == "Level 3")
                        Debug.Log("GAME END");
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
}