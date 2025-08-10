using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;

    public LayerMask targetLayerMask;
    [Header("Weapon Config")]
    public float Range;
    public float fireRate;
    public float reloadTime;
    public int magazineSize;
    public float damage;


    
}
