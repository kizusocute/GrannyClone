using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isEasy;
    public bool isMobile;
    public int maxDay = 3;
    int currentDay;
    public bool allocksOpen;

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
        currentDay = maxDay;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
