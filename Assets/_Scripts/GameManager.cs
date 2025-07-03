using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //public bool isEasy;
    public bool isMobile;

    public int maxDays = 3;
    public int currentDays;
    public bool allLocksOpen;

    public GameObject endGameCutSence;

    private bool isPaused = false;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnDisable()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentDays = maxDays;
        UIManagerGameScene.Instance.UpdateDaysText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        if (allLocksOpen)
        {
            StartCoroutine(EndGameCutSence());
        }
    }
    void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
        //Debug.Log("Game Over");
    }

    public void DecreaseDays()
    {
        currentDays--;
        UIManagerGameScene.Instance.UpdateDaysText();
        if (currentDays <= 0)
        {
            EndGame();
        }
    }

    IEnumerator EndGameCutSence()
    {
        yield return new WaitForSeconds(1f);
        endGameCutSence.SetActive(true);
        yield return new WaitForSeconds(7.5f);
        EndGame();
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        isPaused = true;
        UIManagerGameScene.Instance.ShowPauseGameMenu();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        UIManagerGameScene.Instance.HidePauseGameMenu();
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }


}
