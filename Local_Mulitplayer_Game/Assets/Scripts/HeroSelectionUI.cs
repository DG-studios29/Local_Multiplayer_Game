using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectionUI : MonoBehaviour
{
    public static HeroSelectionUI Instance;

    public GameObject selectionPanel;
    public GameObject playerUICanvas;
    public Button startGameButton;
    public List<HeroManager> heroButtons;

    public List<Image> playerHeroImages;  // List to hold Image components for each player
    public List<TextMeshProUGUI> playerHeroNames;    // List to hold Text components for player hero names

    private Dictionary<int, string> chosenHeroes = new Dictionary<int, string>();
    private int currentSelectingPlayer = 0;  // Tracks which player is selecting

    private void Awake()
    {
        Instance = this;
        playerUICanvas.SetActive(false);
    }

    public void Setup(int numberOfPlayers)
    {
        selectionPanel.SetActive(true);
        chosenHeroes.Clear();

        // Initialize all players with no selection
        for (int i = 0; i < numberOfPlayers; i++)
        {
            chosenHeroes[i] = "";
            // Ensure UI is cleared for each player
            playerHeroImages[i].sprite = null;  // Clear the image
            playerHeroNames[i].text = "Select Hero";  // Placeholder text
        }

        startGameButton.interactable = false;

        // Assign button listeners
        foreach (var button in heroButtons)
        {
            button.Initialize();
        }

        Debug.Log("Hero selection started.");
    }

    public void OnHeroSelected(string heroName)
    {
        if (chosenHeroes.ContainsKey(currentSelectingPlayer))
        {
            chosenHeroes[currentSelectingPlayer] = heroName;
            Debug.Log($"Player {currentSelectingPlayer + 1} selected {heroName}");

            // Update visual display for the current player
            UpdatePlayerHeroUI(currentSelectingPlayer, heroName);

            currentSelectingPlayer++;
            if (currentSelectingPlayer >= chosenHeroes.Count)
            {
                currentSelectingPlayer = 0; // Optionally cycle back to the first player
            }
        }

        // Check if all players have chosen
        startGameButton.interactable = AreAllHeroesSelected();
    }

    private void UpdatePlayerHeroUI(int playerIndex, string heroName)
    {
        // Update the hero's image and name in the UI
        Sprite heroSprite = GetHeroSprite(heroName);
        playerHeroImages[playerIndex].sprite = heroSprite;
        playerHeroNames[playerIndex].text = heroName;
    }

    private Sprite GetHeroSprite(string heroName)
    {
        // This function returns the corresponding sprite based on the hero name.
        // You would need to map hero names to specific sprites.
        // For example, you could use a dictionary or switch case to find the correct sprite.
        // For simplicity, assuming the heroName matches the file name.

        return Resources.Load<Sprite>($"Heroes/{heroName}"); // Example path to hero sprites
    }

    public void OnHeroChange()
    {
        // Reset the current player's hero selection
        if (chosenHeroes.ContainsKey(currentSelectingPlayer))
        {
            chosenHeroes[currentSelectingPlayer] = "";
            Debug.Log($"Player {currentSelectingPlayer + 1} changed their selection.");
        }

        // Allow the player to select a new hero (revert to selection mode for this player)
        Debug.Log($"Player {currentSelectingPlayer + 1} can now choose a new hero.");
    }

    private bool AreAllHeroesSelected()
    {
        foreach (var hero in chosenHeroes.Values)
        {
            if (string.IsNullOrEmpty(hero))
            {
                return false;
            }
        }
        return true;
    }

    public void OnStartGame()
    {
        selectionPanel.SetActive(false);
        GameManager.Instance.StartGame(new List<string>(chosenHeroes.Values));

        playerUICanvas.SetActive(true);
    }
}
