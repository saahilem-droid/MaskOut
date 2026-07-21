using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonFX : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    public Vector3 normalScale = Vector3.one;
    public Vector3 hoverScale = new Vector3(1.08f, 1.08f, 1f);
    public Vector3 pressedScale = new Vector3(0.95f, 0.95f, 1f);
    public float speed = 12f;

    Vector3 targetScale;

    void Awake()
    {
        targetScale = normalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * speed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = normalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetScale = pressedScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetScale = hoverScale;
    }
}
