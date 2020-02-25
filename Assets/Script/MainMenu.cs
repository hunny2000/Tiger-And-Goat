using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject MainScreenPanel;

    [SerializeField] private GameObject PlayButton;

    [SerializeField] private GameObject ModesPanel;
    [SerializeField] private GameObject SingleData;

    [SerializeField] private InputField TigerName;
    [SerializeField] private InputField GoatName;

    [SerializeField] private GameObject MultiplayerData;

    private void Awake()
    {
        TigerName.text = "Tiger";
        GoatName.text = "Goat";
    }

    public void Pause()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        MainScreenPanel.SetActive(false);
    }
    public void Resume()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        MainScreenPanel.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }


    public void Play()
    {
        PlayButton.SetActive(false);
        ModesPanel.SetActive(true);
    }

    //SINGLE PLAYER
    public void SinglePlayer()
    {
        SingleData.SetActive(true);
        ModesPanel.SetActive(false);

        PlayerPrefs.SetString("Mode", "Offline");
        PlayerPrefs.SetString("TigerName", TigerName.text);
        PlayerPrefs.SetString("GoatName", GoatName.text);
    }
    public void SinglePlay()
    {
        SceneManager.LoadScene(sceneBuildIndex: 2);
    }


    //MULTIPLAYER
    public void Multiplayer()
    {
        PlayerPrefs.SetString("Mode", "Online");
        MultiplayerData.SetActive(true);
        ModesPanel.SetActive(false);
    }
    public void TigerPlayer()
    {
        PlayerPrefs.SetString("Choice", "Tiger");
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }
    public void GoatPlayer()
    {
        PlayerPrefs.SetString("Choice", "Goat");
        SceneManager.LoadScene(sceneBuildIndex: 1);
    }
}
