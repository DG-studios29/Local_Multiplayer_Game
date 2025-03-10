using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Settings")]
    public GameObject playerPrefab;
    public int numberOfPlayers = 2;

    [Header("Player Materials")]
    public List<Material> playerMaterials;

    [Header("Spawn points")]
    public List<Transform> forestSpawnPoints;
    private List<Transform> currentSpawnPoints;

    [Header("Camera Tracking")]
    public CinemachineTargetGroup targetGroup;
    public CinemachineCamera cineCamera;
    public CinemachineBasicMultiChannelPerlin noise;

    public List<string> selectedHeroes = new List<string>();

    [Header("Game Timers")]
    public float gameDuration = 300f;
    public float timer;
    private bool gameStarted = false;

    [Header("Maps")]
    public GameObject forestMap;

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
            if (timer <= gameDuration / 2 && !shakeTriggered)
            {
                TriggerCameraShake();
                shakeTriggered = true;
            }
        }
        else
        {
            LoadSuddenDeathScene();
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
        currentSpawnPoints = forestSpawnPoints;
        SpawnPlayers(currentSpawnPoints);
    }

    void LoadSuddenDeathScene()
    {
        Debug.Log("Loading Sudden Death Scene...");
        SceneManager.LoadScene("SuddenDeath");
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
                AssignHeroScript(player, selectedHeroes[i]);
                AssignPlayerMaterials(player, i);
            }
            else
            {
                Debug.LogWarning("Not enough spawn points for all players!");
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
        if (playerMaterials.Count > 0)
        {
            MeshRenderer[] meshRenderers = player.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material = playerMaterials[playerIndex % playerMaterials.Count];
            }
        }
        else
        {
            Debug.LogWarning("Player materials not assigned!");
        }
    }

    void TriggerCameraShake()
    {
        noise.AmplitudeGain = 1f;
        noise.FrequencyGain = 2f;
        Invoke("StopCameraShake", 5f);
    }

    void StopCameraShake()
    {
        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
    }
}