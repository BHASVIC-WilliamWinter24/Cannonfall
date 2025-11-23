using UnityEngine;
using UnityEngine.Rendering;

public class SignpostMenu : MonoBehaviour
{
    private int buffer;
    [SerializeField] private bool active = false; 

    void Update()
    {
        if (active)
        {
            Select();
            buffer -= 1;
        }
    }

    private void Select()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Debug.Log("E");
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
        {
            if (buffer <= 0) {
                active = false;
                Time.timeScale = 1; // unpause
                FadePopup(false);
            }
        }
    }

    public void Activate()
    {
        active = true;
        buffer = 2;
        FadePopup(true);
        Time.timeScale = 0; // pause
    }

    private void FadePopup(bool fadeBool)
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
