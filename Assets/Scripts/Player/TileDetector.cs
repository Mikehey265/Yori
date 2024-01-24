using UnityEngine;

public class TileDetector : MonoBehaviour
{
    private Vector3 lastTilePosition;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            MovePlayer.Instance.StopMoving(lastTilePosition);
        }

        if (other.gameObject.CompareTag("Tile"))
        {
            lastTilePosition = new Vector3(other.gameObject.transform.position.x, 1, other.gameObject.transform.position.z);
        }
    }
}
