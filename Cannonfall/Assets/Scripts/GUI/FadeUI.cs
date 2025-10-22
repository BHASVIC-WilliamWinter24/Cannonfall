using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FadeUI : MonoBehaviour
{
    [SerializeField] private GameObject objectToFade; // reference to object
    [SerializeField] private float fadeSpeed;
    public bool fadeIn;

    void Update()
    {
        Fade(fadeIn);
    }

    private void Fade(bool fadeIn)
    {
        Color objectColor = objectToFade.GetComponent<Image>().color; // get colour of object
        float desiredFade;
        if (fadeIn)
            desiredFade = 1; // fully opaque
        else
            desiredFade = 0; // fully transparent
        objectColor.a = Mathf.MoveTowards(objectColor.a, desiredFade, fadeSpeed * Time.deltaTime); // fade
        objectToFade.GetComponent<Image>().color = objectColor; // change
    }
}
