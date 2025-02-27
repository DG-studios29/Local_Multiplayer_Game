using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public List<Transform> spawnPoints;

    private void Start()
    {
        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < spawnPoints.Count)
            {
                GameObject player = Instantiate(playerPrefab, spawnPoints[i].position, Quaternion.identity);
                var playerInput = player.GetComponent<PlayerInput>();

                // Assign input control schemes
                if (i == 0)
                {
                    playerInput.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current);
                    playerInput.defaultActionMap = "Player";
                    player.name = "Player 1";
                }
                else if (i == 1)
                {
                    playerInput.SwitchCurrentControlScheme("Arrows", Keyboard.current);
                    playerInput.defaultActionMap = "Player";
                    player.name = "Player 2";
                }
                else if (Gamepad.all.Count >= (i - 1))
                {
                    playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.all[i - 2]);
                    playerInput.defaultActionMap = "Player";
                    player.name = $"Player {i + 1} (Controller {i - 1})";
                }
            }
        }
    }
}
