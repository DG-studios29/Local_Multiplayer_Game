using UnityEngine;

public class ItemShoot : ItemObject
{
  


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        Debug.Log("called");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void DoAttack()
    {
        base.DoAttack();
    }
}
