using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

using Unity.Cinemachine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Add singleton instance for easy access
    public GameObject playerPrefab;
    public int numberOfPlayers = 2;
    public List<Transform> spawnPoints;
    public CinemachineTargetGroup targetGroup;

    public List<string> selectedHeroes = new List<string>();

    private void Awake()
    {
        Instance = this;  // Singleton setup
    }

    private void Start()
    {
        ShowHeroSelectionUI();
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

    void AssignHeroScript(GameObject player, string heroName)
    {
        switch (heroName)
        {
            case "FireMage":
                player.AddComponent<FireMage>();
                break;
            case "IceWarrior":
                player.AddComponent<IceWarrior>();
                break;
            case "ShadowRogue":
                player.AddComponent<ShadowRogue>();
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
