using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement instance;

    public GameObject pausePanel;
    public GameObject tutorialPanel;
    public GameObject mainPanel;
    private int killcount;
    public TextMeshProUGUI killCounter;
    //public GameObject linePosition;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Pause();
    }
    public void KillCount(int count)
    {
        killcount += count;
        killCounter.text = "" + killcount;
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    public void Play()
    {
        SceneManager.LoadScene("Level 1");
        Time.timeScale = 1;
    }
    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void Resume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void HowToPlay()
    {
        mainPanel.SetActive(false);
        tutorialPanel.SetActive(true);
    }
    public void Back()
    {
        mainPanel.SetActive(true);
        tutorialPanel.SetActive(false); ;
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
