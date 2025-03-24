using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
//https://docs.unity3d.com/6000.0/Documentation/ScriptReference/PlayerPrefs.html
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Settings")]
    public GameObject playerPrefab; // The player prefab to spawn
    public int numberOfPlayers = 2; // Default number of players

    [Header("Player Materials")]
    public List<Material> playerMaterials; // Materials for each player

    [Header("Spawn points")]
    public List<Transform> forestSpawnPoints; // Spawn points for players in the forest map
    public List<Transform> cemeterySpawnPoints; // Spawn points for players in the forest map
    public List<Transform> winterSpawnPoints; // Spawn points for players in the forest map
    private List<Transform> currentSpawnPoints; // Active spawn points during the match

    [Header("Camera Tracking")]
    public CinemachineTargetGroup targetGroup; // Tracks players for the camera
    public CinemachineCamera cineCamera; // Main game camera
    public CinemachineBasicMultiChannelPerlin noise; // Camera shake effect

    public List<string> selectedHeroes = new List<string>(); // Stores chosen heroes

    [Header("Game Timers")]
    public float gameDuration = 300f; // Total game time in seconds
    public float timer; // Countdown timer
    private bool gameStarted = false; // Tracks if the game has started

    [Header("Maps")]
    public string selectedMap = "Forest"; // Default map
    public GameObject forestMap; // The forest map object
    public GameObject cemeteryMap; // The cemetery map object
    public GameObject wintermap; // The winter map object

    [Header("Player UI")]
    public Slider player1HealthSlider;
    public TMP_Text player1HealthText;
    public Slider player2HealthSlider;
    public TMP_Text player2HealthText;
    public TextMeshProUGUI player1NameText;
    public TextMeshProUGUI player2NameText;

    [Header("Hero Abilities UI")]
    public Image player1Ability1Icon;
    public TMP_Text player1Ability1CooldownText;
    public Image player2Ability1Icon;
    public TMP_Text player2Ability1CooldownText;
    public Image player1Ability2Icon;
    public TMP_Text player1Ability2CooldownText;
    public Image player2Ability2Icon;
    public TMP_Text player2Ability2CooldownText;
    public Image player1Ability3Icon;
    public TMP_Text player1Ability3CooldownText;
    public Image player2Ability3Icon;
    public TMP_Text player2Ability3CooldownText;

    [Header("Game End Settings")]
    public TMP_Text endGameText; // To display the end game result (e.g., winner)
    public GameObject gameOverUI; // UI panel to show the game over screen
    public bool isPlayer1Alive = true;
    public bool isPlayer2Alive = true;

    // Player health references
    public PlayerHealth player1Health;
    public PlayerHealth player2Health;

    private bool shakeTriggered = false; // Tracks if camera shake has been triggered

    private void Awake()
    {
        Instance = this; 
    }

    private void Start()
    {
        ShowHeroSelectionUI(); // Show hero selection screen when game starts
    }

    void Update()
    {
        if (gameStarted)
        {
            HandleGameTimer();

            // Check if players are alive
            CheckPlayerHealth();
        }
    }

    void HandleGameTimer()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= gameDuration / 2 && !shakeTriggered)
            {
                TriggerCameraShake(); // Shake the camera halfway through the match
                shakeTriggered = true;
            }
        }
        
    }

    void CheckPlayerHealth()
    {
        // Check if Player 1 is alive
        GameObject player1 = GameObject.Find("Player 1");
        if (player1 != null)
        {
            PlayerHealth player1Health = player1.GetComponent<PlayerHealth>();
            if (player1Health.currentHealth <= 0 && isPlayer1Alive)
            {
                isPlayer1Alive = false;
                EndGame("Player 2 wins!"); // Player 2 wins if Player 1 dies
            }
        }

        // Check if Player 2 is alive
        GameObject player2 = GameObject.Find("Player 2");
        if (player2 != null)
        {
            PlayerHealth player2Health = player2.GetComponent<PlayerHealth>();
            if (player2Health.currentHealth <= 0 && isPlayer2Alive)
            {
                isPlayer2Alive = false;
                EndGame("Player 1 wins!"); // Player 1 wins if Player 2 dies
            }
        }

        // If both players are alive, check the timer
        if (timer <= 0)
        {
            if (isPlayer1Alive && isPlayer2Alive)
            {
                // If both players are alive when time runs out, check health
                int player1Health = (int)player1.GetComponent<PlayerHealth>().currentHealth;
                int player2Health = (int)player2.GetComponent<PlayerHealth>().currentHealth;
                if (player1Health > player2Health)
                {
                    EndGame("Player 1 wins!");
                }
                else if (player2Health > player1Health)
                {
                    EndGame("Player 2 wins!");
                }
                else
                {
                    EndGame("It's a draw!"); // In case both have the same health
                }
            }
        }
    }

    // End the game and display the result
    void EndGame(string winner)
    {
        gameStarted = false;

        // Store the winner's name in PlayerPrefs (or use a singleton for more complex storage)
        PlayerPrefs.SetString("Winner", winner);
        PlayerPrefs.Save(); // Ensure the value is saved

        Debug.Log(winner + " is the winner!");

        // Load the Game Over scene
        SceneManager.LoadScene("GameOver");
    }





    public void StartGame(List<string> chosenHeroes)
    {
        selectedHeroes = chosenHeroes;
        
        for (int i = 0; i < selectedHeroes.Count; i++)
        {
            if (string.IsNullOrEmpty(selectedHeroes[i]))
            {
                selectedHeroes[i] = "Blazeheart"; // Default hero if none chosen
            }
        }

        timer = gameDuration; // Reset timer
        shakeTriggered = false;
        gameStarted = true;
        
        SelectMap(selectedMap);
        SpawnPlayers(currentSpawnPoints); // Spawn the players
    }

    //void LoadSuddenDeathScene()
    //{
    //    Debug.Log("Loading Sudden Death Scene...");

    //    PlayerData.ClearData();

    //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    //    foreach (var player in players)
    //    {
    //        PlayerData.SavePlayerStats(player);
    //    }

    //    SceneManager.LoadScene("SuddenDeath");
    //}


    public void SetupPlayerUI(GameObject player, string playerName)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();


        // Assign UI elements to each player based on their name
        if (player.name == "Player 1")
        {
            playerHealth.healthSlider = player1HealthSlider;
            playerHealth.healthText = player1HealthText;
            player1NameText.text = playerName;

        }
        else if (player.name == "Player 2")
        {
            playerHealth.healthSlider = player2HealthSlider;
            playerHealth.healthText = player2HealthText;
            player2NameText.text = playerName;

        }

        playerHealth.UpdateHealthUI(); // Update health UI on start

    }

    public void SetupHeroAbilitiesUI(GameObject player)
    {
        // Get the HeroBase script from the player to access its abilities
        HeroBase hero = player.GetComponent<HeroBase>();

        // Check which player is setting up and assign corresponding UI elements
        if (player.name == "Player 1")
        {
            hero.ability1Icon = player1Ability1Icon;
            hero.ability2Icon = player1Ability2Icon;
            hero.ultimateIcon = player1Ability3Icon;

            player1Ability1Icon.sprite = hero.abilities.ability1.icon;
            player1Ability2Icon.sprite = hero.abilities.ability2.icon;
            player1Ability3Icon.sprite = hero.abilities.ultimate.icon;

            hero.ability1CooldownText = player1Ability1CooldownText;
            hero.ability2CooldownText = player1Ability2CooldownText;
            hero.ultimateCooldownText = player1Ability3CooldownText;
        }
        else if (player.name == "Player 2")
        {
            hero.ability1Icon = player2Ability1Icon;
            hero.ability2Icon = player2Ability2Icon;
            hero.ultimateIcon = player2Ability3Icon;

            player2Ability1Icon.sprite = hero.abilities.ability1.icon;
            player2Ability2Icon.sprite = hero.abilities.ability2.icon;
            player2Ability3Icon.sprite = hero.abilities.ultimate.icon;

            hero.ability1CooldownText = player2Ability1CooldownText;
            hero.ability2CooldownText = player2Ability2CooldownText;
            hero.ultimateCooldownText = player2Ability3CooldownText;
        }
    }


    void SpawnPlayers(List<Transform> points)
    {
        
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (i < points.Count)
            {
                // Spawn player at a spawn point
                GameObject player = Instantiate(playerPrefab, points[i].position, Quaternion.identity);
                player.name = "Player " + (i + 1);
                // Add the player to the players list
                
                AddPlayerToCamera(player, 1f, 2f); // Add player to camera tracking
                AssignHeroScript(player, selectedHeroes[i]); // Attach hero script
                AssignPlayerMaterials(player, i); // Assign player materials
                string playerName = "Player " + (i + 1);
                GameManager.Instance.SetupPlayerUI(player, playerName); // Setup UI
                GameManager.Instance.SetupHeroAbilitiesUI(player);
                RevealPlayerBase.instance.players.Add(player.transform);
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
        targetGroup.AddMember(player.transform, weight, radius); // Add player to camera group
    }

    void ShowHeroSelectionUI()
    {
        HeroSelectionUI.Instance.Setup(numberOfPlayers); // Show hero selection screen
    }

    void AssignHeroScript(GameObject player, string heroName)
    {
        HeroBase hero = null;
        HeroAbility abilityData = Resources.Load<HeroAbility>($"Abilities/{heroName}");

        switch (heroName)
        {
            case "Blazeheart":
                hero = player.AddComponent<Blazeheart>();
                break;
            case "Frost":
                hero = player.AddComponent<Frost>();
                break;
            case "Nightshade":
                hero = player.AddComponent<Nightshade>();
                break;
            case "Stonewarden":
                hero = player.AddComponent<Stonewarden>();
                break;
            default:
                Debug.LogWarning("Hero not found: " + heroName);
                return;
        }

        if (hero != null && abilityData != null)
        {
            hero.abilities = abilityData;
        }
        else
        {
            Debug.LogError($"Failed to assign abilities to {heroName}");
        }
    }


    void AssignPlayerMaterials(GameObject player, int playerIndex)
    {
        // Add hero-specific material based on selection
        if (selectedHeroes.Count > playerIndex)
        {
            string heroName = selectedHeroes[playerIndex];

            Material chosenMaterial = GetMaterialForHero(heroName);

            if (chosenMaterial != null)
            {
                MeshRenderer[] meshRenderers = player.GetComponentsInChildren<MeshRenderer>();
                foreach (var meshRenderer in meshRenderers)
                {
                    meshRenderer.material = chosenMaterial;
                }
            }
            else
            {
                Debug.LogWarning("No material found for hero: " + heroName);
            }
        }
        else
        {
            Debug.LogWarning("Player index out of range in AssignPlayerMaterials!");
        }
    }

    Material GetMaterialForHero(string heroName)
    {
        switch (heroName)
        {
            case "Blazeheart":
                return playerMaterials.Find(mat => mat.name == "BlazeheartMaterial");
            case "Frost":
                return playerMaterials.Find(mat => mat.name == "FrostMaterial");
            case "Nightshade":
                return playerMaterials.Find(mat => mat.name == "NightshadeMaterial");
            case "Stonewarden":
                return playerMaterials.Find(mat => mat.name == "StonewardenMaterial");
            default:
                Debug.LogWarning("No matching material for hero: " + heroName);
                return null;
        }
    }


    void TriggerCameraShake()
    {
        noise.AmplitudeGain = 1f; // Start camera shake
        noise.FrequencyGain = 2f;
        Invoke("StopCameraShake", 5f); // Stop shake after 5 seconds
    }

    void StopCameraShake()
    {
        noise.AmplitudeGain = 0f; // Stop camera shake
        noise.FrequencyGain = 0f;
    }

    public void SelectMap(string mapName)
    {
        selectedMap = mapName;

        // Deactivate all maps
        forestMap.SetActive(false);
        cemeteryMap.SetActive(false);
        wintermap.SetActive(false);

        // Activate the chosen map
        switch (mapName)
        {
            case "Forest":
                forestMap.SetActive(true);
                currentSpawnPoints = forestSpawnPoints;
                break;
            case "Cemetery":
                cemeteryMap.SetActive(true);
                currentSpawnPoints = cemeterySpawnPoints;
                break;
            case "Winter":
                wintermap.SetActive(true);
                currentSpawnPoints = winterSpawnPoints;
                break;
        }

        Debug.Log($"Map Selected: {mapName}");
    }

}
