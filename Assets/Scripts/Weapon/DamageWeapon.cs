using UnityEngine;

public class DamageWeapon : MonoBehaviour
{
    public float damage;
    public float Range;
    public Transform Shooter;
    private LineRenderer lineRenderer; 

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 2; 
        lineRenderer.enabled = false; 
    }
    public void Shoot()
    {
        Vector3 targetPosition = MousePositionManager.Instance.MouseWorldPos;
        Vector3 direction = (targetPosition - Shooter.position).normalized; 
        Ray weaponRay = new Ray(Shooter.position, targetPosition);

        lineRenderer.SetPosition(0, Shooter.position);
        lineRenderer.SetPosition(1, Shooter.position + direction * Range);
        lineRenderer.enabled = true;
        Invoke(nameof(DisableLineRenderer), 1.0f);

        if (Physics.Raycast(weaponRay, out RaycastHit hitInfo, Range))
        {
            print("damaged");
            if (hitInfo.collider.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
    }
    private void DisableLineRenderer()
    {
        lineRenderer.enabled = false; 
    }
}
