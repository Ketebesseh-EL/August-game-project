using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour

{

    public UnityEvent OnWeaponShoot;
    public bool Automatic;
    public float FireCooldown;

    public float CurrentCooldown;
    
    public InputActionReference fire;



    void Start()
    {
        CurrentCooldown = FireCooldown;
        
    }




    void Update()
    {   if (Automatic)
        {
            if (fire.action.IsPressed())
            {
                if (CurrentCooldown <= 0f)
                {
                    print("fire");
                    OnWeaponShoot?.Invoke();
                    CurrentCooldown = FireCooldown;
                }
            }
            
        }
        else
        {
            if (fire.action.WasPressedThisFrame())
            {
                if (CurrentCooldown <= 0f)
                {
                    print("fire");
                    OnWeaponShoot?.Invoke();
                    CurrentCooldown = FireCooldown;
                }
            }    
        }
        CurrentCooldown -= Time.deltaTime;
    }
}
