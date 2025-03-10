using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniArmySpawner : MonoBehaviour
{
    [System.Serializable]
    public class ArmyType // Setup array requirement.
    {
        public string name;
        public GameObject prefab;
        public Transform spawnPoint;
        public float cooldownTime;
    }
    [Header("Army Type")]
    public ArmyType[] armyTypes;
    private bool[] canSpawn;

    private MeshRenderer[] playerMeshRenderers;

    private void Start()
    {
        canSpawn = new bool[armyTypes.Length]; // Initialize cooldown tracking array
        for (int i = 0; i < canSpawn.Length; i++)
        {
            canSpawn[i] = true; // Allow spawning initially
        }

        playerMeshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void SetPlayerMaterial(Material playerMaterial)
    {
        // Apply the material to all mesh renderers
        foreach (var renderer in playerMeshRenderers)
        {
            renderer.material = playerMaterial;
        }
    }
    public void SpawnArmy(InputAction.CallbackContext context) // Spawns army when  button 1 is pressed.
    {
        if (context.performed)
        {
            int armyIndex = Mathf.Clamp((int)context.ReadValue<float>(), 0, armyTypes.Length - 1); // Ensure index is within range
            TrySpawn(armyIndex);
        }
    }

    private void TrySpawn(int index) // Checks if spawn is possible.
    {
        if (canSpawn[index])
        {
<<<<<<< Updated upstream
            Instantiate(armyTypes[index].prefab, armyTypes[index].spawnPoint.position, Quaternion.identity); // Spawn the unit
=======
            GameObject newArmy = Instantiate(armyTypes[index].prefab, armyTypes[index].spawnPoint.position, Quaternion.identity); // Spawn the unit
            EnemyAI spawnedEnemy = newArmy.GetComponent<EnemyAI>();
            spawnedEnemy.enemyParent = this.gameObject; // Set parent as the player that spawned

            // Apply the player's material to all mesh renderers in the army prefab
            MeshRenderer[] armyMeshRenderers = newArmy.GetComponentsInChildren<MeshRenderer>();
            foreach (var renderer in armyMeshRenderers)
            {
                renderer.material = playerMeshRenderers[0].material; // Apply the player's material to each mesh renderer in the army
            }

>>>>>>> Stashed changes
            StartCoroutine(Cooldown(index, armyTypes[index].cooldownTime)); // Start cooldown coroutine
        }
        else
        {
            Debug.Log($"{armyTypes[index].name} is on cooldown!");
        }
    }


    private IEnumerator Cooldown(int index, float cooldown) // Handles cooldown timing.
    {
        canSpawn[index] = false; // Disable spawning for this unit type
        yield return new WaitForSeconds(cooldown);
        canSpawn[index] = true; // Enable spawning again
<<<<<<< Updated upstream
        Debug.Log($"{armyTypes[index].name} is ready to spawn again!"); // Notify cooldown is over
=======
        Debug.Log($"{armyTypes[index].name} is ready to spawn again!");
>>>>>>> Stashed changes
    }
}