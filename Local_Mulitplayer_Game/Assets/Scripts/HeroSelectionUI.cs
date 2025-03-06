using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeroSelectionUI : MonoBehaviour
{
    public static HeroSelectionUI Instance;

    public GameObject selectionPanel;
    public Button startGameButton;
    public List<HeroManager> heroButtons;
    private List<string> chosenHeroes = new List<string>();
    private int currentPlayerIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void Setup(int numberOfPlayers)
    {
        
        selectionPanel.SetActive(true);
        chosenHeroes.Clear();

        // fill the heroes list with default choices (FireMage) for the number of players
        for (int i = 0; i < numberOfPlayers; i++)
        {
            chosenHeroes.Add("FireMage");  // Default hero selection for each player
        }

    
        for (int i = 0; i < heroButtons.Count; i++)
        {
            heroButtons[i].Initialize(i);  // Pass the player index to the hero button
        }

        
        currentPlayerIndex = 0;
        startGameButton.interactable = false;
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
           
            currentPlayerIndex++;
           
            if (currentPlayerIndex >= chosenHeroes.Count)
            {
                startGameButton.interactable = true;
            }
            else
            {
                UpdateSelectionDisplay();
            }
        }
        else
        {
            
            Debug.LogError("Player cannot change the selection of another player!");
        }
    }

    private void UpdateSelectionDisplay()
    {
       
        Debug.Log($"Player {currentPlayerIndex + 1}, choose your hero!");
    }

    public void OnStartGame()
    {
        
        selectionPanel.SetActive(false);
        GameManager.Instance.StartGame(chosenHeroes);
    }
}
