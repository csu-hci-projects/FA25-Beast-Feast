using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    public void Exit()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
