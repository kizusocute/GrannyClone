using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    const string DOOR_OPEN_TRIGGER = "Open";

    public KeyCode openDoorKey = KeyCode.R;

    Animator doorAnimator;
    public Transform player;
    public float interactionRange = 3f;
    public LayerMask playerLayer;
    private bool isPlayerInRange = false;
    //public GameObject keyObject;
    public string keyLayerName = "";
    
    public AudioClip doorOpenSound;
    AudioSource audioSource;

    void Start()
    {
        doorAnimator = GetComponent<Animator>();
        //keyLayerName = keyObject != null ? keyObject.layer.ToString() : keyLayerName;
        audioSource = player.gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheckPlayerInRange();
        if (isPlayerInRange && Input.GetKeyDown(openDoorKey))
        {
            OpenDoor();
        }
    }

    void CheckPlayerInRange()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, interactionRange, playerLayer))
        {
            if (hit.transform == player)
            {
                isPlayerInRange = true;
                return;
            }
            isPlayerInRange = false;
        }
    }

    void OpenDoor()
    {
        if (doorAnimator != null)
        {
            if(!string.IsNullOrEmpty(keyLayerName))
            {
                bool playerHasKey = false;
                foreach (Transform key in player)
                {
                    if (key.gameObject.layer == LayerMask.NameToLayer(keyLayerName))
                    {
                        playerHasKey = true;
                        break;
                    }
                }
                if(!playerHasKey)
                {
                    Debug.Log("Khong co chia khoa");
                    return;
                }
            }
            doorAnimator.SetTrigger(DOOR_OPEN_TRIGGER);
            PlaySound(doorOpenSound);
        }
    }

    void PlaySound(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}
