using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    [HideInInspector] public int currentAmmo = 0;
    private float nextTimeToFire = 0f;
    public InputActionReference fire;
    public InputActionReference reload;


    private bool isReloading = false;

    void Start()
    {
        
        currentAmmo = weaponData.magazineSize;
    }

    public virtual void FixedUpdate(){}

    public void TryReload()
    {
        if (!isReloading && currentAmmo < weaponData.magazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log(weaponData.weaponName + "is reloading...");
        yield return new WaitForSeconds(weaponData.reloadTime);

        currentAmmo = weaponData.magazineSize;
        isReloading = false;

        Debug.Log(weaponData.weaponName + "is reloaded.");

    }

    public void TryShoot()
    {
        if (!isReloading && currentAmmo > 0 && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1 / weaponData.fireRate);
            HandleShoot();
        }

    }

    private void HandleShoot()
    {
        currentAmmo--;
        Debug.Log(weaponData.weaponName + "Shooted a bullet.");
        Shoot();
    }

    public abstract void Shoot();

    
}
