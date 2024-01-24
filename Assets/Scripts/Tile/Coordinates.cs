using TMPro;
using UnityEngine;

[ExecuteAlways]
public class Coordinates : MonoBehaviour
{
    private TextMeshPro label;
    private Vector2Int coordinates;
    private bool isWalkable;
    
    private void Awake()
    {
        label = GetComponent<TextMeshPro>();
     //   DisplayCoordinates();
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
      //      DisplayCoordinates();
            UpdateObjectName();
        }
    }

 /*   private void DisplayCoordinates()
    {
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);

        label.text = coordinates.x + "," + coordinates.y;
    }*/

    private void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }
}
