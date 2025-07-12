using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TouchTrack : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector2 Offset { get; protected set; }
    public Vector2 StartTouchP { get; protected set; }
    protected RectTransform _canvasRect;
    protected Canvas _canvas;
    private int _trackingFingerId;
    private bool _isTracking;
    private bool _isMouse;


    protected virtual void Awake() {
        _canvas = GetComponentInParent<Canvas>();
        _canvasRect = (RectTransform)_canvas.transform;
    }

    protected virtual void Update() {
        if (_isTracking) {
            if (_isMouse) {
                if (Input.GetMouseButton(0))
                    TrackMouseDelta();
                else
                    StopMouseTracking();
            }
            else if (Input.touchCount > 0) {
                bool stillTouching = false;
                foreach (var touch in Input.touches) {
                    if (touch.fingerId == _trackingFingerId && touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) {
                        stillTouching = true;
                        break;
                    }
                }

                if (stillTouching == false)
                    StopTouchTracking();
                else
                    TrackTouchDelta();
            }
        }
    }

    private void TrackTouchDelta() {
        if (_isTracking == false) return;
        Vector2 _currentTouchP = StartTouchP;

        foreach (var touch in Input.touches) {
            if (touch.fingerId == _trackingFingerId) {
                TrackDelta(touch.position);
                break;
            }
        }
    }
    private void TrackMouseDelta() {
        if (_isTracking == false) return;
        TrackDelta(Input.mousePosition);
    }

    protected virtual void TrackDelta(Vector2 screenPosition) {
        Vector2 delta = screenPosition - StartTouchP;
        SetOffset(delta);
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (_isTracking == false) {
            if (Input.GetMouseButton(0)) {
                // Mouse
                _isTracking = true;
                _isMouse = true;
                StartTouchP = Input.mousePosition;

                OnStartMove();
            }
            else {
                // Touch
                foreach (var touch in Input.touches) {
                    if (touch.position == eventData.position) {
                        _isTracking = true;
                        _trackingFingerId = touch.fingerId;
                        StartTouchP = eventData.position;

                        OnStartMove();
                        break;
                    }
                }

                //Debug.LogWarning("Тач не найден!");
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (eventData.pointerId == _trackingFingerId)
            StopTouchTracking();
    }

    protected virtual void StopTouchTracking() {
        if (_isMouse == false) {
            _isTracking = false;
            _trackingFingerId = -1;

            OnEndMove();
            SetOffset(Vector2.zero);
        }
    }
    protected virtual void StopMouseTracking() {
        if (_isMouse) {
            _isTracking = false;
            _isMouse = false;

            OnEndMove();
            SetOffset(Vector2.zero);
        }
    }

    protected virtual void OnStartMove() { }
    protected virtual void OnEndMove() { }

    protected abstract void SetOffset(Vector2 offset);
}
