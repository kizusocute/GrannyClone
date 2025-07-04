using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerMenuScene : MonoBehaviour
{
    public static UIManagerMenuScene Instance;

    public GameObject backGroundPanel;
    public GameObject optionsPanel;
    public Button playButton;
    public Button optionsButton;
    public Button exitButton;
    public Button backButton;
    public Button easyModeButton;
    public Button hardModeButton;

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
        playButton.onClick.RemoveAllListeners();
        optionsButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
        easyModeButton.onClick.RemoveAllListeners();
        hardModeButton.onClick.RemoveAllListeners();

        backGroundPanel.SetActive(true);
        optionsPanel.SetActive(false);
        playButton.onClick.AddListener(PlayGame);
        optionsButton.onClick.AddListener(ShowOptions);
        exitButton.onClick.AddListener(Application.Quit);
        backButton.onClick.AddListener(ShowMainMenu);
        easyModeButton.onClick.AddListener(SetEasyMode);
        hardModeButton.onClick.AddListener(SetHardMode);
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
