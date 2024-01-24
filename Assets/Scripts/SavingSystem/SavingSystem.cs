using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    public static SavingSystem Instance { get; private set; }
    
    private const string PLAYER_PREFS_POS_X = "playerX";
    private const string PLAYER_PREFS_POS_Z= "playerZ";
    private const string PLAYER_PREFS_STEPSCOUNT = "stepsCount";
    private const string PLAYER_PREFS_STRESSAMOUNT = "stressAmount";

    private const string PLAYER_PREFS_ENEMY_POS_X = "enemyX";
    private const string PLAYER_PREFS_ENEMY_POS_Z = "enemyZ";
    private const string PLAYER_PREFS_ENEMY_PATROL_INDEX = "enemyPatrolIndex";

    private const string PLAYER_PREFS_CURRENT_ROOM = "currentRoom";

    private void Awake()
    {
        Instance = this;
    }

    public void SavePlayerStats(float playerXPos, float playerZPos, int stepsCount, int stressAmount)
    {
        PlayerPrefs.SetFloat(PLAYER_PREFS_POS_X, playerXPos);
        PlayerPrefs.SetFloat(PLAYER_PREFS_POS_Z, playerZPos);
        PlayerPrefs.SetInt(PLAYER_PREFS_STEPSCOUNT, stepsCount);
        PlayerPrefs.SetInt(PLAYER_PREFS_STRESSAMOUNT, stressAmount);
        PlayerPrefs.Save();
        // Debug.Log("Saved");
    }

    public void SaveEnemyPosition(float enemyXPos, float enemyZPos, int currentPatrolIndex)
    {
        PlayerPrefs.SetFloat(PLAYER_PREFS_ENEMY_POS_X, enemyXPos);
        PlayerPrefs.SetFloat(PLAYER_PREFS_ENEMY_POS_Z, enemyZPos);
        PlayerPrefs.SetInt(PLAYER_PREFS_ENEMY_PATROL_INDEX, currentPatrolIndex);
        PlayerPrefs.Save();
        Debug.Log("Saved Enemy Pos, x: " + enemyXPos + ", z: " + enemyZPos );
    }

    public float GetSavedPlayerPosX()
    {
        return PlayerPrefs.GetFloat(PLAYER_PREFS_POS_X);
    }
    
    public float GetSavedPlayerPosZ()
    {
        return PlayerPrefs.GetFloat(PLAYER_PREFS_POS_Z);
    }

    public int GetSavedPlayerClicks()
    {
        return PlayerPrefs.GetInt(PLAYER_PREFS_STEPSCOUNT);
    }

    public int GetCurrentPlayerStress()
    {
        return PlayerPrefs.GetInt(PLAYER_PREFS_STRESSAMOUNT);
    }

    public bool ArePlayerStatsSaved()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_POS_X) && PlayerPrefs.HasKey(PLAYER_PREFS_POS_Z) && PlayerPrefs.HasKey(PLAYER_PREFS_STEPSCOUNT) && PlayerPrefs.HasKey(PLAYER_PREFS_STRESSAMOUNT))
        {
            Debug.Log("Saved stats");
            return true;
        }

        Debug.Log("No saved stats");
        return false;
    }
    
    public float GetSavedEnemyPosX()
    {
        return PlayerPrefs.GetFloat(PLAYER_PREFS_ENEMY_POS_X);
    }
    
    public float GetSavedEnemyPosZ()
    {
        return PlayerPrefs.GetFloat(PLAYER_PREFS_ENEMY_POS_Z);
    }

    public int GetEnemyCurrentPatrolIndex()
    {
        return PlayerPrefs.GetInt(PLAYER_PREFS_ENEMY_PATROL_INDEX);
    }

    public bool IsEnemyPositionSaved()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_ENEMY_POS_X) && PlayerPrefs.HasKey(PLAYER_PREFS_ENEMY_POS_Z) && PlayerPrefs.HasKey(PLAYER_PREFS_ENEMY_PATROL_INDEX))
        {
            Debug.Log("Enemy saved stats");
            return true;
        }
        
        Debug.Log("No enemy saved stats");
        return false;
    }

    public void SaveCurrentRoom(int currentRoom)
    {
        PlayerPrefs.SetInt(PLAYER_PREFS_CURRENT_ROOM, currentRoom);
        PlayerPrefs.Save();
    }

    public int GetCurrentRoom()
    {
        return PlayerPrefs.GetInt(PLAYER_PREFS_CURRENT_ROOM);
    }

    public bool IsCurrentRoomSaved()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_CURRENT_ROOM))
        {
            return true;
        }
        return false;
    }

}
