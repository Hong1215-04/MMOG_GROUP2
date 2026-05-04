using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera selectedcam;
    public GameObject Player1;
    public GameObject Player2;
    private float zoom;
    private float smoothTime = 0.25f;
    private float minzoom = 3f;
    private float maxzoom = 15f;
    private float velocity = 0f;
    private float maxdistance = 28f;

    Vector2 Player1Pos;
    Vector2 Player2Pos;
    float distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        zoom = selectedcam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        Player1Pos = Player1.transform.position;
        Player2Pos = Player2.transform.position;
        distance = Vector2.Distance(Player1Pos, Player2Pos);

        float zoomcontrol = Mathf.Clamp01(distance / maxdistance);
        zoom = Mathf.Lerp(minzoom, maxzoom, zoomcontrol);

        selectedcam.orthographicSize = Mathf.SmoothDamp(selectedcam.orthographicSize, zoom, ref velocity, smoothTime);

    }
}
