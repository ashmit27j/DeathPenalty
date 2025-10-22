
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject gameOverCanvas;
    public float timeRemaining = 60f;
    private bool timerActive = false;
    public GameObject victoryCanvas;
    public TextMeshProUGUI timeCompletedInText;

    void Start()
    {
        //disable canvases at start
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }

        if (victoryCanvas != null)
        {
            victoryCanvas.SetActive(false);
        }

        //check for timer text assignment
        if (timerText == null)
        {
            Debug.LogError("Timer Text not assigned!");
        }
    }

    void Update()
    {
        if (timerActive && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else if (timerActive && timeRemaining <= 0)
        {
            timeRemaining = 0;
            UpdateTimerDisplay();
            EndGame();
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StartTimer()
    {
        timerActive = true;
        Debug.Log("Timer has started!");
    }

    public void AddTime(float seconds)
    {
        timeRemaining += seconds;
        Debug.Log("Added " + seconds + " seconds. New time: " + timeRemaining);
    }

    void EndGame()
    {
        timerActive = false;
        Debug.Log("Game Over!");

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Change to your main menu scene name
    }

    public void Victory()
    {
        timerActive = false;

        // Set Victory Screen time
        if (timeCompletedInText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timeCompletedInText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (victoryCanvas != null)
        {
            victoryCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }

}
