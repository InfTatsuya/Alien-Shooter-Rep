using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    public delegate void OnStickInputValueUpdated(Vector2 direction);
    public event OnStickInputValueUpdated onStickInputValueUpdated;

    public delegate void OnStickTaped();
    public event OnStickTaped onTaped;

    [SerializeField] RectTransform thumbStickTransform;
    [SerializeField] RectTransform backgroundTransform;
    [SerializeField] RectTransform centerTransform;

    private bool isDragging;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPos = eventData.position;
        Vector2 centerPos = backgroundTransform.position;

        Vector2 localOffset = Vector2.ClampMagnitude(touchPos - centerPos, backgroundTransform.sizeDelta.x / 2); 
        thumbStickTransform.position = centerPos + localOffset;

        Vector2 inputVector = localOffset / (backgroundTransform.sizeDelta.x / 2);
        onStickInputValueUpdated?.Invoke(inputVector);

        isDragging = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        backgroundTransform.position = eventData.position;
        thumbStickTransform.position = eventData.position;

        isDragging = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        thumbStickTransform.localPosition = Vector2.zero;
        backgroundTransform.localPosition = Vector2.zero;

        onStickInputValueUpdated?.Invoke(Vector2.zero);

        if(!isDragging)
        {
            onTaped?.Invoke();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
