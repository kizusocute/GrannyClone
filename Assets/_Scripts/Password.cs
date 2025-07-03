using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassWord : MonoBehaviour
{
    public Button[] digitButtons;
    public Button goButton;
    public Button noButton;

    public Light keypadLight;
    public float activationDistance = 1f;
    const string PASSWORD = "0741";
    private string inputPassword = "";
    private int guessCount = 0;
    private int maxGuesses = 3;

    private bool doorUnlock = false;
    private Transform playerTransform;

    public AudioClip buttonPressSound;
    public AudioClip incorrectPasswordSound;
    public AudioClip doorUnlockSound;

    AudioSource audioSource;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < digitButtons.Length; i++)
        {
            int digit = i;
            digitButtons[i].onClick.RemoveAllListeners();
            digitButtons[i].onClick.AddListener(() => OnDigitButtonClicked(digit.ToString()));
        }
        goButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        goButton.onClick.AddListener(OnGoButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
        UIManagerGameScene.Instance.keypadPanel.SetActive(false);

        audioSource = playerTransform.gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Vector3.Distance(playerTransform.position, UIManagerGameScene.Instance.keypad.transform.position) > activationDistance)
        {
            UIManagerGameScene.Instance.keypadPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnActivateKeypadButtonClicked();
        }
    }

    void OnActivateKeypadButtonClicked()
    {
        if (Vector3.Distance(playerTransform.position, UIManagerGameScene.Instance.keypad.transform.position) <= activationDistance)
        {
            UIManagerGameScene.Instance.keypadPanel.SetActive(true);
            SetButtonInteractable(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void OnDigitButtonClicked(string digit)
    {
        if (inputPassword.Length < 4 && guessCount <= maxGuesses)
        {
            inputPassword += digit;
            Debug.Log("Current Input: " + inputPassword);
        }
        PlaySound(buttonPressSound);
    }

    void OnGoButtonClicked()
    {
        if(guessCount < maxGuesses)
        {
            if (inputPassword == PASSWORD)
            {
                doorUnlock = true;
                keypadLight.color = Color.green;
                UIManagerGameScene.Instance.keypadPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                PlaySound(doorUnlockSound);
                Debug.Log("Cua da duoc mo");
            }
            else
            {
                guessCount++;
                PlaySound(incorrectPasswordSound);
                Debug.Log("Sai mat khau, vui long nhap lai");
                inputPassword = "";
                if (guessCount >= maxGuesses)
                {
                    //UIManager.Instance.keypadPanel.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    guessCount = 0; 
                    SetButtonInteractable(false);
                }
            }
        }
    }

    void OnNoButtonClicked()
    {
        inputPassword = "";
        guessCount = 0; 
        SetButtonInteractable(true);
    }

    void SetButtonInteractable(bool interactable)
    {
        foreach (Button button in digitButtons)
        {
            button.interactable = interactable;
        }
        goButton.interactable = interactable;
        noButton.interactable = interactable;
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
