using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnButtonDown;
    public event Action OnButtonUp;


    public void OnPointerDown(PointerEventData eventData) {
        OnButtonDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData) {
        OnButtonUp?.Invoke();
    }
}
