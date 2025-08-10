using UnityEngine;

public class TurnToMouse : MonoBehaviour
{
    public float smoothSpeed = 720f;
    public bool lockYOnly = true;
    public bool smooth = true;

    void Update()
    {
        Vector3 targetWorld = MousePositionManager.Instance.MouseWorldPos;
        Vector3 direction = targetWorld - transform.position;

        if (lockYOnly)
        {
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f) return;
        }

        Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);

        if (smooth)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, smoothSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = targetRot;
        }
    }
}
