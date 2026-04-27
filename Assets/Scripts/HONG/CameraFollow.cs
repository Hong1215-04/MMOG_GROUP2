using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float Damping;
    [SerializeField] private Vector3 offset;
    public Transform Player1;
    public Transform Player2;

    private Vector3 vel = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 targetpositionbeforeoff = (Player1.position + Player2.position) / 2f;
        Vector3 targetposition = targetpositionbeforeoff + offset;
        targetposition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetposition, ref vel, Damping);
    }
}
