using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("UI Screens")]
    public GameObject MenuScreen;     // Main menu at game start
    public GameObject PauseScreen;    // Pause menu on Esc after game starts
    public GameObject DebuggerUI;     // Debugger overlay (toggle F1)
    //public GameObject AboutScreen;    // About info panel

    [Header("Game State")]
    public bool gameStarted = false;

    void Start()
    {
        bool restarted = PlayerPrefs.GetInt("RestartRun", 0) == 1;
        PlayerPrefs.SetInt("RestartRun", 0);

        if (!restarted)
        {
            if (MenuScreen != null) MenuScreen.SetActive(true);
            if (PauseScreen != null) PauseScreen.SetActive(false);
            if (DebuggerUI != null) DebuggerUI.SetActive(false);
            //if (AboutScreen != null) AboutScreen.SetActive(false);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0f;
            gameStarted = false;
        }
        else
        {
            if (MenuScreen != null) MenuScreen.SetActive(false);
            if (PauseScreen != null) PauseScreen.SetActive(false);
            if (DebuggerUI != null) DebuggerUI.SetActive(false);
            //if (AboutScreen != null) AboutScreen.SetActive(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1f;
            gameStarted = true;
        }
    }

    public void RestartRun()
    {
        // Optional: Set a flag before reload so Start() knows you're doing a full restart, not a main menu return
        PlayerPrefs.SetInt("RestartRun", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        // SPACE: Start game if menu is up
        if (!gameStarted && MenuScreen != null && MenuScreen.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        // ESC: Show/hide pause if game is running
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameStarted && PauseScreen != null)
            {
                bool pausing = !PauseScreen.activeSelf;
                PauseScreen.SetActive(pausing);

                // Pause/unpause game logic
                Time.timeScale = pausing ? 0f : 1f;
                Cursor.visible = pausing;
                Cursor.lockState = pausing ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }

        // F1: Toggle debugger UI
        if (Input.GetKeyDown(KeyCode.F1) && DebuggerUI != null)
        {
            DebuggerUI.SetActive(!DebuggerUI.activeSelf);
        }
    }

    // Called by "New Run" button or SPACE
    public void StartGame()
    {
        gameStarted = true;
        if (MenuScreen != null) MenuScreen.SetActive(false);
        if (PauseScreen != null) PauseScreen.SetActive(false);
        //if (AboutScreen != null) AboutScreen.SetActive(false);

        // Resume game logic
        Time.timeScale = 1f;

        // Lock and hide cursor for gameplay
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Called by "About the Game" button
    //public void ShowAboutScreen()
    //{
    //    if (AboutScreen != null)
    //        AboutScreen.SetActive(true);
    //}

    // About screen "Back" button
    //public void HideAboutScreen()
    //{
    //    if (AboutScreen != null)
    //        AboutScreen.SetActive(false);
    //}

    // Called by "Resume" button in PauseScreen
    public void ResumeGame()
    {
        if (PauseScreen != null)
            PauseScreen.SetActive(false);

        // Resume game logic
        Time.timeScale = 1f;

        // Hide cursor for gameplay
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Called by "Main Menu" button in PauseScreen
    public void ReturnToMenu()
    {
        gameStarted = false;
        if (MenuScreen != null) MenuScreen.SetActive(true);
        if (PauseScreen != null) PauseScreen.SetActive(false);

        // Pause game
        Time.timeScale = 0f;

        // Show/unlock cursor for menu
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
