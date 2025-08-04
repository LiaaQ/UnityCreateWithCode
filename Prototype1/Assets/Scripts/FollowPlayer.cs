using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    private Vector3 offset = new Vector3(0, 6, -9);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Offset the camera position based on the player's position
        transform.position = player.transform.position + offset;
    }
}
