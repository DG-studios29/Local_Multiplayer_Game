using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectionUI : MonoBehaviour
{
    public static HeroSelectionUI Instance;

    public GameObject selectionPanel;
    public GameObject playerUICanvas;
    public Button startGameButton;
    public List<HeroManager> heroButtons;

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

        
            currentSelectingPlayer++;

            // Check if all players have chosen
            startGameButton.interactable = AreAllHeroesSelected();

        }

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
