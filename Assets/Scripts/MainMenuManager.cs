using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuScene;
    [SerializeField] GameObject loadingScene;

    private void Awake()
    {
        mainMenuScene.SetActive(true);
        loadingScene.SetActive(false);
    }

    public void Play()
    {
        mainMenuScene.SetActive(false);
        loadingScene.SetActive(true);
        SceneManager.LoadScene("Gameplay");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
