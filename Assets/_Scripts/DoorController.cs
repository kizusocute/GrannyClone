using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    const string DOOR_OPEN_TRIGGER = "Open";

    public KeyCode openDoorKey = KeyCode.R;
    private bool isOpen = false;

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
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= interactionRange)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, interactionRange))
            {
                if (hit.transform == player)
                {
                    isPlayerInRange = true;
                    return;
                }
            }
        }

        isPlayerInRange = false;
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
            if (isOpen) return;
            isOpen = true;
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
