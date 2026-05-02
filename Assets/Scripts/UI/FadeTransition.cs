using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeTransition : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadePanel;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float startAlpha = 0f;
    [SerializeField] private float endAlpha = 1f;
    [SerializeField] private int sceneBuildIndex = 1;
    [Header("UI Objects to Disable")]
    [SerializeField] private GameObject uiObject1;
    [SerializeField] private GameObject uiObject2;
    [SerializeField] private GameObject uiObject3;
    private bool isFading = false;

    private void Start()
    {
        if (fadePanel != null)
        {
            Color panelColor = fadePanel.color;
            panelColor.a = startAlpha;
            fadePanel.color = panelColor;
        }
    }

    public void StartFadeTransition()
    {
        if (!isFading)
        {
            if (uiObject1 != null)
            {
                uiObject1.SetActive(false);
            }
            if (uiObject2 != null)
            {
                uiObject2.SetActive(false);
            }
            if (uiObject3 != null)
            {
                uiObject3.SetActive(false);
            }
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        isFading = true;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            if (fadePanel != null)
            {
                Color panelColor = Color.black;
                panelColor.a = Mathf.Lerp(startAlpha, endAlpha, smoothT);
                fadePanel.color = panelColor;
            }
            yield return null;
        }

        if (fadePanel != null)
        {
            Color panelColor = Color.black;
            panelColor.a = endAlpha;
            fadePanel.color = panelColor;
        }

        SceneManager.LoadScene(sceneBuildIndex);
    }
}
