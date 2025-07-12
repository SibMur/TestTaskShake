using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffsetIndicator : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 20;
    [SerializeField] private float _minScale = 0.5f;
    [SerializeField] private float _minAlpha = 0.2f;
    [SerializeField] private float _alphaDistanceScale = 3f;
    [Space]
    [SerializeField] private List<Image> _points;
    protected RectTransform _canvasRect;
    protected Canvas _canvas;
    private Vector2 _startPoint;
    private Vector2 _endPoint;


    protected virtual void Awake() {
        _canvas = GetComponentInParent<Canvas>();
        _canvasRect = (RectTransform)_canvas.transform;
    }

    public void Activate(Vector2 startScreenPoint, Vector2 endScreenPoint) {
        _startPoint = startScreenPoint;
        _endPoint = endScreenPoint;
        UpdateVisual();
        gameObject.SetActive(true);
    }

    public void SetStartPoint(Vector2 screenPoint) {
        _startPoint = screenPoint;
        UpdateVisual();
    }

    public void SetEndPoint(Vector2 screenPoint) {
        _endPoint = screenPoint;
        UpdateVisual();
    }

    private void UpdateVisual() {
        Camera cam = (_canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : _canvas.worldCamera;
        RectTransform parentRect = _points[0].rectTransform.parent as RectTransform;

        // Экранные координаты в локальные точек
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, _startPoint, cam, out Vector2 localStart);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, _endPoint, cam, out Vector2 localEnd);

        float maxStep = _maxDistance / _canvas.scaleFactor;
        float totalDistance = Vector2.Distance(localStart, localEnd);
        float remainingDistance = totalDistance;
        Vector2 dir = (localStart - localEnd).normalized;
        List<Vector2> positions = new() { localEnd };

        // Максимально допустимое количество промежуточных точек (исключая начало и конец)
        int maxIntermediatePoints = _points.Count - 2;
        int intermediateCount = 0;

        while (intermediateCount < maxIntermediatePoints && remainingDistance > maxStep) {
            Vector2 prevPos = positions[^1];
            Vector2 nextPos = prevPos + dir * maxStep;
            positions.Add(nextPos);
            remainingDistance -= maxStep;
            intermediateCount++;
        }

        // Если все промежуточные точки использованы, а расстояние ещё большое - равномерно распределяем все точки между началом и концом
        bool shouldDistributeEvenly = (intermediateCount == maxIntermediatePoints && remainingDistance > maxStep);
        if (shouldDistributeEvenly) {
            positions.Clear();
            for (int i = 0; i < _points.Count; i++) {
                float t = (float)i / (_points.Count - 1);
                positions.Add(Vector2.Lerp(localStart, localEnd, t));
            }
        }
        else {
            // Если всё нормально — добавляем стартовую точку и переворачиваем список, чтобы он шёл от начала к концу
            positions.Add(localStart);
            positions.Reverse();
        }

        // Позиции, скейл, прозрачность
        for (int i = 0; i < _points.Count; i++) {
            if (i < positions.Count) {
                _points[i].gameObject.SetActive(true);
                var rect = _points[i].rectTransform;
                rect.anchoredPosition = positions[i];

                // Скейл
                float t = (_points.Count == 1) ? 1f : (float)i / (_points.Count - 1);
                float scale = Mathf.Lerp(_minScale, 1f, t);
                rect.localScale = Vector3.one * scale;

                // Прозрачность
                float distanceToStart = Vector2.Distance(positions[i], localStart);
                float alphaStep = maxStep * _alphaDistanceScale;
                float alpha = (distanceToStart < alphaStep) ? Mathf.Lerp(_minAlpha, 1f, distanceToStart / alphaStep) : 1f;

                var color = _points[i].color;
                color.a = alpha;
                _points[i].color = color;
            }
            else {
                _points[i].gameObject.SetActive(false);
            }
        }
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }
}
