using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;

    // Update is called once per frame
    void Update()
    {
        // On spacebar press, send dog
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Check if the player has reached the limit of dogs
            if (GameObject.FindGameObjectsWithTag("Dog").Length == 1)
            {
                Debug.Log("You have reached the limit of dogs!");
                return;
            }
            else
            {
                Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
            }
        }
    }
}
