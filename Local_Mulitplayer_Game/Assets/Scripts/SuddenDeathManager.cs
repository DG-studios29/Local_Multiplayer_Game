using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class SuddenDeathManager : MonoBehaviour
{
    public static SuddenDeathManager Instance;

    [Header("Player Settings")]
    public GameObject playerPrefab;
    public int numberOfPlayers = 2;

    [Header("Spawn Points")]
    public List<Transform> suddenDeathSpawnPoints;

    [Header("Camera Tracking")]
    public CinemachineTargetGroup targetGroup;

    [Header("Sudden Death Settings")]
    public float suddenDeathDuration = 60f;
    private float timer;
    private bool suddenDeathActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartSuddenDeath();
    }

    private void Update()
    {
        if (suddenDeathActive)
        {
            HandleTimer();
        }
    }

    void StartSuddenDeath()
    {
        suddenDeathActive = true;
        timer = suddenDeathDuration;
        SpawnPlayers();
        Debug.Log("Sudden Death Started!");
    }

    void HandleTimer()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            EndSuddenDeath();
        }
    }

    void EndSuddenDeath()
    {
        suddenDeathActive = false;
        Debug.Log("Sudden Death Ended!");
        // Handle end of sudden death (show results, reset game, etc.)
    }

    void SpawnPlayers()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (i < suddenDeathSpawnPoints.Count)
            {
                GameObject player = Instantiate(playerPrefab, suddenDeathSpawnPoints[i].position, Quaternion.identity);
                player.name = "Player " + (i + 1);
                AddPlayerToCamera(player);
                player.GetComponent<PlayerController>().moveSpeed = 10f;
            }
        }
    }

    public void AddPlayerToCamera(GameObject player)
    {
        if (targetGroup != null && player != null)
        {
            targetGroup.AddMember(player.transform, 1f, 2f);
        }
    }
}
