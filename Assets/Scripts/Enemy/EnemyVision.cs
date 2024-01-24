using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public static EnemyVision Instance { get; private set; }

    //Debug materials
    [SerializeField] private Material visionScore1;
    [SerializeField] private Material visionScore2;
    [SerializeField] private Material outOfVisionRange;
    
    public GameEventVoid getVisionTiles;
    public GameEventVoid setVisionTilesBehaviour;
    
    public EnemyMovement enemyMovement;
    private TileReferences tileReferences;
    
    public Transform enemyPos;
    private bool visionNeedsUpdate;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        tileReferences = TileReferences.Instance;
    }

    private void LateUpdate()
    {
        if (visionNeedsUpdate)
        {
            IterateTilesList();
            setVisionTilesBehaviour.EventRaised();
            visionNeedsUpdate = false;
        }
    }

    private void OnEnable()
    {
        getVisionTiles.onEventRaised += GetVisionTiles;
    }

    private void OnDisable()
    {
        getVisionTiles.onEventRaised -= GetVisionTiles;
    }
    
    private void GetVisionTiles()
    {
        visionNeedsUpdate = true;
    }

    private void IterateTilesList()
    {
        foreach (GameObject obj in tileReferences.tileReferencesArray)
        {
            UpdateVisionOnStep(obj);
        }

        foreach (GameObject obj in tileReferences.tileReferencesArray)
        {
            SetStressBehaviourOnTiles(obj);
        }
    }
    

    #region Getting tiles to mark vision score on
    private bool GetRightTile(GameObject obj)
    {
        if (obj.transform.position.x == transform.position.x + 1 &&
            obj.transform.position.z == transform.position.z)
            return true;
        return false;
    }
    private bool GetAboveTile(GameObject obj)
    {
        if (obj.transform.position.z == transform.position.z + 1 &&
            obj.transform.position.x == transform.position.x)
            return true;
        return false;
    }
    private bool GetLeftTile(GameObject obj)
    {
        if (obj.transform.position.x == transform.position.x - 1 &&
            obj.transform.position.z == transform.position.z)
            return true;
        return false;
    }
    private bool GetBelowTile(GameObject obj)
    {
        if (obj.transform.position.z == transform.position.z - 1 &&
            obj.transform.position.x == transform.position.x)
            return true;
        return false;
    }
    private bool GetTopLeftTile(GameObject obj)
    {
        if (obj.transform.position.x == transform.position.x - 1 && 
            obj.transform.position.z == transform.position.z + 1)
            return true;
        return false;
    }
    private bool GetTopRightTile(GameObject obj)
    {
        if (obj.transform.position.x == transform.position.x + 1 && 
            obj.transform.position.z == transform.position.z + 1)
            return true;
        return false;
    }
    private bool GetBottomLeftTile(GameObject obj)
    {
        if (obj.transform.position.x == transform.position.x - 1 &&
            obj.transform.position.z == transform.position.z - 1)
            return true;
        return false;
    }
    private bool GetBottomRightTile(GameObject obj)
    {
        if (obj.transform.position.x == transform.position.x + 1 &&
            obj.transform.position.z == transform.position.z - 1)
            return true;
        return false;
    }

    private bool GetFacingUpTiles(GameObject obj)
    {
        if (enemyMovement.facingDirection == 1)
        {
            if (obj.transform.position.z == transform.position.z + 2 &&
                obj.transform.position.x == transform.position.x)
            {
                return true;
            }
            if (obj.transform.position.z == transform.position.z + 3 &&
                obj.transform.position.x == transform.position.x)
            {
                return true;
            }
        }
        return false;
    }
    private bool GetFacingRightTiles(GameObject obj)
    {
        if (enemyMovement.facingDirection == 2)
        {
            if (obj.transform.position.x == transform.position.x - 2 &&
                obj.transform.position.z == transform.position.z)
            {
                return true;
            }
            if (obj.transform.position.x == transform.position.x - 3 &&
                obj.transform.position.z == transform.position.z)
            {
                return true;
            }
        }
        return false;
    }
    private bool GetFacingDownTiles(GameObject obj)
    {
        if (enemyMovement.facingDirection == 3)
        {
            if (obj.transform.position.z == transform.position.z - 2 &&
                obj.transform.position.x == transform.position.x)
            {
                return true;
            }

            if (obj.transform.position.z == transform.position.z - 3 &&
                obj.transform.position.x == transform.position.x)
            {
                return true;
            }
        }
        return false;
    }
    private bool GetFacingLeftTiles(GameObject obj)
    {
        if (enemyMovement.facingDirection == 4)
        {
            if (obj.transform.position.x == transform.position.x + 2 &&
                obj.transform.position.z == transform.position.z)
            {
                return true;
            }

            if (obj.transform.position.x == transform.position.x + 3 &&
                obj.transform.position.z == transform.position.z)
            {
                return true;
            }
        }
        return false;
    }

    private bool GetStandingTile(GameObject obj)
    {
        if (obj.transform.position.x == transform.position.x &&
            obj.transform.position.z == transform.position.z)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Setting Stress behaviour on marked tiles
    private void SetStressBehaviourOnTiles(GameObject obj)
    {
        MeshRenderer meshRenderer = obj.GetComponentInChildren<MeshRenderer>();
        StressBehaviour stressBehaviour = obj.GetComponentInChildren<StressBehaviour>();
        
        SetVisionScoreOneTiles(obj, meshRenderer, stressBehaviour);
        SetVisionScoreTwoTiles(obj, meshRenderer,stressBehaviour);
        SetVisionScoreExtendedTiles(obj, meshRenderer, stressBehaviour);
        SetVisionScoreStandingTile(obj, meshRenderer, stressBehaviour);
    }
    private void SetVisionScoreOneTiles(GameObject obj, MeshRenderer meshRenderer, StressBehaviour stressBehaviour)
    {
        if (GetTopLeftTile(obj))
        {
            //meshRenderer.material = visionScore1;
            stressBehaviour.stressAmount = 1;
            stressBehaviour.isMarked = true;
        }
        if (GetTopRightTile(obj))
        {
           //meshRenderer.material = visionScore1;
            stressBehaviour.stressAmount = 1;
            stressBehaviour.isMarked = true;
        }
        if (GetBottomLeftTile(obj))
        {
            //meshRenderer.material = visionScore1;
            stressBehaviour.stressAmount = 1;
            stressBehaviour.isMarked = true;
        }
        if (GetBottomRightTile(obj))
        {
           //meshRenderer.material = visionScore1;
            stressBehaviour.stressAmount = 1;
            stressBehaviour.isMarked = true;
        }
    }
    private void SetVisionScoreTwoTiles(GameObject obj, MeshRenderer meshRenderer, StressBehaviour stressBehaviour)
    {
        if (GetRightTile(obj))
        {
            //meshRenderer.material = visionScore2;
            stressBehaviour.stressAmount = 2;
            stressBehaviour.isMarked = true;
        }
        if (GetLeftTile(obj))
        {
            //meshRenderer.material = visionScore2;
            stressBehaviour.stressAmount = 2;
            stressBehaviour.isMarked = true;

        }
        if (GetAboveTile(obj))
        {
           //meshRenderer.material = visionScore2;
            stressBehaviour.stressAmount = 2;
            stressBehaviour.isMarked = true;

        }
        if (GetBelowTile(obj))
        {
            //meshRenderer.material = visionScore2;
            stressBehaviour.stressAmount = 2;
            stressBehaviour.isMarked = true;
        }
    }
    private void SetVisionScoreExtendedTiles(GameObject obj, MeshRenderer meshRenderer, StressBehaviour stressBehaviour)
    {
        if (GetFacingUpTiles(obj))
        {
            //meshRenderer.material = visionScore1;
            stressBehaviour.stressAmount = 1;
            stressBehaviour.isMarked = true;
        }
        if (GetFacingLeftTiles(obj))
        {
            //meshRenderer.material = visionScore1;
            stressBehaviour.stressAmount = 1;
            stressBehaviour.isMarked = true;
        }
        if (GetFacingDownTiles(obj))
        {
            //meshRenderer.material = visionScore1;
            stressBehaviour.stressAmount = 1;
            stressBehaviour.isMarked = true;
        }
        if (GetFacingRightTiles(obj))
        {
            //meshRenderer.material = visionScore1;
            stressBehaviour.stressAmount = 1;
            stressBehaviour.isMarked = true;
        }
    }

    private void SetVisionScoreStandingTile(GameObject obj, MeshRenderer meshRenderer, StressBehaviour stressBehaviour)
    {
        if (GetStandingTile(obj))
        {
            //meshRenderer.material = visionScore2;
            stressBehaviour.stressAmount = 5;
            stressBehaviour.isMarked = true;
        }
    }
    private void UpdateVisionOnStep(GameObject obj)
    {
        //obj.GetComponentInChildren<MeshRenderer>().material = outOfVisionRange;
        obj.GetComponentInChildren<StressBehaviour>().stressAmount = 0;
        obj.GetComponentInChildren<StressBehaviour>().isMarked = false;
    }
    #endregion

}
