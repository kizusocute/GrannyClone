using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    //public bool isEasy;
    public bool isMobile;

    public int maxDays = 3;
    public int currentDays;
    public bool allLocksOpen;

    public GameObject endGameCutSence;

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
        UIManager.Instance.UpdateDaysText();
    }

    // Update is called once per frame
    void Update()
    {
        if (allLocksOpen)
        {
            StartCoroutine(EndGameCutSence());
        }
    }
    void EndGame()
    {
        // Implement end game logic here
        Debug.Log("Game Over");
    }

    public void DecreaseDays()
    {
        currentDays--;
        UIManager.Instance.UpdateDaysText();
        if (currentDays <= 0)
        {
            EndGame();
        }
    }

    IEnumerator EndGameCutSence()
    {
        yield return new WaitForSeconds(1f);
        endGameCutSence.SetActive(true);
    }

    
}
