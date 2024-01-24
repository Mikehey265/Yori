using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    public static MovePlayer Instance { get; private set; }
    
    [SerializeField] private float maxRange = 3f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameEventInt playerMoved;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioClip footsteps;
    
    private RaycastHit hit;
    private Camera cameraVar;
    
    private Vector3 currentPosition;
    private Vector3 clickedPosition;
    private Vector3 previousPosition;
    private int stepsTaken;
    private int clicksAmount;
    private bool isMoving;
    private bool moveEventFired = false;

    private void Awake()
    {
        Instance = this;
        cameraVar = Camera.main;
    }

    private void Start()
    {
        if (SavingSystem.Instance.ArePlayerStatsSaved())
        {
            transform.position = new Vector3(SavingSystem.Instance.GetSavedPlayerPosX(), 1, SavingSystem.Instance.GetSavedPlayerPosZ());
            clicksAmount = SavingSystem.Instance.GetSavedPlayerClicks();
            InGameUI.Instance.UpdateStepsCounterText(clicksAmount);
            currentPosition = transform.position;
        } 
        else
        {
            currentPosition = transform.position;
            InGameUI.Instance.UpdateStepsCounterText(clicksAmount);
            clicksAmount = 0;
        }
    }

    private void Update()
    {
        if (InGameUI.Instance.GetIsGameLoading() || GameManager.Instance.ReturnMiniGameActive()) return;
        Mouse mouse = Mouse.current;
        Vector3 mousePosition = mouse.position.ReadValue();
        Ray ray = cameraVar.ScreenPointToRay(mousePosition);
        if (!isMoving && !InGameUI.Instance.GetIsGamePaused())
        {
            gridManager.ResetTileHighlight();
            if (mouse.leftButton.wasPressedThisFrame)
            {
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    clickedPosition = new Vector3(hit.transform.position.x, 1, hit.transform.position.z);
                    CheckPosition();
                }
            }   
            if(!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit))
            {
                //highlight the tile when just hovering
                gridManager.HandleMouseOverTile(hit.transform);
            }
        }
        
        if (isMoving)
        {
            Move();
            gridManager.HandleMouseSelectedTile(hit.transform);     //highlight the tile when clicked
        }
    }
    private bool IsObstacleBetweenPlayerAndDesiredLocation(Vector3 playerLocation, Vector3 desiredLocation)
    {
        // Adjusting x y to see object to check object on the 
        playerLocation.y += 0.3f;
        desiredLocation.y += 0.3f;

        if (Physics.Linecast(playerLocation, desiredLocation, out RaycastHit hit))
        {
            if(hit.collider.CompareTag("Obstacle"))
            {
                return true;
            }
        }
        return false;
    }
    private void CheckPosition()
    {
        float distance = Vector3.Distance(currentPosition, clickedPosition);
        Vector3 position = clickedPosition - currentPosition;
        if (IsObstacleBetweenPlayerAndDesiredLocation(currentPosition, clickedPosition))
        {
            // Detected an obstacle, return from the function
            Debug.Log("OBSTACLE");
            return;
        }
        if (distance <= maxRange)
        {
            
            if (clickedPosition == previousPosition && clickedPosition != currentPosition)
            {
                StressManager.Instance.AddStressPoints(1);
            }
            
            //if x is greater than z, and both z coordinates are the same, player can move on x axis
            if (Mathf.Abs(position.x) > Mathf.Abs(position.z) && (int)currentPosition.z == (int)clickedPosition.z)
            {
                if (transform.position.x < clickedPosition.x)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
                stepsTaken = (int)distance;
                clicksAmount++;
                InGameUI.Instance.UpdateStepsCounterText(clicksAmount);
                moveEventFired = false;
                isMoving = true;
                previousPosition = currentPosition;
            }
            //if z is greater than x, and both x coordinates are the same, player can move on z axis
            else if(Mathf.Abs(position.x) < Mathf.Abs(position.z) && (int)currentPosition.x == (int)clickedPosition.x)
            {
                if (transform.position.z > clickedPosition.z)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
                stepsTaken = (int)distance;
                clicksAmount++;
                InGameUI.Instance.UpdateStepsCounterText(clicksAmount);
                moveEventFired = false;
                isMoving = true;
                previousPosition = currentPosition;
            }
        }
        else
        {
            stepsTaken = 0;
            isMoving = false;
        }
    }

    //Move towards clicked position
    private void Move()
    {
        float step = Time.deltaTime * moveSpeed;
        transform.position = Vector3.MoveTowards(transform.position, clickedPosition, step);
        // transform.LookAt(clickedPosition);
        animator.SetBool("IsMoving", true);
        
        

        if (!moveEventFired)
        {
            playerMoved.EventRaised(stepsTaken);
            moveEventFired = true;
        }
        
        //player arrived at clicked position
        if (transform.position == clickedPosition)
        {
            if (footsteps != null)
            {
                AudioSource.PlayClipAtPoint(footsteps, transform.position);
            }
            currentPosition = transform.position;
            gridManager.ResetSelectedTileHighlight();
            stepsTaken = 0;
            isMoving = false;
            animator.SetBool("IsMoving", false);
        }
    }

    //function performed in TileDetector script. When player detects a wall, he stops on last detected tile.
    //when player click on tile behind the wall it should not count as a move for player and enemy
    public void StopMoving(Vector3 newPosition)
    {
        if (isMoving)
        {
            isMoving = false;
            stepsTaken = 0;
            transform.position = newPosition;
            currentPosition = transform.position;
            clickedPosition = currentPosition;
            animator.SetBool("IsMoving", false);
        }
    }

    public Vector3 GetCurrentPlayerPosition()
    {
        return currentPosition;
    }

    public void SetCurrentPlayerPosition(Vector3 newPosition)
    {
        isMoving = false;
        transform.position = newPosition;
        currentPosition = transform.position;
        clickedPosition = currentPosition;
        animator.SetBool("IsMoving", false);
    }
    
    public void SetCurrentPlayerPosition(float positionX, float positionZ)
    {
        isMoving = false;
        transform.position = new Vector3(positionX, 1, positionZ);
        currentPosition = transform.position;
        clickedPosition = currentPosition;
        animator.SetBool("IsMoving", false);
    }

    public bool GetIsPlayerMoving()
    {
        return isMoving;
    }

    public int GetClicksAmount()
    {
        return clicksAmount;
    }
}
