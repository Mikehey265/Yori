using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    [Header("[HighlightManager]")]
    public TileHighlightManager tileHighlightManager;
    

    public void HandleMouseOverTile(Transform tileTransform)
    {
        tileHighlightManager.HighlightTile(tileTransform);
    }
    public void HandleMouseSelectedTile(Transform tileTransform)
    {
        tileHighlightManager.HighlightSelectedTile(tileTransform);
    }
    public void ResetTileHighlight()
    {
        tileHighlightManager.ResetHighlight();
    }

    public void ResetSelectedTileHighlight()
    {
        tileHighlightManager.ResetSelectionHighlight();
    }
}
    

    

