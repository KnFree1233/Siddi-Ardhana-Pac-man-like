using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] TMP_Text conditionText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] AudioSource winAudio;
    [SerializeField] AudioSource loseAudio;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        conditionText.text = "You " + PlayerPrefs.GetString("Condition");
        scoreText.text = "Score: " + PlayerPrefs.GetInt("Score").ToString();
        if (PlayerPrefs.GetString("Condition") == "Win")
        {
            winAudio.Play();
        }
        else if (PlayerPrefs.GetString("Condition") == "Lose")
        {
            loseAudio.Play();
        }
    }

    public void Retry()
    {
        ResetPlayerScore();
        SceneManager.LoadScene("Gameplay");
    }

    public void MainMenu()
    {
        ResetPlayerScore();
        SceneManager.LoadScene("Main Menu");
    }

    private void ResetPlayerScore()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
