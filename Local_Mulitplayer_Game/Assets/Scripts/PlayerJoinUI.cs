using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerJoinUI : MonoBehaviour
{
    public GameObject[] playerPanels;
    public TextMeshProUGUI[] playerLabels;
    public Color[] playerColors;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("[UI] OnPlayerJoined() was triggered!");

        int index = playerInput.playerIndex;
        Debug.Log($"[UI] Player joined with index: {index}");

        if (index < 0 || index >= playerPanels.Length)
        {
            Debug.LogWarning($"[UI] Invalid player index: {index}");
            return;
        }

        playerPanels[index].SetActive(true);

        if (playerLabels.Length > index && playerLabels[index] != null)
        {
            playerLabels[index].text = $"Player {index + 1} Joined!";
            Debug.Log($"[UI] Set player label: {playerLabels[index].name} → Player {index + 1} Joined!");
        }

        if (playerColors.Length > index)
        {
            Color playerColor = playerColors[index];

            var images = playerPanels[index].GetComponentsInChildren<Image>();
            foreach (var img in images)
            {
                Color original = img.color;
                Color adjustedColor = new Color(playerColor.r, playerColor.g, playerColor.b, original.a);
                img.color = adjustedColor;
            }

            Debug.Log($"[UI] Applied player color (preserved alpha): {playerColor}");
        }
    }
}
