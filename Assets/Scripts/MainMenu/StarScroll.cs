using UnityEngine;
using UnityEngine.UI;

public class StarScroll : MonoBehaviour
{
    [SerializeField] private RawImage starLayer;
    [SerializeField] private float scrollSpeed = 0.01f;

    void Update()
    {
        starLayer.uvRect = new Rect(
            starLayer.uvRect.x + scrollSpeed * Time.deltaTime,
            starLayer.uvRect.y + (scrollSpeed * 1f) * Time.deltaTime,
            starLayer.uvRect.width,
            starLayer.uvRect.height
        );
    }
}