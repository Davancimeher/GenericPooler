using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public Vector3 shootDirection;
    private float MoveSpeed = 200f;
    private bool isPooled;

    private void OnEnable()
    {
        if(isPooled)
            shootDirection = GunController.Instance.Projectile.transform.forward;

    }
    private void Start()
    {
        if (!isPooled)
        {
            shootDirection = GunController.Instance.Projectile.transform.forward;
            isPooled = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            StatisticManager.Instance.UpdateUI(_prefabShooted: true);

            other.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        transform.position += shootDirection * MoveSpeed * Time.deltaTime;
    }

    private void OnDisable()
    {
        if (Pooler.Instance != null) Pooler.Instance.DespawnObjectFromPool("Bullet", this.gameObject);
    }
}
