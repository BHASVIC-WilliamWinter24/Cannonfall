using UnityEngine;

public class EmptySavePopup : MonoBehaviour
{
    public int saveSlot = 0;    
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
                StartCoroutine(child.gameObject.GetComponent<FadeUI>().Fade(fadeBool));
            }
            catch // if doesn't have UI (must have text)
            {
                StartCoroutine(child.gameObject.GetComponent<FadeText>().Fade(fadeBool));
            }
        }
    }
}
