using System;
using UnityEngine;

public class Joystick : TouchTrack
{
    [SerializeField] private Transform _joystickIndicator;
    [SerializeField] private float _screenFraction = 0.1f;
    [Space]
    [SerializeField] private OffsetIndicator _offsetIndicator;

    public event Action<Vector2> OnChange;
    public event Action OnStart;
    public event Action OnEnd;


    protected override void TrackDelta(Vector2 screenPosition) {
        base.TrackDelta(screenPosition);
        _offsetIndicator.SetEndPoint(screenPosition);
    }

    protected override void OnStartMove() {
        base.OnStartMove();

        _joystickIndicator.gameObject.SetActive(false);
        _offsetIndicator.Activate(StartTouchP, StartTouchP);
        OnStart?.Invoke();
    }

    protected override void OnEndMove() {
        base.OnEndMove();

        _joystickIndicator.gameObject.SetActive(true);
        _offsetIndicator.Deactivate();
        OnEnd?.Invoke();
    }

    protected override void SetOffset(Vector2 offset) {
        offset = offset / (Screen.height * _screenFraction);
        Offset = new(Mathf.Clamp(offset.x, -1, 1), Mathf.Clamp(offset.y, -1, 1));
        OnChange?.Invoke(Offset);
    }
}
