using UnityEngine;

public class QuitPopup : MonoBehaviour
{
    [SerializeField] GameObject quit;
    [SerializeField] GameObject cancel;
    private int selectedButton = 1; // starts on cancel
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
    }

    void Select()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
        {
            if (selectedButton == 0) // quit
            {
                
            }
            else if (selectedButton == 1) // cancel
            {
                
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
            
        }
        else if (selectedButton == 1) // cancel
        {
            
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
}