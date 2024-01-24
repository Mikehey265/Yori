using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceStressGameScript : MonoBehaviour
{
    [Header("[UI mini-game data]")]
    public Transform bar; // Drag your Bar UI element here
    public Transform greenZone;
    public Transform point; // Drag your Point UI element here
    
    [Header("[Speed of the point]")]
    public float speed = 600f; // Speed of point movement

    private float currentSpeed = 0f;
    private bool isPlayerWin = false;
    private RectTransform rectTransform;
    private RectTransform barRectTransform;
    private bool isGameActive = true;
    private bool movingRight = true;
    [SerializeField] private GameEventInt enemyMove;
    [SerializeField] private AudioSource successSound;
    [SerializeField] private AudioSource failSound;
    

    void Awake()
    {
        rectTransform = point.GetComponent<RectTransform>();
        barRectTransform = bar.GetComponent<RectTransform>();
        
        //set point in the left side of bar
        float halfWidthOfPoint = rectTransform.rect.width / 2;
        float halfWidthOfBar = barRectTransform.rect.width / 2;
        float startingXPos = -halfWidthOfBar + halfWidthOfPoint;
        point.localPosition = new Vector3(startingXPos, point.localPosition.y, point.localPosition.z);
    }
    

    void Update()
    {
        if (isGameActive)
        {
            MovePoint();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGameActive = false;
                isPlayerWin = CheckResult();
                enemyMove.EventRaised(1);
                currentSpeed = speed;
            }
        }
    }

    public bool GetIsGameActive()
    {
        return isGameActive;
    }
    void MovePoint()
    {
        float step = currentSpeed * Time.deltaTime * (movingRight ? 1 : -1);
        float newPosX = point.localPosition.x + step;
        float halfWidth = rectTransform.rect.width / 2;

        // Calculate min and max X for point according to bar borders
        float minX = -barRectTransform.rect.width / 2 + halfWidth;
        float maxX = barRectTransform.rect.width / 2 - halfWidth;

        point.localPosition = new Vector3(Mathf.Clamp(newPosX, minX, maxX), point.localPosition.y, point.localPosition.z);

        if (point.localPosition.x == maxX || point.localPosition.x == minX)
        {
            movingRight = !movingRight;
        }
    }

    bool CheckResult()
    {
        // Calculate bounds for the green zone
        RectTransform greenZoneRectTransform = greenZone.GetComponent<RectTransform>();
        RectTransform pointRectTransform = point.GetComponent<RectTransform>();
        
        Vector2 greenZoneMin = greenZoneRectTransform.localPosition - new Vector3(greenZoneRectTransform.rect.width * 0.5f, greenZoneRectTransform.rect.height * 0.5f);
        Vector2 greenZoneMax = greenZoneRectTransform.localPosition + new Vector3(greenZoneRectTransform.rect.width * 0.5f, greenZoneRectTransform.rect.height * 0.5f);
        
        if (pointRectTransform.anchoredPosition.x > greenZoneMin.x && pointRectTransform.anchoredPosition.x < greenZoneMax.x)
        {
            Debug.Log("You win! Stress is reduced by 1 point");
            StressManager.Instance.RemoveStressPoint();
            if (successSound != null)
            {
                Debug.Log("play success clip");
                successSound.Play();
            }
            return true;
        }
        else
        {
            Debug.Log("Game over!");
            if (failSound != null)
            {
                Debug.Log("play fail clip");
                failSound.Play();
            }
            return false;
        }
    }
    public void ResetGame()
    {
        // reset point to the left side of the bar
        float halfWidthOfPoint = rectTransform.rect.width / 2;
        float halfWidthOfBar = barRectTransform.rect.width / 2;
        float startingXPos = -halfWidthOfBar + halfWidthOfPoint;
        point.localPosition = new Vector3(startingXPos, point.localPosition.y, point.localPosition.z);

        isPlayerWin = false;
        isGameActive = true;
        movingRight = true;
        currentSpeed = speed / StressManager.Instance.GetCurrentStressPoints();
    }
}
