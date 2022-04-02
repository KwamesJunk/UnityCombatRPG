using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
   CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        //print("Fading");

    }

 
    public IEnumerator FadeOut(float time)
    {
        do
        {
            yield return null;

            canvasGroup.alpha += Time.deltaTime / time;

        } while (canvasGroup.alpha < 1.0f);
    }

    public IEnumerator FadeIn(float time)
    {
        do
        {
            yield return null;

            canvasGroup.alpha -= Time.deltaTime / time;

        } while (canvasGroup.alpha > 0.0f);
    }

    public void BlackOut()
    {
        canvasGroup.alpha = 1.0f;
    }
  
    // music:
    // Mobile Night Market
    // Hello Meteor
    // The End of All Known Land
}
