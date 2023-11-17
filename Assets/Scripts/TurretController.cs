using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public static TurretController instance;

    public Transform firePoint;
    public GameObject bulletPrefab;
    Vector2 Direction;

    public float shootingDelay;
    public float ShootingInterval;
    public bool isShooting;
    public float bulletForce;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        InvokeRepeating("Shoot", shootingDelay, ShootingInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Direction = new Vector2(transform.localScale.x, 0);
    }

    void Shoot()
    {
        isShooting = true;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(Direction * bulletForce * 5);
    }
}
