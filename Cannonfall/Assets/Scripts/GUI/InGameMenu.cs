using UnityEngine;
using TMPro;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] GameObject Continue;
    [SerializeField] GameObject quit;
    private int selectedButton = 0; // starts on continue
    private int buffer = 0;
    private bool active = false;

    void Update()
    {
        if (active)
        {
            Select();
            Navigate();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                active = false;
                FadePopup(false);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                active = true;
                FadePopup(true);
            }
        }
    }

    void Select()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
        {
            if (selectedButton == 0) // continue
            {
                active = false;
                FadePopup(false);
            }
            else if (selectedButton == 1) // quit
            {
                QuitPopup();
            }
        }
    }

    void Navigate()
    {
        float navigate = Input.GetAxisRaw("Vertical");
        if (navigate > 0 && buffer == 0)
        {
            selectedButton -= 1;
            if (selectedButton < 0) // if goes out of range
                selectedButton = 1; // resets as highest value
            buffer = 30;
        }
        else if (navigate < 0 && buffer == 0)
        {
            selectedButton += 1;
            if (selectedButton > 1) // if goes out of range
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
        if (selectedButton == 0) // overwrite 
        {
            Continue.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            quit.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        }
        else if (selectedButton == 1) // cancel
        {
            Continue.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
            quit.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        }
        else
        {
            Debug.Log("selectedButton out of range");
        }
    }

    public void FadePopup(bool fadeBool)
    {
        foreach (Transform child in transform)
        {
            try // if has  UI
            {
                child.gameObject.GetComponent<FadeUI>().fadeIn = fadeBool;
            }
            catch // if doesn't have UI (must have text)
            {
                child.gameObject.GetComponent<FadeText>().fadeIn = fadeBool;
            }
        }
    }

    void QuitPopup()
    {
        
    }
}
