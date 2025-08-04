using UnityEngine;

public class SpinPropeller : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation in degrees per second

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
