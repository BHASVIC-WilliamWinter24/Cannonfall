using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Unity.VisualScripting;
using TMPro;

public class NewGameMenu : MonoBehaviour
{
    [SerializeField] GameObject boxSelector;
    private Vector2 saveSlotDimensions = new Vector2(854, 295);
    private Vector3 slot1Pos = new Vector3(-439, 120);
    private Vector3 slot2Pos = new Vector3(505, 120, 0);
    private Vector3 slot3Pos = new Vector3(-439, -175, 0);
    private Vector3 slot4Pos = new Vector3(505, -175, 0);
    private Vector2 backDimensions = new Vector2(370, 150);
    private Vector3 backPos = new Vector3(745, -450);
    private int selectedButton = 0;
    private int buffer = 0;
    private bool overwritePopup;
    public bool OverwritePopup { get { return overwritePopup; }  set { overwritePopup = value; } }

    void Update()
    {
        if (!overwritePopup)
        {
            ShowData();
            Select();
            Navigate();
        }
    }

    void ShowData()
    {
        for (int n = 1; n <= 4; n++)
        {
            string fileName = Application.persistentDataPath + "slot" + n + ".save";
            if (checkSaveEmpty(fileName))
            {
                GameObject.Find("Save Slot " + n).GetComponent<TextMeshProUGUI>().text = "SLOT " + n + " - EMPTY";
                GameObject.Find("Save Details " + n).GetComponent<TextMeshProUGUI>().text = "Empty Save";
            }
            else
            {
                string time = SaveSystem.readFileTime(n);
                if (time == null)
                    time = "N/A";
                GameObject.Find("Save Slot " + n).GetComponent<TextMeshProUGUI>().text = "SLOT " + n + " - "; // add level
                GameObject.Find("Save Details " + n).GetComponent<TextMeshProUGUI>().text = "Saved: " + time;
            }
            
        }
    }

    bool checkSaveEmpty(string fileName)
    {
        bool emptySlot = false;
        if (File.Exists(fileName))
        {
            string fileContents = File.ReadAllText(fileName);
            if (fileContents == null || fileContents == "")
            {
                emptySlot = true;
            }
        }
        else
        {
            emptySlot = true;
        }
        return emptySlot;
    }

    void Select()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
        {
            if (selectedButton == 0) // slot 1
            {
                string fileName = Application.persistentDataPath + "slot1" + ".save";
                if (!checkSaveEmpty(fileName))
                {
                    overwriteCheck(1);
                }
                else
                {
                    GameManager.instance.SaveSlot = 1;
                    SceneManager.LoadScene("Gameplay");
                }
            }
            else if (selectedButton == 1) // slot 2
            {
                string fileName = Application.persistentDataPath + "slot2" + ".save";
                if (!checkSaveEmpty(fileName))
                {
                    overwriteCheck(2);
                }
                else
                {
                    GameManager.instance.SaveSlot = 2;
                    SceneManager.LoadScene("Gameplay");
                }
            }
            else if (selectedButton == 2) // slot 3
            {
                string fileName = Application.persistentDataPath + "slot1" + ".save";
                if (!checkSaveEmpty(fileName))
                {
                    overwriteCheck(3);
                }
                else
                {
                    GameManager.instance.SaveSlot = 3;
                    SceneManager.LoadScene("Gameplay");
                }
            }
            else if (selectedButton == 3) // slot 4
            {
                string fileName = Application.persistentDataPath + "slot1" + ".save";
                if (!checkSaveEmpty(fileName))
                {
                    overwriteCheck(4);
                }
                else
                {
                    GameManager.instance.SaveSlot = 4;
                    SceneManager.LoadScene("Gameplay");
                }
            }
            else if (selectedButton == 4) // back
            {
                SceneManager.LoadScene("Main Menu"); // go to main menu
            }
        }
    }

    private void overwriteCheck(int slot)
    {
        GameObject popup = GameObject.Find("Overwrite Popup");
        popup.GetComponent<SaveOverwrite>().FadePopup(true);
        OverwritePopup = true;
        popup.GetComponent<SaveOverwrite>().saveSlot = slot;
    }

    void Navigate()
    {
        float navigate = Input.GetAxisRaw("Horizontal");
        if (navigate < 0 && buffer == 0)
        {
            selectedButton -= 1;
            if (selectedButton < 0) // if goes out of range
                selectedButton = 4; // resets as highest value
            buffer = 30;
        }
        else if (navigate > 0 && buffer == 0)
        {
            selectedButton += 1;
            if (selectedButton > 4) // if goes out of range
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
        RectTransform rectTransform = boxSelector.GetComponent<RectTransform>();
        if (selectedButton == 0) //  save 1
        {
            rectTransform.localPosition = slot1Pos;
            rectTransform.sizeDelta = saveSlotDimensions;
        }
        else if (selectedButton == 1) // save 2
        {
            rectTransform.localPosition = slot2Pos;
            rectTransform.sizeDelta = saveSlotDimensions;
        }
        else if (selectedButton == 2) // save 3
        {
            rectTransform.localPosition = slot3Pos;
            rectTransform.sizeDelta = saveSlotDimensions;
        }
        else if (selectedButton == 3) // save 4
        {
            rectTransform.localPosition = slot4Pos;
            rectTransform.sizeDelta = saveSlotDimensions;
        }
        else if (selectedButton == 4) // back button
        {
            rectTransform.localPosition = backPos;
            rectTransform.sizeDelta = backDimensions;
        }
        else
        {
            Debug.Log("selectedButton out of range"); // show that some error has occurred 
        }
    }

}
