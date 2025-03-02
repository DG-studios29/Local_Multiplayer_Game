using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private float maxHealth;
    [SerializeField] private int damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float targetPlayerRange;
    [SerializeField] private float openRange;

    public float MaxHealth => maxHealth;

    public int Damage => damage;

    public float MoveSpeed => moveSpeed;

    public float TargetPlayerRange => targetPlayerRange;

    public float OpenRange => openRange;
}
