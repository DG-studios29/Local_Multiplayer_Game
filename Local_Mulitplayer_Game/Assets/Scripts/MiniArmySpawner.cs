using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniArmySpawner : MonoBehaviour
{
    [System.Serializable]
    public class ArmyType
    {
        public string name;
        public GameObject prefab;
        public Transform spawnPoint;
        public float cooldownTime;
    }

    [Header("Army Type")]
    public ArmyType[] armyTypes;
    private bool[] canSpawn;

    private int selectedArmyIndex = 0; // Tracks the currently selected army
    private MeshRenderer[] playerMeshRenderers;

    private void Start()
    {
        canSpawn = new bool[armyTypes.Length];
        for (int i = 0; i < canSpawn.Length; i++)
        {
            canSpawn[i] = true;
        }

        playerMeshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void SetPlayerMaterial(Material playerMaterial)
    {
        foreach (var renderer in playerMeshRenderers)
        {
            renderer.material = playerMaterial;
        }
    }

    // Switching army type using arrow keys (left/right on controller)
    public void SwitchArmy(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float input = context.ReadValue<float>();

            if (input > 0) // Right arrow or D-Pad Right
            {
                selectedArmyIndex = (selectedArmyIndex + 1) % armyTypes.Length;
            }
            else if (input < 0) // Left arrow or D-Pad Left
            {
                selectedArmyIndex = (selectedArmyIndex - 1 + armyTypes.Length) % armyTypes.Length;
            }

            Debug.Log($"Selected Army: {armyTypes[selectedArmyIndex].name}");
        }
    }

    // Spawn army using controller (X button)
    public void SpawnSelectedArmy(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TrySpawn(selectedArmyIndex);
        }
    }

    // Spawn army directly using number keys (1,2,3)
  
    public void SpawnArmyByKey1(InputAction.CallbackContext context) { if (context.performed) TrySpawn(0); }
    public void SpawnArmyByKey2(InputAction.CallbackContext context) { if (context.performed) TrySpawn(1); }
    public void SpawnArmyByKey3(InputAction.CallbackContext context) { if (context.performed) TrySpawn(2); }


    private void TrySpawn(int index)
    {
        if (canSpawn[index])
        {
            GameObject newArmy = Instantiate(armyTypes[index].prefab, armyTypes[index].spawnPoint.position, Quaternion.identity);
            EnemyAI spawnedEnemy = newArmy.GetComponent<EnemyAI>();
            spawnedEnemy.enemyParent = this.gameObject;

            MeshRenderer[] armyMeshRenderers = newArmy.GetComponentsInChildren<MeshRenderer>();
            foreach (var renderer in armyMeshRenderers)
            {
                renderer.material = playerMeshRenderers[0].material;
            }

            StartCoroutine(Cooldown(index, armyTypes[index].cooldownTime));
        }
        else
        {
            Debug.Log($"{armyTypes[index].name} is on cooldown!");
        }
    }

    private IEnumerator Cooldown(int index, float cooldown)
    {
        cooldown = 0.5f;

        canSpawn[index] = false;
        yield return new WaitForSeconds(cooldown);
        canSpawn[index] = true;
        Debug.Log($"{armyTypes[index].name} is ready to spawn again!");
    }
}
