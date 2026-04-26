using UnityEngine;

public class SlowApplication : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement movement = other.GetComponent<PlayerMovement>(); 

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerMovement movement = other.GetComponent<PlayerMovement>();

    }
}
