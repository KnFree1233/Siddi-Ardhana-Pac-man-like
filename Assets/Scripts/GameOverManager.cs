using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] TMP_Text conditionText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject winImage;
    [SerializeField] GameObject loseImage;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        conditionText.text = "You " + PlayerPrefs.GetString("Condition");
        scoreText.text = "Score: " + PlayerPrefs.GetInt("Score").ToString();
        if (PlayerPrefs.GetString("Condition") == "Win")
        {
            winImage.SetActive(true);
        }
        else if (PlayerPrefs.GetString("Condition") == "Lose")
        {
            loseImage.SetActive(true);
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
