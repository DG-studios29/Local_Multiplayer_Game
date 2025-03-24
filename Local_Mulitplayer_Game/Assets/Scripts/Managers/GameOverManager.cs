using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    
    public GameObject playerWinner1; // Panel for player 1's win
    public GameObject playerWinner2; // Panel for player 2's win

    void Start()
    {
        // Retrieve the winner's name from PlayerPrefs
        string winnerName = PlayerPrefs.GetString("Winner", "No winner");

        // Activate the appropriate winner panel based on the winner's name
        if (winnerName == "Player 1 wins!") // Change this condition based on how you set player names
        {
            playerWinner1.SetActive(true);
            playerWinner2.SetActive(false); // Ensure the other panel is hidden
        }
        else if (winnerName == "Player 2 wins!")
        {
            playerWinner1.SetActive(false); // Ensure player 1's panel is hidden
            playerWinner2.SetActive(true);
        }
        else if (winnerName == "It's a draw!")
        {
            playerWinner1.SetActive(false);
            playerWinner2.SetActive(false);
        }

        // Optional: Clear the PlayerPrefs entry for the next game
        PlayerPrefs.DeleteKey("Winner");
    }
}
