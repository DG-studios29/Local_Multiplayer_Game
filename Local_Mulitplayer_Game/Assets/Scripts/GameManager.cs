using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab; // The player prefab to spawn
    public int numberOfPlayers = 2; // Number of players to spawn
    public List<Transform> spawnPoints; // List of spawn points for players
    public CinemachineTargetGroup targetGroup;

    private void Start()
    {
        SpawnPlayers(); // Call the function to spawn players when the game starts
    }

    // Call this function when a player spawns
    public void AddPlayerToCamera(GameObject player, float weight = 1f, float radius = 2f)
    {
        if (targetGroup == null || player == null) return;

        // Add the player to the target group
        targetGroup.AddMember(player.transform, weight, radius);
    }


    void SpawnPlayers()
    {
        // Loop through the number of players to spawn
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (i < spawnPoints.Count)
            {
                // Check if there's a valid spawn point available
                GameObject player = Instantiate(playerPrefab, spawnPoints[i].position, Quaternion.identity); // Instantiate the player at the spawn point
                player.name = "Player " + (i + 1); // Set the player's name
                // Add the player to the camera target group
                AddPlayerToCamera(player, 1f, 2f);
            }
            else
            {
                // Warn if there aren't enough spawn points for the number of players
                Debug.LogWarning("Not enough spawn points for all players!");
            }
        }
    }


}

// https://docs.unity3d.com/Packages/com.unity.cinemachine@2.7/api/Cinemachine.CinemachineTargetGroup.html#methods