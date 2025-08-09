using UnityEngine;

public class testWeapon : Weapon
{
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (fire.action.IsPressed())
        {
            TryShoot();
        }
        if (reload.action.IsPressed())
        {
            TryReload();
        }
    }

    public override void Shoot()
    {
        RaycastHit hit;
        Vector3 targetPosition = MousePositionManager.Instance.MouseWorldPos;
        targetPosition.y = transform.position.y;
        Vector3 shootDirection = (targetPosition - transform.position).normalized;
        if (Physics.Raycast(transform.position, shootDirection, out hit, weaponData.Range, weaponData.targetLayerMask))
        {
            Debug.Log(weaponData.weaponName + " hit" + hit.collider.name);
            if (hit.collider.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(weaponData.damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 targetPosition = MousePositionManager.Instance.MouseWorldPos;
        targetPosition.y = transform.position.y;
        Gizmos.DrawLine(transform.position, targetPosition);
    }
}
