using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public static EnemyMovement Instance { get; private set; }  //added
    
    private Vector3 currentPos;
    
    [SerializeField] private List<GameObject> patrolPoints;
    private List<Vector3> traversedPositions;
    
    [SerializeField] private EnemyVision enemyVision;
    [SerializeField]private SpriteRenderer spriteRenderer;
    
    public GameEventInt playerMoved;
    public GameEventVoid getVision;
    
    private int currentPatrolIndex;
    public int facingDirection;     // 1 = up, 2 = right, 3 = down, 4 = left
    private float moveSpeed = 5;
    private bool move;
    private int traversePositionIndex;
    public int activateInRoom;
    private void OnEnable()
    {
        playerMoved.onEventRaised += MovementListen;
    }

    private void OnDisable()
    {
        playerMoved.onEventRaised -= MovementListen;
    }

    private void Awake()    //added
    {
        Instance = this;
    }

    private void Start()
    {
        enemyVision = enemyVision.GetComponent<EnemyVision>();
        spriteRenderer = spriteRenderer.GetComponent<SpriteRenderer>();
        if (SavingSystem.Instance.IsEnemyPositionSaved() && isActiveAndEnabled)
        {
            currentPatrolIndex = SavingSystem.Instance.GetEnemyCurrentPatrolIndex();
            currentPos.x = SavingSystem.Instance.GetSavedEnemyPosX();
            currentPos.z = SavingSystem.Instance.GetSavedEnemyPosZ();
            transform.position = currentPos;
            CalculateDifferenceBetweenPoints();
        }
        else
        {
            currentPos.x = patrolPoints[currentPatrolIndex].gameObject.transform.position.x;
            currentPos.z = patrolPoints[currentPatrolIndex].gameObject.transform.position.z;
            transform.position = currentPos;
            CalculateDifferenceBetweenPoints();
        }
        getVision.EventRaised();
    }

    private void Update()
    {
        if (move)
        {
            Move();
        }
    }

    private void MovementListen(int playerSteps)
    {
        traversedPositions = new List<Vector3>();
        traversePositionIndex = 0;
        currentPos = SimulateMove(playerSteps);
        move = true;
    }
    
    
    private void CalculateDifferenceBetweenPoints()
    {
        Vector3 fromPos = patrolPoints[currentPatrolIndex].gameObject.transform.position;
        Vector3 toPos = patrolPoints[currentPatrolIndex + 1].gameObject.transform.position;
        Vector3 vectorDiff = fromPos - toPos;
        var distanceInX = Mathf.Abs(vectorDiff.x);
        var distanceInY = Mathf.Abs(vectorDiff.z);
        Debug.Log("difference is: X: " + distanceInX + " Y: " + distanceInY);
    }
    
    
    
    //Each following object in the list needs to have a common value on one of the axes
    private Vector3 SimulateMove(int steps)
    {
        Vector3 simulateMovement = new Vector3();
        Vector3 simulatePosition = currentPos;
        int simulatePatrolIndex = currentPatrolIndex;
        for (int i = 0; i < steps; i++)
        {
            simulateMovement = Vector3.zero;
            if (simulatePosition != patrolPoints[simulatePatrolIndex + 1].gameObject.transform.position)
            {
                //Compare on the X axis
                //Check if patrol point is to the left of position
                if (simulatePosition.x >
                    patrolPoints[simulatePatrolIndex + 1].gameObject.transform.position.x)
                {
                    simulateMovement.x--;
                    facingDirection = 2;
                    spriteRenderer.flipX = true;
                }

                //Check if patrol point is to the right of position
                if (simulatePosition.x <
                    patrolPoints[simulatePatrolIndex + 1].gameObject.transform.position.x)
                {
                    simulateMovement.x++;
                    facingDirection = 4;
                    spriteRenderer.flipX = false;
                }
                //compare on the Y axis
                //Check if patrol point is below position
                if (simulatePosition.z >
                    patrolPoints[simulatePatrolIndex + 1].gameObject.transform.position.z)
                {
                    simulateMovement.z--;
                    facingDirection = 3;
                    spriteRenderer.flipX = false;
                }

                //Check if patrol point is above position
                if (simulatePosition.z <
                    patrolPoints[simulatePatrolIndex + 1].gameObject.transform.position.z)
                {
                    simulateMovement.z++;
                    facingDirection = 1;
                    spriteRenderer.flipX = true;
                }
            }
            //Adds the simulated movement to the simulated position
            simulatePosition += simulateMovement;
            traversedPositions.Add(simulatePosition);
            //Checks if the enemy has arrived at a patrol point after it's latest movement
            if (patrolPoints[simulatePatrolIndex + 1].gameObject.transform.position == simulatePosition)
            {
                simulatePatrolIndex++;
            }
            if (simulatePatrolIndex == patrolPoints.Count - 1)
            {
                simulatePatrolIndex = 0;
            }
        }
        currentPatrolIndex = simulatePatrolIndex;
        return simulatePosition;
    }

    private void Move()
    {
        if (traversePositionIndex == traversedPositions.Count)
        {
            move = false;
            getVision.EventRaised();
            return;
        }
        float step = Time.deltaTime * moveSpeed;
        transform.position = Vector3.MoveTowards(transform.position, traversedPositions[traversePositionIndex], step);
        {
            if (transform.position == traversedPositions[traversePositionIndex])
            {
                traversePositionIndex++;
            }
        }
    }
    

    public Vector3 GetCurrentEnemyPosition()    //added
    {
        return currentPos;
    }

    public int GetCurrentPatrolPoint()  //added
    {
        return currentPatrolIndex;
    }
}
