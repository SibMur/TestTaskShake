using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
    [HideInInspector]
    public Camera virtualCamera;

    private RaycastHit[] hitInfo;

    [SerializeField]
    private LayerMask groundLayer;

    public static bool IsMoved { get; private set; }


    private void Start() {
        Cursor.visible = true;
        hitInfo = new RaycastHit[1];
    }

    private void OnEnable() {
        PlatformSwitcher.Instance.MoveButton.OnButtonDown += StartMove;
        PlatformSwitcher.Instance.MoveButton.OnButtonUp += EndMove;
    }
    private void OnDisable() {
        PlatformSwitcher.Instance.MoveButton.OnButtonDown -= StartMove;
        PlatformSwitcher.Instance.MoveButton.OnButtonUp -= EndMove;
    }


    private void Update() {
        if (PlatformSwitcher.Instance.IsMobilePlatform) {           
            if (IsMoved)
                Move(virtualCamera.ScreenPointToRay(ClickTracking.ClickPosition));
        }
        else {
            Move(virtualCamera.ScreenPointToRay(UnityEngine.Input.mousePosition));
        }
    }

    private void StartMove() {
        IsMoved = true;
    }
    private void EndMove() {
        IsMoved = false;
    }

    private void Move(Ray ray) {
        if (Physics.RaycastNonAlloc(ray, hitInfo, 100f, groundLayer) > 0) {
            base.transform.position = hitInfo[0].point;
        }
    }
}