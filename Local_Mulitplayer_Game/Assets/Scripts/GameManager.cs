<<<<<<< HEAD
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
=======
﻿using System.Collections;
using System.Collections.Generic;
>>>>>>> Stashed changes
=======
﻿using System.Collections;
using System.Collections.Generic;
>>>>>>> Stashed changes
=======
﻿using System.Collections;
using System.Collections.Generic;
>>>>>>> Stashed changes
=======
﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
>>>>>>> parent of 43e7cd5 (Sudden death)
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Add singleton instance for easy access
<<<<<<< HEAD
=======
    public static GameManager Instance;

    [Header("Player Settings")]
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> parent of 43e7cd5 (Sudden death)
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
        HeroSelectionUI.Instance.Setup(numberOfPlayers);  // No need for callback now
    }

<<<<<<< HEAD
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
>>>>>>> parent of 43e7cd5 (Sudden death)
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

<<<<<<< HEAD
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> parent of 43e7cd5 (Sudden death)
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
}
