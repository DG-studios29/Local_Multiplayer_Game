[System.Serializable]
public class PlayerStats
{
    public string playerName;
    public string heroClass;
    public float health;
    public float maxHealth;

    public PlayerStats(string name, string hero, float hp, float maxHp)
    {
        playerName = name;
        heroClass = hero;
        health = hp;
        maxHealth = maxHp;
    }
}
