using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypadPassword : MonoBehaviour
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

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < digitButtons.Length; i++)
        {
            int digit = i; 
            digitButtons[i].onClick.AddListener(() => OnDigitButtonClicked(digit.ToString()));
        }
        goButton.onClick.AddListener(OnGoButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
        UIManager.Instance.keypadPanel.SetActive(false);
    }

    void Update()
    {
        if (Vector3.Distance(playerTransform.position, UIManager.Instance.keypad.transform.position) > activationDistance)
        {
            UIManager.Instance.keypadPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            OnActivateKeypadButtonClicked();
        }
    }

    void OnActivateKeypadButtonClicked()
    {
        if (Vector3.Distance(playerTransform.position, UIManager.Instance.keypad.transform.position) <= activationDistance)
        {
            UIManager.Instance.keypadPanel.SetActive(true);
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
    }

    void OnGoButtonClicked()
    {
        if(guessCount < maxGuesses)
        {
            if (inputPassword == PASSWORD)
            {
                doorUnlock = true;
                keypadLight.color = Color.green;
                UIManager.Instance.keypadPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Debug.Log("Cua da duoc mo");
            }
            else
            {
                guessCount++;
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
}
