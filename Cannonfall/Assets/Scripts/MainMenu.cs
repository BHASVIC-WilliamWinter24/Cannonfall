using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject NewGameButton; // index 0
    [SerializeField] GameObject LoadButton; // index 1
    [SerializeField] GameObject QuitButton; // index 2
    private int selectedButton = 0; // which button is currently selected
    private int buffer = 0; // slows down menu navigation

    void Update()
    {
        Select();
        Navigate();
    }

    void Select()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
        {
            if (selectedButton == 0) // new game
            {
                SceneManager.LoadScene("New Game Menu");
            }
            else if (selectedButton == 1) // load
            {
                SceneManager.LoadScene("Load Menu");
            }
            else if (selectedButton == 2) // quit
            {
                Debug.Log("Quit");
                Application.Quit(); 
            }
        }
    }

    void Navigate()
    {
        float navigate = Input.GetAxisRaw("Horizontal");
        if (navigate < 0 && buffer == 0)
        {
            selectedButton -= 1;
            if (selectedButton < 0) // if goes out of range
                selectedButton = 2; // resets as highest value
            buffer = 30;
        }
        else if (navigate > 0 && buffer == 0)
        {
            selectedButton += 1;
            if (selectedButton > 2) // if goes out of range
                selectedButton = 0; // resets as lowest value
            buffer = 30;
        }
        else if (buffer > 0)
        {
            buffer--;
        }
        if (navigate == 0)
        {
            buffer -= 10;
            if (buffer < 0)
                buffer = 0;
        }

        if (selectedButton == 0)
        {
            NewGameButton.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold; // make bold
            LoadButton.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal; // make normal
            QuitButton.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal; // make normal
        }
        else if (selectedButton == 1)
        {
            NewGameButton.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal; // make normal
            LoadButton.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold; // make bold
            QuitButton.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal; // make normal
        }
        else if (selectedButton == 2)
        {
            NewGameButton.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal; // make normal
            LoadButton.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal; // make normal
            QuitButton.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold; // make bold        
        }
        else
        {
            Debug.Log("selectedButton out of range"); // show that some error has occurred 
        }
    }
}
