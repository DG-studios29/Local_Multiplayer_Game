using UnityEngine;

public class ItemRotate : ItemObject
{
    public GameObject rotationPivot;
    public GameObject rotateObject;
    //public Transform rotationPivot;

    public GameObject pivotInstance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
     protected override void Start()
    {

        base.Start();

        rotationPivot = new GameObject("Empty");
        pivotInstance = Instantiate(rotationPivot, transform.position, Quaternion.identity,transform); // or just create a transform that sits where we want on the player
      
        rotateObject = itemData.projectilePrefab;

        SetUpRotor();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();  
    }


    protected override void DoAttack()
    {
        //base.DoAttack();
        //will deal area damage within that radius by attack rate


    }


    private void SetUpRotor()
    {
        GameObject pivotInstanceN = Instantiate(rotationPivot, pivotInstance.transform.position, Quaternion.Euler(0,0,0), pivotInstance.transform); // or just create a transform that sits where we want on the player
        GameObject nR = Instantiate(rotateObject, new Vector3(pivotInstanceN.transform.position.x + radius,
                                              pivotInstanceN.transform.position.y,
                                              pivotInstanceN.transform.position.z) ,
            Quaternion.identity,
            pivotInstanceN.transform);

        GameObject pivotInstanceS = Instantiate(rotationPivot, pivotInstance.transform.position, Quaternion.Euler(0,180,0), pivotInstance.transform);
        GameObject sR = Instantiate(rotateObject, new Vector3(pivotInstanceS.transform.position.x + radius,
                                              pivotInstanceS.transform.position.y,
                                              pivotInstanceS.transform.position.z) ,
            Quaternion.identity,
            pivotInstanceS.transform);

        GameObject pivotInstanceW = Instantiate(rotationPivot, pivotInstance.transform.position, Quaternion.Euler(0,90,0), pivotInstance.transform);
        GameObject wR = Instantiate(rotateObject, new Vector3(pivotInstanceW.transform.position.x + radius,
                                              pivotInstanceW.transform.position.y,
                                              pivotInstanceW.transform.position.z) ,
            Quaternion.identity,
            pivotInstanceW.transform);

        GameObject pivotInstanceE = Instantiate(rotationPivot,pivotInstance.transform.position, Quaternion.Euler(0,270,0), pivotInstance.transform);
        GameObject eR = Instantiate(rotateObject, new Vector3(pivotInstanceE.transform.position.x + radius,
                                              pivotInstanceE.transform.position.y,
                                              pivotInstanceE.transform.position.z) ,
            Quaternion.identity,
            pivotInstanceE.transform);

      


    }

}
