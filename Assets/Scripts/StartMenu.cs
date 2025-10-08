using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void LoadDungeon()
    {
        SceneManager.LoadScene("Dungeon");
    }

    public void LoadIsland()
    {
        SceneManager.LoadScene("Island");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
