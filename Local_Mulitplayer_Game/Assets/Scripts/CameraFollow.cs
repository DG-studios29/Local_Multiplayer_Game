using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public string playerTag = "Player";     // Tag to find player objects
    public Vector3 offset;                  // Camera offset position
    public float smoothTime = 0.3f;         // Smoothness of the camera movement
    public float minZoom = 20f;             // Minimum camera field of view
    public float maxZoom = 80f;             // Maximum camera field of view
    public float zoomPadding = 2f;         // Extra padding for zooming out
    public float zoomSpeed = 5f;           // Speed of zooming

    private Vector3 velocity;              // Velocity for smooth dampening
    private Camera cam;                    // Reference to the Camera component
    private List<Transform> players = new List<Transform>(); // List of player transforms

    void Start()
    {
        cam = GetComponent<Camera>();      // Get the Camera component
        FindPlayers();                     // Find players at the start
    }

    void LateUpdate()
    {
        // Continuously check for new players
        FindPlayers();                     

        if (players.Count == 0) return;

        MoveCamera();
        ZoomCamera();
    }

    void FindPlayers()
    {// Clear the current list
        players.Clear();                                           

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag(playerTag);
        foreach (GameObject player in playerObjects)
        {
            // Add each player’s transform
            players.Add(player.transform);                         
        }
    }

    void MoveCamera()
    {
        Vector3 centerPoint = GetCenterPoint();                    // Get the midpoint of all players
        Vector3 newPosition = centerPoint + offset;                // Set the camera position with offset

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void ZoomCamera()
    { // Get the max distance between players
        float greatestDistance = GetGreatestDistance();           
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, greatestDistance / 50f) + zoomPadding;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom, Time.deltaTime * zoomSpeed); // Smooth zoom
    }

    float GetGreatestDistance()
    {
        if (players.Count == 1) return 0f;

        var bounds = new Bounds(players[0].position, Vector3.zero);

        for (int i = 1; i < players.Count; i++)
        {
            // Encapsulate all player positions in bounds
            bounds.Encapsulate(players[i].position);               
        }
        
        return bounds.size.magnitude;                              
    }

    Vector3 GetCenterPoint()
    {
        if (players.Count == 1)
        {
            
            return players[0].position;                            
        }

        var bounds = new Bounds(players[0].position, Vector3.zero);

        for (int i = 1; i < players.Count; i++)
        {
            // Get bounds containing all players
            bounds.Encapsulate(players[i].position);              
        }

        return bounds.center;                                      
    }
}
