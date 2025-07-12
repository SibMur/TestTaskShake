using System;
using UnityEngine;

public class TouchPad : TouchTrack
{
    [SerializeField] private Transform _pointer;
    [SerializeField] private float _sensitivity = 150;
    public event Action<Vector2> OnChange;
    public event Action OnStart;
    public event Action OnEnd;


    protected override void TrackDelta(Vector2 screenPosition) {
        base.TrackDelta(screenPosition);
        MovePointer(screenPosition);
        StartTouchP = screenPosition;
    }

    private void MovePointer(Vector2 screenPosition) {
        var rectTransform = (RectTransform)_pointer.transform;

        Camera camera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPosition, camera, out Vector2 localPoint);

        Vector3 worldPos = _canvasRect.TransformPoint(localPoint);
        Vector2 adjustedLocalPos = ((RectTransform)_pointer.transform.parent).InverseTransformPoint(worldPos);
        rectTransform.anchoredPosition = adjustedLocalPos;
    }

    protected override void OnStartMove() {
        base.OnStartMove();

        _pointer.gameObject.SetActive(true);
        OnStart?.Invoke();
    }

    protected override void OnEndMove() {
        base.OnEndMove();

        _pointer.gameObject.SetActive(false);
        OnEnd?.Invoke();
    }

    protected override void SetOffset(Vector2 offset) {
        Offset = (offset / Screen.height) * _sensitivity;
        OnChange?.Invoke(Offset);
    }
}
