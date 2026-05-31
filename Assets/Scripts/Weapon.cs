using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
	public Camera playerCamera;
	
	// Shoot
	public bool isShooting;
	public bool readyToShoot;
	bool allowReset = true;
    public float shootingCooldown = 0f;
    public float fireRate = 600f;
    public float burstCoolDown = 0.5f;
	public float burstDelay = 0.05f;
	
	// Burst
	public int bulletsPerBurst = 3;
	public int burstBulletsLeft;
	
	// Spread
	public float spreadIntensity;
	
	// Bullet
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletVelocity = 100;
	public float bulletPrefabLifeTime = 5f;
	
	public enum ShootingMode
	{
		Single,
		Burst,
		Auto
	}
	
	public ShootingMode currentShootingMode;
	
	private void Awake()
	{
		readyToShoot = true;
		burstBulletsLeft = bulletsPerBurst;
	}
	
	void Update()
	{
		if (currentShootingMode == ShootingMode.Auto)
		{
			// Holding Left Mouse
			isShooting = Input.GetKey(KeyCode.Mouse0);
		}
		else
		{
			// Clicking Left Mouse
			isShooting = Input.GetKeyDown(KeyCode.Mouse0);
		}
		
		if (readyToShoot && isShooting)
		{
			burstBulletsLeft = bulletsPerBurst;
			FireWeapon();
		}
	}
	
	private void FireWeapon()
	{
		readyToShoot = false;
		
		Vector3 shootingDirection = CalculateDircetionAndSpread().normalized;
		
		// Spawn Boolet
		GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
		
		bullet.transform.forward = shootingDirection;
		
		// Shoot Boolet
		bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
		
		// Destroy Boolet
		StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
		
		// Check if shot
		if (allowReset)
		{
			if (currentShootingMode == ShootingMode.Burst)
			{
				shootingCooldown = burstCoolDown;
			}
            else
            {
                shootingCooldown = 60 / fireRate;
            }
            Invoke("ResetShot", shootingCooldown);
			allowReset = false;
		}
		
		// Burst mode
		if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) // Already shot
		{
			burstBulletsLeft--;
			Invoke("FireWeapon", burstDelay);
		}
	}
	
	private void ResetShot()
	{
		readyToShoot = true;
		allowReset = true;
	}
	
	public Vector3 CalculateDircetionAndSpread()
	{
		// Shoot to where mouse is pointing
		Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		
		Vector3 targetPoint;
		if (Physics.Raycast(ray, out hit))
		{
			targetPoint = hit.point;
		}
		else
		{
			// Shot at air
			targetPoint = ray.GetPoint(100);
		}
		
		Vector3 direction = targetPoint - bulletSpawn.position;
		
		float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
		float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
		
		// Return direction + speed
		return direction + new Vector3(x, y, 0);
	}
	
	private IEnumerator DestroyBulletAfterTime(GameObject bullet, float bulletPrefabLifeTime)
	{
		yield return new WaitForSeconds(bulletPrefabLifeTime);
		Destroy(bullet);
	}
}