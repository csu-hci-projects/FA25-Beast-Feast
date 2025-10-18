using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject creditsPanel;

    public void ShowCredits()
    {
        menuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        creditsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}