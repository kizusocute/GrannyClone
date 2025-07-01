using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject keypadPanel;
    public GameObject keypad;
    public Text dayTexts;

    public GameObject backGroundPanel;
    public GameObject optionsPanel;
    public Button playButton;
    public Button optionsButton;
    public Button exitButton;
    public Button backButton;
    public Button easyModeButton;
    public Button hardModeButton;

    public Button pickUpButton;
    public Button dropButton;

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
        backGroundPanel.SetActive(true);
        optionsPanel.SetActive(false);
        playButton.onClick.AddListener(PlayGame);
        optionsButton.onClick.AddListener(ShowOptions);
        exitButton.onClick.AddListener(Application.Quit);
        backButton.onClick.AddListener(ShowMainMenu);
        easyModeButton.onClick.AddListener(SetEasyMode);
        hardModeButton.onClick.AddListener(SetHardMode);
        dropButton.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }


    public void UpdateDaysText()
    {
        if (dayTexts != null)
        {
            dayTexts.text = "Days left : " + GameManager.Instance.currentDays;
        }
    }

    void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    void ShowOptions()
    {
        backGroundPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    void ShowMainMenu()
    {
        backGroundPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
    void SetEasyMode()
    {
        bool isEasy = true;
        PlayerPrefs.SetInt("isEasy", isEasy ? 1 : 0);
        PlayerPrefs.Save();
        easyModeButton.gameObject.SetActive(false);
        hardModeButton.gameObject.SetActive(true);
    }

    void SetHardMode()
    {
        bool isEasy = false;
        PlayerPrefs.SetInt("isEasy", isEasy ? 1 : 0);
        PlayerPrefs.Save();
        easyModeButton.gameObject.SetActive(true);
        hardModeButton.gameObject.SetActive(false);

    }
}
