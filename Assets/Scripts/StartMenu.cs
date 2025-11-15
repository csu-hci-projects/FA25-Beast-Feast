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
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
