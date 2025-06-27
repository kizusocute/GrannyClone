using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public Camera playerCamera;
    public GameObject player;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
            {
                Wood wood = hit.collider.GetComponent<Wood>();
                if (wood != null)
                {
                    if (isHoldingAxe())
                    {
                        wood.DownWood();
                    }
                    else
                    {
                        Debug.Log("Ban k co riu");
                    }
                }
            }
        }
    }

    bool isHoldingAxe()
    {
        foreach (Transform child in player.transform)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer("Axe"))
            {
                return true;
            }
        }
        return false;
    }
}
