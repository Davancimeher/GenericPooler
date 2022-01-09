using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Vector3 shootDirection;
    private float MoveSpeed = 200f;

    private void OnEnable()
    {
        shootDirection = GunController.Instance.shootDirection;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
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
        if(Pooler.Instance != null) Pooler.Instance.DespawnObjectFromPool("Bullet", this.gameObject);
    }
}
