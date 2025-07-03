using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerGameScene : MonoBehaviour
{
    public static UIManagerGameScene Instance;
    public GameObject keypadPanel;
    public GameObject keypad;
    public Text dayTexts;

    public Button pickUpButton;
    public Button dropButton;

    public GameObject MobileControlUI;

    public GameObject pauseGameMenu;
    public Button resumeButton;
    public Button MainMenuButton;
    public Button pauseButton;

    

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

    void Start()
    {
        GameManager gameManager = GameManager.Instance;
        dropButton.gameObject.SetActive(false);
        pauseGameMenu.SetActive(false);
        if (GameManager.Instance.isMobile)
        {
            MobileControlUI.SetActive(true);
        }
        else
        {
            MobileControlUI.SetActive(false);
        }
        resumeButton.onClick.RemoveAllListeners();
        MainMenuButton.onClick.RemoveAllListeners();
        pauseButton.onClick.RemoveAllListeners();

        resumeButton.onClick.AddListener(GameManager.Instance.ResumeGame);
        MainMenuButton.onClick.AddListener(GameManager.Instance.BackToMainMenu);
        pauseButton.onClick.AddListener(GameManager.Instance.PauseGame);
    }



    public void UpdateDaysText()
    {
        if (dayTexts != null)
        {
            dayTexts.text = "Days left : " + GameManager.Instance.currentDays;
        }
    }

    public void ShowPauseGameMenu()
    {
        pauseGameMenu.SetActive(true);
        MobileControlUI.SetActive(false);
    }

    public void HidePauseGameMenu()
    {
        pauseGameMenu.SetActive(false);
        if (GameManager.Instance.isMobile)
        {
            MobileControlUI.SetActive(true);
        }
    }




}
