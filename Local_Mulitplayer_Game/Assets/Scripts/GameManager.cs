<<<<<<< Updated upstream
<<<<<<< Updated upstream
﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
=======
﻿using System.Collections;
using System.Collections.Generic;
>>>>>>> Stashed changes
=======
﻿using System.Collections;
using System.Collections.Generic;
>>>>>>> Stashed changes
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
<<<<<<< Updated upstream
    public static GameManager Instance;  // Add singleton instance for easy access
=======
    public static GameManager Instance;

    [Header("Player Settings")]
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    public GameObject playerPrefab;
    public int numberOfPlayers = 2;

    [Header("Spawn points")]
    public List<Transform> spawnPoints;
    public List<Transform> suddenDeathSpawnPoints;

    [Header("Camera Tracking")]
    public CinemachineTargetGroup targetGroup;
    public CinemachineCamera cineCamera;
    public CinemachineBasicMultiChannelPerlin noise;

    [Header("Player Materials")]
    public List<Material> playerMaterials;

    public List<string> selectedHeroes = new List<string>();

    [Header("Game Timers")]
    public float gameDuration = 300f; // 5 minutes
    public float suddenDeathDuration = 60f; // 1 minute
    public float timer;
    private bool isSuddenDeath = false;
    private bool gameStarted = false; // To check if the game has started

    [Header("Maps")]
    public GameObject forestMap;
    public GameObject cemeteryMap;

    private bool shakeTriggered = false;

    private void Awake()
    {
        Instance = this;  // Singleton setup
    }

    private void Start()
    {
        ShowHeroSelectionUI();
    }

    private void Update()
    {
        if (gameStarted)
        {
            HandleGameTimer();
        }
    }

    void HandleGameTimer()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            // Check if timer has reached halfway
            if (timer <= gameDuration / 2 && !shakeTriggered)
            {
                TriggerCameraShake(); // Trigger shake when timer reaches halfway
                shakeTriggered = true;
            }
        }
        else
        {
            if (!isSuddenDeath)
            {
                StartSuddenDeath();
            }
            else
            {
                EndGame();
            }
        }
    }

    public void StartGame(List<string> chosenHeroes)
    {
        selectedHeroes = chosenHeroes;

        for (int i = 0; i < selectedHeroes.Count; i++)
        {
            if (string.IsNullOrEmpty(selectedHeroes[i]))
            {
                selectedHeroes[i] = "FireMage";
            }
        }

        timer = gameDuration;
        shakeTriggered = false;
        gameStarted = true; 
        SpawnPlayers(spawnPoints);
    }

    void StartSuddenDeath()
    {
        isSuddenDeath = true;
        timer = suddenDeathDuration;
        RespawnPlayers(suddenDeathSpawnPoints);
        Debug.Log("Sudden Death Started!");
        mapChange();
    }

    void EndGame()
    {
        gameStarted = false; // Stop the timer when the game ends
        Debug.Log("Game Over!");
        // Implement game over logic (show results, reset game, etc.)
    }

    void SpawnPlayers(List<Transform> points)
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (i < points.Count)
            {
                GameObject player = Instantiate(playerPrefab, points[i].position, Quaternion.identity);
                player.name = "Player " + (i + 1);
                AddPlayerToCamera(player, 1f, 2f);

                var controller = player.GetComponent<PlayerController>();
                controller.moveSpeed = isSuddenDeath ? 10f : 5f; // Speed up during sudden death
                AssignHeroScript(player, selectedHeroes[i]);

                // Change the player's materials
                AssignPlayerMaterials(player, i); // Assign materials based on player index
            }
            else
            {
                Debug.LogWarning("Not enough spawn points for all players!");
            }
        }
    }




    void RespawnPlayers(List<Transform> points)
    {
        // Create a copy of the spawn points to avoid modifying the original list while iterating
        List<Transform> availablePoints = new List<Transform>(points);

        // Shuffle the available points to randomize the selection order
        for (int i = 0; i < availablePoints.Count; i++)
        {
            Transform temp = availablePoints[i];
            int randomIndex = Random.Range(i, availablePoints.Count);
            availablePoints[i] = availablePoints[randomIndex];
            availablePoints[randomIndex] = temp;
        }

        // Respawn players at different spawn points
        int playerIndex = 0;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (playerIndex < availablePoints.Count)
            {
                player.transform.position = availablePoints[playerIndex].position;
                player.GetComponent<PlayerController>().moveSpeed = 10f; // Faster pace in sudden death
                playerIndex++;
            }
        }
    }

    public void AddPlayerToCamera(GameObject player, float weight = 1f, float radius = 2f)
    {
        if (targetGroup == null || player == null) return;
        targetGroup.AddMember(player.transform, weight, radius);
    }

    void ShowHeroSelectionUI()
    {
        HeroSelectionUI.Instance.Setup(numberOfPlayers);  // No need for callback now
    }

<<<<<<< Updated upstream
<<<<<<< Updated upstream
    public void StartGame(List<string> chosenHeroes)
    {
        selectedHeroes = chosenHeroes;

        // Ensure we have enough heroes selected
        while (selectedHeroes.Count < numberOfPlayers)
        {
            selectedHeroes.Add("FireMage"); // Add a default hero if any selection is missing
        }

        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (i < spawnPoints.Count)
            {
                GameObject player = Instantiate(playerPrefab, spawnPoints[i].position, Quaternion.identity);
                player.name = "Player " + (i + 1);
                AddPlayerToCamera(player, 1f, 2f);

                AssignHeroScript(player, selectedHeroes[i]);
            }
            else
            {
                Debug.LogWarning("Not enough spawn points for all players!");
            }
        }
    }

=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    void AssignHeroScript(GameObject player, string heroName)
    {
        switch (heroName)
        { // Assign the hero script the players
            case "FireMage":
                player.AddComponent<FireMage>();
                break;
            case "IceWarrior":
                player.AddComponent<IceWarrior>();
                break;
            case "ShadowRogue":
                player.AddComponent<ShadowRogue>();
                break;
<<<<<<< Updated upstream
=======
            case "EarthGuardian":
                break;
>>>>>>> Stashed changes
            default:
                Debug.LogWarning("Hero not found: " + heroName);
                break;
        }
    }
    void AssignPlayerMaterials(GameObject player, int playerIndex)
    {
        // Ensure there are enough materials for the players
        if (playerMaterials.Count > 0)
        {
            MeshRenderer[] meshRenderers = player.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                // Assign a unique material to each renderer for the player
                meshRenderer.material = playerMaterials[playerIndex % playerMaterials.Count];
            }
        }
        else
        {
            Debug.LogWarning("Player materials not assigned!");
        }
    }
    // Camera Shake Function
    void TriggerCameraShake()
    {
        // Set shake amplitude and frequency for the shake effect
        noise.AmplitudeGain = 1f; 
        noise.FrequencyGain = 2f; 

        
        Invoke("StopCameraShake", 5f);
    }

    void StopCameraShake()
    {
        // Reset 
        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
    }
    public void mapChange()
    {
        // Switch the maps 
        if (isSuddenDeath)
        {
            forestMap.SetActive(false);
            cemeteryMap.SetActive(true);
        }
        else
        {
            forestMap.SetActive(true);
            cemeteryMap.SetActive(false);
        }
    }
}
