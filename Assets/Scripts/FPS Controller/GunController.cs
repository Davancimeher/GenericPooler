using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : SingletonMB<GunController>
{
   [SerializeField] private Animator PlayerAnimator;

    public bool IsAiming;
    public float FireRate;

    [SerializeField] private float FireRateValue;
    [SerializeField] private Transform Projectile;  

    private Pooler Pooler;

    public Vector3 shootDirection;

    private void Start()
    {
        Pooler = Pooler.Instance;
        FireRateValue = FireRate;
    }
    private void DoAim()
    {
        if (!IsAiming)
        {
            PlayerAnimator.SetBool("Aiming", true);
            IsAiming = true;
        }
    }
    private void RemoveAim()
    {
        if (IsAiming)
        {
            PlayerAnimator.SetBool("Aiming", false);
            IsAiming = false;

        }
    }
    private void Shoot()
    {
        if (IsAiming)
        {
            FireRateValue -= Time.deltaTime;

            if(FireRateValue <= 0)
            {
                Debug.Log("shoot bullet");
                FireRateValue = FireRate;
                shootDirection = Projectile.transform.forward;
                Pooler.UpdateFromPool("Bullet", 1, Projectile.transform.position);

            }
        }
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            DoAim();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            RemoveAim();
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Shoot();
        }
    }
}
