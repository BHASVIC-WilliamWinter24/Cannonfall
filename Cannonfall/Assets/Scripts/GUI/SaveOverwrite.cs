using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.IO;
using System.IO;

public class SaveOverwrite : MonoBehaviour
{
    [SerializeField] GameObject overwrite;
    [SerializeField] GameObject cancel;
    public int saveSlot = 0;
    private int selectedButton = 1; // starts on cancel
    private int buffer = 0;
    public int selectBuffer = 2;

    void Start()
    {
        FadePopup(false);
    }

    void Update()
    {
        if (saveSlot != 0)
        {
            Select();
            Navigate();
            if (selectBuffer > 0)
            selectBuffer -= 1;
        }
    }

    void Select()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
        {
            if (selectedButton == 0 && selectBuffer <= 0) // overwrite
            {
                GameManager.instance.SaveSlot = saveSlot;
                string fileName = Application.persistentDataPath + "slot" + saveSlot + ".save";
                //Debug.Log(fileName);
                if (File.Exists(fileName))
                {
                    File.Delete(fileName); // delete previous file
                }
                SceneManager.LoadScene("Level 1");
            }
            else if (selectedButton == 1 && selectBuffer <= 0) // cancel
            {
                FadePopup(false);
                saveSlot = 0;
                GameObject.Find("Background").GetComponent<NewGameMenu>().OverwritePopup = false;
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
            overwrite.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            cancel.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        }
        else if (selectedButton == 1) // cancel
        {
            overwrite.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
            cancel.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        }
        else
        {
            Debug.Log("selectedButton out of range");
        }
    }

    public void FadePopup(bool fadeBool)
    {
        selectBuffer = 2;
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
