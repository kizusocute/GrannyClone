using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPickUp : MonoBehaviour
{
    const string PICKUP_ITEM_TAG = "PickUpItem";
    const string Axe_TAG = "Axe";
    public float pickUpRange = 2f;

    public KeyCode pickUpKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.G;

    public Transform itemHoldPosition;
    public Transform AxeHoldPosition;
    public Transform playerBody;
    //public Transform keyHolder;

    private GameObject heldItem;

    private bool isKey = false;
    private bool isAxe = false;

    [Header("Audio")]
    public AudioClip pickUpSound;
    public AudioClip dropSound;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UIManagerGameScene.Instance.pickUpButton.onClick.AddListener(TryPickUpItem);
        UIManagerGameScene.Instance.dropButton.onClick.AddListener(Drop);
    }

    void Update()
    {
        if (Input.GetKeyDown(pickUpKey))
        {
            TryPickUpItem();
        }
        if (Input.GetKeyDown(dropKey))
        {
            Drop();
        }
    }

    void TryPickUpItem()
    {
        if (heldItem != null)
        {
            //DropItem();
            Debug.Log("Da co vat pham tren tay");
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickUpRange);
        
        GameObject closestItem = null;
        float closestDistance = pickUpRange;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(PICKUP_ITEM_TAG))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItem = hitCollider.gameObject;
                }
                isKey = true;
            }
            else if (hitCollider.CompareTag(Axe_TAG))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItem = hitCollider.gameObject;
                }
                isAxe = true;
                isKey = false;
            }
        }
        if (closestItem != null)
        {
            PickUp(closestItem);
        }
    }
    void PickUp(GameObject item)
    {
        heldItem = item;
        heldItem.GetComponent<Collider>().enabled = false;
        if (heldItem.GetComponent<Rigidbody>())
        {
            heldItem.GetComponent<Rigidbody>().isKinematic = true;
        }
        if(isKey)
        {
            heldItem.transform.SetParent(playerBody);
            heldItem.transform.position = itemHoldPosition.position;
            heldItem.transform.rotation = itemHoldPosition.rotation;
        }
        else if (isAxe)
        {
            heldItem.transform.SetParent(playerBody);
            heldItem.transform.position = AxeHoldPosition.position;
            heldItem.transform.rotation = AxeHoldPosition.rotation;
        }
        PlaySound(pickUpSound);
        Debug.Log("Da nhat vat pham: " + item.name);
        UIManagerGameScene.Instance.pickUpButton.gameObject.SetActive(false);
        UIManagerGameScene.Instance.dropButton.gameObject.SetActive(true);
    }

    void Drop()
    {
        if (heldItem == null)
        {
            Debug.Log("Khong co vat pham tren tay");
            return;
        }
        heldItem.GetComponent<Collider>().enabled = true;
        heldItem.transform.position = transform.position + transform.forward; 
        if (heldItem.GetComponent<Rigidbody>())
        {
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
        }
        heldItem.transform.SetParent(null);
        heldItem = null;
        isKey = false;
        PlaySound(dropSound);

        Granny granny = FindObjectOfType<Granny>();
        if(granny != null)
        {
            granny.OnSoundHeard(transform.position);
        }

        Debug.Log("Da tha vat pham");
        UIManagerGameScene.Instance.pickUpButton.gameObject.SetActive(true);
        UIManagerGameScene.Instance.dropButton.gameObject.SetActive(false);
    }

    void PlaySound(AudioClip audioClip)
    {
        if(audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}
