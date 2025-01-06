using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class InGame_UI : MonoBehaviour
{

    private bool gamePaused;

    [Header("Menu gameobjects")]
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject endLevelUI;

    [Header("TextComponents")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI currentFruitsAmount;

    [SerializeField] private TextMeshProUGUI endTimerText;
    [SerializeField] private TextMeshProUGUI endBestTimerText;
    [SerializeField] private TextMeshProUGUI endFruitsText;


    private void Start()
    {
        GameManager.instance.levelNumber = SceneManager.GetActiveScene().buildIndex;
        PlayerManager.instance.inGameUI = this;
        Time.timeScale = 1;
        SwitchUI(inGameUI);
    }


    void Update()
    {
        UpdateInGameInfo();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CheckIfNotPaused();
        }
    }

    public void OnLevelFinished()
    {
        endFruitsText.text = "Fruits: " + PlayerManager.instance.fruits;
        endTimerText.text = "Your Time: " + GameManager.instance.timer.ToString("00") + " s";
        endBestTimerText.text = "Best Time: " + PlayerPrefs.GetFloat("Level" + GameManager.instance.levelNumber + "Best Time" , 999).ToString("00") + " s";

        SwitchUI(endLevelUI);
    }

    private bool CheckIfNotPaused()
    {
        if (!gamePaused)
        {
            gamePaused = true;
            Time.timeScale = 0;
            SwitchUI(pauseUI);
            return true;
        }
        else
        {
            gamePaused=false;
            Time.timeScale=1;
            SwitchUI(inGameUI);
            return false;
        }
    }

    public void OnDeath() => SwitchUI(pauseUI);

    private void UpdateInGameInfo()
    {
        timerText.text = "Time: " + GameManager.instance.timer.ToString("00") + " s";
        currentFruitsAmount.text = PlayerManager.instance.fruits.ToString();
    }

    public void SwitchUI(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        uiMenu.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
        

    public void ReloadCurrentLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void LoadNextLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


}
