using UnityEngine;

public static class ClickTracking
{
    public static bool ClickStart {
        get {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                return true;

            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
                return true;

            return false;
        }
    }

    public static bool ClickHold {
        get {
            if (Input.GetKey(KeyCode.Mouse0))
                return true;

            if (Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Stationary))
                return true;

            return false;
        }
    }

    public static bool ClickEnd {
        get {
            if (Input.GetKeyUp(KeyCode.Mouse0))
                return true;

            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
                return true;

            return false;
        }
    }

    public static Vector2 ClickPosition {
        get {
            if (Input.touchCount > 0)
                return Input.touches[0].position;

            return Input.mousePosition;
        }
    }

    //public static Vector2 ClickPositionDelta {
    //    get {
    //        return Input.mousePositionDelta;
    //    }
    //}
}

