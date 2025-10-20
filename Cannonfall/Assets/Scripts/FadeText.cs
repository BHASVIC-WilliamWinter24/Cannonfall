using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FadeText : MonoBehaviour
{
    [SerializeField] private GameObject objectToFade; // reference to object
    [SerializeField] private float fadeSpeed;

    public IEnumerator Fade(bool fadeIn)
    {
        Color objectColor = objectToFade.GetComponent<TextMeshProUGUI>().color; // get reference to colour
        float fadeAmount;
        if (fadeIn) // if fading to black
        {
            while (objectToFade.GetComponent<TextMeshProUGUI>().color.a < 1) // continue until fully opaque
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime); // how much the thing should fade by
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount); // change colour by fade amount
                objectToFade.GetComponent<TextMeshProUGUI>().color = objectColor; // set this colour to the screen
                yield return null; // stop code when completed loop
            }
        }
        else // if fading from black
        {
            while (objectToFade.GetComponent<TextMeshProUGUI>().color.a > 0) // continue until fully transparent
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime); // how much the thing should fade by
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount); // change colour by fade amount
                objectToFade.GetComponent<TextMeshProUGUI>().color = objectColor; // set this colour to the screen
                yield return null; // stop code when completed loop
            }
        }
    }
}