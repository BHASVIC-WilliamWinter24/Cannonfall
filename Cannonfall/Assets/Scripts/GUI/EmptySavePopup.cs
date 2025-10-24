using UnityEngine;

public class EmptySavePopup : MonoBehaviour
{ 
    void Start()
    {
        FadePopup(false);
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
