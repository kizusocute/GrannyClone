using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    private float fallForce = 2f;
    private bool isFalling = false;
    private static int fallenWoodCount = 0;

    void Update()
    {
        if (isFalling)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.AddForce(transform.forward * fallForce, ForceMode.Impulse);
            isFalling = false;
            fallenWoodCount++;
            if(fallenWoodCount >= 2)
            {
                Debug.Log("...");
                GameManager.Instance.allLocksOpen = true;
            }
        }
    }

    public void DownWood()
    {
        isFalling = true;
    }
}
