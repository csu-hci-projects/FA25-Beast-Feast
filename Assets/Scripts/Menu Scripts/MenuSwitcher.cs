using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject creditsPanel;
    public GameObject characterEdit;

    public void ShowCredits()
    {
        menuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        creditsPanel.SetActive(false);
        //characterEdit.SetActive(false);
        menuPanel.SetActive(true);
    }

    

    public void CharacterEdit()
    {
        menuPanel.SetActive(false);
        characterEdit.SetActive(true);
    }

    public void BackToMenu1()
    {
        //creditsPanel.SetActive(false);
        characterEdit.SetActive(false);
        menuPanel.SetActive(true);
    }
}