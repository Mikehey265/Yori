using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ObtainableItem item;
    public GameEventInt addPlayerMovedScriptableObject;
    [SerializeField] private AudioClip pickupSound;

    private bool isObtainable = false;
    [SerializeField] private TMP_Text uiText;
    private void Start()
    {
        item.obj = gameObject;
        isObtainable = false;
        item.isObtained = false;
        uiText.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isObtainable)
        {
            if (GameManager.Instance.ReturnMiniGameActive()) return;
            InventoryManager.Instance.AddItem(PickedUp());
            addPlayerMovedScriptableObject.EventRaised(1);
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isObtainable = true;
            uiText.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isObtainable = false;
            uiText.enabled = false;
        }
    }

    public ObtainableItem PickedUp()
    {
        Destroy(gameObject);
        return item;
    }
}
