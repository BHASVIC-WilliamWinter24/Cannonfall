using System;
using TMPro;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private GameObject upgradeName;
    [SerializeField] private GameObject upgradeDetails;
    private int upgrade;
    private string[] upgradeTextNames = {"SPEED BOOST", "MORE RECOIL", "EXTRA JUMP", "EXPLOSIVE SHOT", "DOUBLE BARREL"};
    private string[] upgradeTextDetails = {"+10% speed", "+5% jump strength", "+1 extra jump", "cannonball explodes on impact", "+1 cannonball per jump"};
    private int buffer;
    private bool active = false; 

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
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) && buffer != 0)
        {
            active = false;
            Time.timeScale = 1; // unpause
            FadePopup(false);
        }
    }

    public void Activate(int upgrade)
    {
        active = true;
        FadePopup(true);
        this.upgrade = upgrade; 
        buffer = 2;
        upgradeName.GetComponent<TextMeshProUGUI>().text = upgradeTextNames[upgrade];
        if (upgrade == 3)
            upgradeName.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 142); // move to higher position
        else
            upgradeName.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100); // move to normal position
        upgradeDetails.GetComponent<TextMeshProUGUI>().text = upgradeTextDetails[upgrade];
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