using GamePush;
using UnityEngine;
using UnityEngine.UI;

public class PlatformSwitcher : MonoBehaviour
{
    public static PlatformSwitcher Instance { get; private set; }
    public bool IsMobilePlatform { get; private set; }

    [field: SerializeField] public Joystick MoveJoystick { get; private set; }
    [field: SerializeField] public CustomButton BulletTime { get; private set; }
    [field: SerializeField] public CustomButton MoveButton { get; private set; }

    [SerializeField] private GameObject _mobileController;

    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        GP_Init.OnReady += OnPluginReady;
        IsMobilePlatform = GP_Device.IsMobile();
        if (IsMobilePlatform) {
            _mobileController.gameObject.SetActive(true);
        }
    }

    private void OnPluginReady() {
        Debug.Log("Plugin ready");
    }
}