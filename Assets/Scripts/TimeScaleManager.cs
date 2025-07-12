using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    public float deadTimeScale = 0.15f;

    private bool dead;

    public float bulletTimeScale = 0.12f;

    private float bulletTimeFactor;

    public float currentTimeScale;

    private float pauseTimeScaleFactor => (!LevelManager.Paused) ? 1 : 0;

    private bool _isClicked;

    private void Awake() {
    }

    private void Start() {
    }

    private void OnEnable() {
        PlatformSwitcher.Instance.BulletTime.OnButtonDown += StartClick;
        PlatformSwitcher.Instance.BulletTime.OnButtonUp += EndClick;
    }
    private void OnDisable() {
        PlatformSwitcher.Instance.BulletTime.OnButtonDown -= StartClick;
        PlatformSwitcher.Instance.BulletTime.OnButtonUp -= EndClick;
    }

    private void Update() {
        if (PlatformSwitcher.Instance.IsMobilePlatform) {
            if(_isClicked)
                bulletTimeFactor = bulletTimeScale;
            else
                bulletTimeFactor = 1f;
        }
        else {
            if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
                bulletTimeFactor = bulletTimeScale;
            else
                bulletTimeFactor = 1f;
        }
        UpdateTimeSclae();
    }

    private void StartClick() {
        _isClicked = true;
    }
    private void EndClick() {
        _isClicked = false;
    }

    public void Dead() {
        dead = true;
    }

    public void ResetTimeScales() {
        dead = false;
    }

    private void UpdateTimeSclae() {
        Time.timeScale = (dead ? deadTimeScale : 1f) * pauseTimeScaleFactor * bulletTimeFactor;
        if (Time.timeScale > 0f) {
            Time.fixedDeltaTime = Time.timeScale * 0.0069444445f;
        }
        currentTimeScale = Time.timeScale;
    }
}
