using System.Collections;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] Image gameOverScreen;
    [SerializeField] TextMeshProUGUI gameOverText;
    
    public float fadeDuration = 1.0f;

    void Start()
    {
        // Example: Start fading out the image
        StartCoroutine(FadeImageAlpha(0f));
        gameOverText.enabled = false;
    }

    IEnumerator FadeImageAlpha(float targetAlpha)
    {
        float startAlpha = gameOverScreen.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            SetImageAlpha(currentAlpha);
            yield return null;
        }

        SetImageAlpha(targetAlpha); // Ensure final alpha is set precisely
    }

    void SetImageAlpha(float newAlpha)
    {
        Color currentColor = gameOverScreen.color;
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
        gameOverScreen.color = newColor;
    }

    public void GameOver()
    {
        StartCoroutine(FadeImageAlpha(1f));
        gameOverText.enabled = true;
        StartCoroutine(DelayedAction(3.0f)); // Call DelayedAction after 3 seconds
    }

    IEnumerator DelayedAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene("StartMenu");
        
    }
}
