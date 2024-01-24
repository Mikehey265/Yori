using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlightManager : MonoBehaviour
{
    [Header("[Highlight Materials]")]
    public Material highlightMaterial;
    public Material originalMaterial;
    public Material selectionMaterial;
    
    
    
    private Transform currentlyHighlighted = null;
    private Transform currentlySelected = null;

    private Color originalColor;
    private Color highlightColor;
    private Color selectionColor;

    private void Start()
    {
        originalColor = originalMaterial.color;
        highlightColor = new Color(0, 0, 1, 1);
        selectionColor = new Color(1, 0, 0.4f, 1);
    }

    //then use in MovePlayer class
    public void HighlightTile(Transform tileTransform)
    {
        if (!IsValidDesiredTileValid(MovePlayer.Instance.GetCurrentPlayerPosition(), tileTransform.position))
        {
            return;
        }
        
        // Reset previous highlight if any
        if (currentlyHighlighted)
        {
            currentlyHighlighted.GetComponentInChildren<MeshRenderer>().material = originalMaterial;
        }
        
        if (tileTransform.CompareTag("Tile"))
        {
            //get out cube from the game object
            Transform cubeComponent = tileTransform.Find("Cube");
            if (cubeComponent)
            {
                currentlyHighlighted = cubeComponent;
                currentlyHighlighted.GetComponent<MeshRenderer>().material = highlightMaterial;
            }
        }
        else
        {
            currentlyHighlighted = null;
        }
    }
    //then use in MovePlayer class when we move (on click event from MovePlayer class)
    public void HighlightSelectedTile(Transform tileTransform)
    {
        if (currentlySelected)
        {
            currentlySelected.GetComponentInChildren<MeshRenderer>().material = originalMaterial;
        }
        if (tileTransform.CompareTag("Tile"))
        {
            //get cube from our game object
            Transform cubeComponent = tileTransform.Find("Cube");
            if (cubeComponent)
            {
                currentlySelected = cubeComponent;
                currentlySelected.GetComponent<MeshRenderer>().material = selectionMaterial;
            }
        }
        else
        {
            currentlySelected = null;
        }
    }

    public void ResetSelectionHighlight()
    {
        if (currentlySelected)
        {
            currentlyHighlighted.GetComponentInChildren<MeshRenderer>().material = originalMaterial;
        }
    }
    public void ResetHighlight()
    {
        if (currentlyHighlighted)
        {
            currentlyHighlighted.GetComponent<MeshRenderer>().material = originalMaterial;
        }
    }

    public bool IsValidDesiredTileValid(Vector3 playerPosition, Vector3 tilePosition)
    {
        Vector3 difference = tilePosition - playerPosition;
        float distance = Vector3.Distance(playerPosition, tilePosition);
        
        if (distance <= 4)
        {
            if (Mathf.Abs(difference.x) > 0.5f && Mathf.Abs(difference.z) < 0.5f)
            {
                // Left or Right
                return true;
            }
            else if (Mathf.Abs(difference.x) < 0.5f && Mathf.Abs(difference.z) > 0.5f) 
            {
                // Forward or Backward
                return true;
            }
              
        }

        return false;
    }
}
