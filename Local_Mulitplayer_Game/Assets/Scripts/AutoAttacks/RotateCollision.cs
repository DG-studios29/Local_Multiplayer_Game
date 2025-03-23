using UnityEngine;

public class RotateCollision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public ItemRotate rotorParent;
    
    //private int projectileID;

    private float maxProjectileHP = 100;
    [SerializeField]private float projectileHP;


    void Start()
    {
        rotorParent = GetComponentInParent<ItemRotate>();
        projectileHP = maxProjectileHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
    }

    public void OnRotationCollision()
    {
        projectileHP -= 30;

        if(projectileHP < 0)
        {
            projectileHP = 0;
            DestroyProjectile();
        }

    }

    public void DestroyProjectile() 
    {
        //tell something to the rotor parent, like ID of destroyed projectile
        Destroy(gameObject);

    }
}
