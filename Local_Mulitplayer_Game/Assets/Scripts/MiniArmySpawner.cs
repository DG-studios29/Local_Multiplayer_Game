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

    public ArmyType[] armyTypes; 
    private bool[] canSpawn; 

    private void Start()
    {
        canSpawn = new bool[armyTypes.Length]; // Initialize cooldown tracking array
        for (int i = 0; i < canSpawn.Length; i++)
        {
            canSpawn[i] = true; // Allow spawning initially
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
            GameObject newArmy = Instantiate(armyTypes[index].prefab, armyTypes[index].spawnPoint.position, Quaternion.identity); // Spawn the unit
            EnemyAI spawnedEnemy = newArmy.GetComponent<EnemyAI>();
            spawnedEnemy.enemyParent = this.gameObject; //parent will be the player that spawned

            StartCoroutine(Cooldown(index, armyTypes[index].cooldownTime)); // Start cooldown coroutine
<<<<<<< HEAD
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

            StartCoroutine(Cooldown(index, armyTypes[index].cooldownTime));
<<<<<<< HEAD
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
=======
>>>>>>> parent of 93b5622 (Sudden death)
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> parent of 43e7cd5 (Sudden death)
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
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        Debug.Log($"{armyTypes[index].name} is ready to spawn again!"); // Notify cooldown is over
=======
        Debug.Log($"{armyTypes[index].name} is ready to spawn again!");
>>>>>>> Stashed changes
<<<<<<< HEAD
=======
        Debug.Log($"{armyTypes[index].name} is ready to spawn again!");
>>>>>>> Stashed changes
=======
        Debug.Log($"{armyTypes[index].name} is ready to spawn again!");
>>>>>>> Stashed changes
=======
        Debug.Log($"{armyTypes[index].name} is ready to spawn again!"); // Notify cooldown is over
>>>>>>> parent of 43e7cd5 (Sudden death)
=======
>>>>>>> parent of 93b5622 (Sudden death)
=======
        Debug.Log($"{armyTypes[index].name} is ready to spawn again!"); 
>>>>>>> main
    }
}