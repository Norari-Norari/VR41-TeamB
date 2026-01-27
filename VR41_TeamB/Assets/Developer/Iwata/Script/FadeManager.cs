
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : B_SingletonMonoBehaviour<FadeManager>
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    public IEnumerator FadeOut()
    {
        yield return Fade(0f, 1f);
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float from, float to)
    {
        float time = 0f;
        Color c = fadeImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;
            c.a = Mathf.Lerp(from, to, t);
            fadeImage.color = c;
            yield return null;
        }

        c.a = to;
        fadeImage.color = c;
    }
}
