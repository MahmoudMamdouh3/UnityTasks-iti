using UnityEngine;
using UnityEngine.UI; // Required for Image
using TMPro;          // Required for TextMeshPro

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timeElapsed;
    private bool isPaused = false;
    
    // These variables act as "slots" where we can drag our UI objects
    public Image healthBarFill;
    public TextMeshProUGUI scoreText;

    // A variable to store current score
    private int currentScore = 0;

    // This function can be called to change health (value between 0 and 1)
    public void UpdateHealth(float amount)
    {
        // Example: amount 0.5 is 50% health
        healthBarFill.fillAmount = amount; 
    }

    // This function can be called to add points
    public void AddScore(int points)
    {
        currentScore += points;
        scoreText.text = "Score: " + currentScore.ToString();
    }
    
    public void TogglePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1; // Resume game time
            isPaused = false;
        }
        else
        {
            Time.timeScale = 0; // Freeze game time
            isPaused = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime; // Adds the time passed since last frame
    
        // Format it nicely (00:00)
        // F2 means "2 decimal places"
        timerText.text = "Time: " + timeElapsed.ToString("F2");
    }
    
}