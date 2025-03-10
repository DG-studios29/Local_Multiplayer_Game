using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeroSelectionUI : MonoBehaviour
{
    public static HeroSelectionUI Instance;
    [Header("UI Elements")]
    public GameObject selectionPanel;
    public Button startGameButton;
    public List<HeroManager> heroButtons;
    private List<string> chosenHeroes = new List<string>();
    private int currentPlayerIndex = 0;

    private void Awake()
    {
        // Singleton setup: this is the only instance of HeroSelectionUI
        Instance = this;
    }

    public void Setup(int numberOfPlayers)
    {
        // Show the hero selection panel
        selectionPanel.SetActive(true);

        // Reset the chosen heroes list for the new game
        chosenHeroes.Clear();

        // Pre-fill the heroes list with default choices (FireMage) for the number of players
        for (int i = 0; i < numberOfPlayers; i++)
        {
            chosenHeroes.Add("FireMage");  // Default hero selection for each player
        }

        // Initialize each hero button with the player index
        for (int i = 0; i < heroButtons.Count; i++)
        {
            heroButtons[i].Initialize(i);  // Pass the player index to the hero button
        }

        // Start with the first player selecting their hero
        currentPlayerIndex = 0;

        // Disable the start button until all players have selected a hero
        startGameButton.interactable = false;

        // Update the UI to display the current player's turn
        UpdateSelectionDisplay();
    }

    public void OnHeroSelected(int playerIndex, string heroName)
    {
        // Ensure that only the current player can select a hero
        if (playerIndex == currentPlayerIndex)
        {
            // Set the chosen hero for the current player
            if (playerIndex < chosenHeroes.Count)
            {
                chosenHeroes[playerIndex] = heroName;
                Debug.Log($"Player {playerIndex + 1} selected {heroName}");
            }

            // Move on to the next player after a selection is made
            currentPlayerIndex++;

            // If all players have selected their heroes, enable the start game button
            if (currentPlayerIndex >= chosenHeroes.Count)
            {
                startGameButton.interactable = true;
            }
            else
            {
                // Update the display for the next player
                UpdateSelectionDisplay();
            }
        }
        else
        {
            // Log an error if the player tries to change someone else's selection
            Debug.LogError("Player cannot change the selection of another player!");
        }
    }

    private void UpdateSelectionDisplay()
    {
        // Show which player is currently selecting their hero
        Debug.Log($"Player {currentPlayerIndex + 1}, choose your hero!");
    }

    public void OnStartGame()
    {
        // Hide the selection panel once the game starts
        selectionPanel.SetActive(false);

        // Call the game manager to start the game directly with the selected heroes
        GameManager.Instance.StartGame(chosenHeroes);
    }
}
