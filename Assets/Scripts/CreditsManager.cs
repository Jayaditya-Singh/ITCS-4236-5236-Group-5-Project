
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsManager : MonoBehaviour
{
    [Header("Screen Sections")]
    public CanvasGroup winScreen;
    public CanvasGroup creditsScreen;

    [Header("Credits Crawl")]
    public RectTransform creditsText;
    public float scrollSpeed = 30f;

    [Header("Transitions")]
    public float fadeDuration = 2f;
    public float endPause = 0.5f;
    public CanvasGroup flashOverlay;
    public float flashDuration = 0.2f;

    private IEnumerator Start()
    {
        creditsScreen.alpha = 0f;
        flashOverlay.alpha = 0f;

        // Show win screen
        yield return new WaitForSeconds(4f);

        // Fade out win screen
        yield return Fade(winScreen, 1f, 0f, fadeDuration);
        winScreen.gameObject.SetActive(false);

        // Set credits starting position BEFORE showing them
        RectTransform panel = creditsText.parent as RectTransform;

        float panelHeight = panel.rect.height;
        float textHeight = creditsText.rect.height;

        Vector2 position = creditsText.anchoredPosition;
        position.y = -panelHeight / 2f - textHeight / 2f;
        creditsText.anchoredPosition = position;

        // Fade in credits
        creditsScreen.gameObject.SetActive(true);
        yield return Fade(creditsScreen, 0f, 1f, fadeDuration);

        // Scroll credits completely off the top
        float endPosition = panelHeight / 2f + textHeight / 2f;

        while (creditsText.anchoredPosition.y < endPosition)
        {
            creditsText.anchoredPosition +=
                Vector2.up * scrollSpeed * Time.deltaTime;

            yield return null;
        }

        // Short pause after credits finish
        yield return new WaitForSeconds(endPause);

        // Quick white flash
        yield return Fade(flashOverlay, 0f, 1f, flashDuration);

        // Return to main menu
        SceneManager.LoadScene("Title");
    }

    private IEnumerator Fade(
        CanvasGroup group, float start, float end, float duration)
    {
        float elapsed = 0f;
        group.alpha = start;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            group.alpha = Mathf.Lerp(
                start,
                end,
                elapsed / duration
            );

            yield return null;
        }

        group.alpha = end;
    }
}
