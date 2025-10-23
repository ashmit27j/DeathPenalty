using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject GameOverScreen;
    public GameObject MenuScreen;
    public GameObject DebuggerUI;

    void Update()
    {
        // ESC key logic
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameOverScreen != null && GameOverScreen.activeSelf)
                return;

            if (MenuScreen != null)
                MenuScreen.SetActive(true);
        }

        // TAB key logic
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // F1 toggles Debugger UI
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (DebuggerUI != null)
                DebuggerUI.SetActive(!DebuggerUI.activeSelf);
        }
    }

    public void OnNewRun()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (MenuScreen != null)
            MenuScreen.SetActive(false);
    }

    public void OnEnableDebugger()
    {
        if (DebuggerUI != null)
            DebuggerUI.SetActive(true);
    }
}
