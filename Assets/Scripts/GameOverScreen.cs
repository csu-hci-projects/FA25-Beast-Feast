using System.Collections;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] Image gameOverScreen;
    [SerializeField] Image winScreen;
     private AudioSource source;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;
    
    public float fadeDuration = 1.0f;

    void Start()
    {
        // Example: Start fading out the image
        
        SetImageAlpha(0f);
        StartCoroutine(FadeImageAlpha(0f, false));
        StartCoroutine(FadeImageAlpha(0f, true));
        source = GetComponent<AudioSource>();
        //gameOverText.enabled = false;
    }

    IEnumerator FadeImageAlpha(float targetAlpha, bool win)
    {
        float startAlpha;
        if (win) {
            startAlpha = winScreen.color.a;
        } else
        {
            startAlpha = gameOverScreen.color.a;
        }
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
        winScreen.color = newColor;
    }

    public void GameOver()
    {
        winScreen.enabled = false;
        gameOverScreen.enabled = true;
        StartCoroutine(FadeImageAlpha(1f, false));
        source.clip = loseClip;
        source.Play();
        //gameOverText.enabled = true;
        StartCoroutine(DelayedAction(3.0f)); // Call DelayedAction after 3 seconds
    }

    IEnumerator DelayedAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene("StartMenu");
        
    }

    public void Win()
    {
        gameOverScreen.enabled = false;
        winScreen.enabled = true;
        StartCoroutine(FadeImageAlpha(1f, true));
        source.clip = winClip;
        source.Play();
        StartCoroutine(DelayedAction(3.0f)); // Call DelayedAction after 3 seconds 
    }
}
