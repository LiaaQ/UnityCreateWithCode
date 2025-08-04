using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    public float speed = 10.0f;
    public float xRange = 10.0f;
    public GameObject foodProjectile;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Keep the player within the xRange limits
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);

        // Launch a projectile when the space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Launch a food projectile from the player's position
            Instantiate(foodProjectile, transform.position, foodProjectile.transform.rotation);
        }
    }
}
