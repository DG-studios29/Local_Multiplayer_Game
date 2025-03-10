using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject playerPrefab;
    public int numberOfPlayers = 2;

    [Header("Player Materials")]
    public List<Material> playerMaterials; // Add different materials for each player

    [Header("Spawn points")]
    public List<Transform> forestSpawnPoints; // Spawn points for the forest map
    public List<Transform> cemeterySpawnPoints; // Spawn points for the cemetery map
    private List<Transform> currentSpawnPoints; // To store the current spawn points based on the map

    [Header("Camera Tracking")]
    public CinemachineTargetGroup targetGroup;
    public CinemachineCamera cineCamera;
    public CinemachineBasicMultiChannelPerlin noise;

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
        Instance = this;
    }

    private void Start()
    {
        ShowHeroSelectionUI();
    }

    private void Update()
<<<<<<< Updated upstream
<<<<<<< Updated upstream
    {
        if (gameStarted)
        {
            HandleGameTimer();
        }
    }

    void HandleGameTimer()
    {
        HeroSelectionUI.Instance.Setup(numberOfPlayers);
    }

    public void StartGame(List<string> chosenHeroes)
    {
        selectedHeroes = chosenHeroes;

        // Fill any missing selections with a default hero
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
        currentSpawnPoints = forestSpawnPoints; // Set spawn points to forest initially
        SpawnPlayers(currentSpawnPoints);
    }

    void StartSuddenDeath()
    {
        isSuddenDeath = true;
        timer = suddenDeathDuration;
        RespawnPlayers(cemeterySpawnPoints); // Use cemetery spawn points for sudden death
        Debug.Log("Sudden Death Started!");
        mapChange(); // Change map to cemetery
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
=======
>>>>>>> Stashed changes
    {
        if (gameStarted)
        {
            HandleGameTimer();
        }
    }

    void HandleGameTimer()
    {
        HeroSelectionUI.Instance.Setup(numberOfPlayers);
<<<<<<< Updated upstream
=======
    }

    public void StartGame(List<string> chosenHeroes)
    {
        selectedHeroes = chosenHeroes;

        // Fill any missing selections with a default hero
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
        currentSpawnPoints = forestSpawnPoints; // Set spawn points to forest initially
        SpawnPlayers(currentSpawnPoints);
    }

    void StartSuddenDeath()
    {
        isSuddenDeath = true;
        timer = suddenDeathDuration;
        RespawnPlayers(cemeterySpawnPoints); // Use cemetery spawn points for sudden death
        Debug.Log("Sudden Death Started!");
        mapChange(); // Change map to cemetery
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
>>>>>>> Stashed changes
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
=======
>>>>>>> Stashed changes
    {
        if (gameStarted)
        {
            HandleGameTimer();
        }
    }

    void HandleGameTimer()
    {
        HeroSelectionUI.Instance.Setup(numberOfPlayers);
<<<<<<< Updated upstream
=======
    }

    public void StartGame(List<string> chosenHeroes)
    {
        selectedHeroes = chosenHeroes;

        // Fill any missing selections with a default hero
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
        currentSpawnPoints = forestSpawnPoints; // Set spawn points to forest initially
        SpawnPlayers(currentSpawnPoints);
    }

    void StartSuddenDeath()
    {
        isSuddenDeath = true;
        timer = suddenDeathDuration;
        RespawnPlayers(cemeterySpawnPoints); // Use cemetery spawn points for sudden death
        Debug.Log("Sudden Death Started!");
        mapChange(); // Change map to cemetery
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
>>>>>>> Stashed changes
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
        HeroSelectionUI.Instance.Setup(numberOfPlayers);
    }

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
            case "EarthGuardian":
                //player.AddComponent<EarthGuardian>();
                break;
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
            currentSpawnPoints = cemeterySpawnPoints; // Switch spawn points to cemetery
        }
        else
        {
            forestMap.SetActive(true);
            cemeteryMap.SetActive(false);
            currentSpawnPoints = forestSpawnPoints; // Switch spawn points to forest
        }
    }
}
