using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class GunController : MonoBehaviour
{
    public RaycastHitEvent OnFireHit;

    public Transform firePoint;
    public LayerMask HitMask;

    public int Ammo {
        get { return _ammo; }
        }

    private Transform _fireParticle;
    private float _fireDistance, _fireSpread, _fireSpeed, _reloadTime;
    private int _bulletPerShot, _maxAmmo, _ammo;
    private bool _reloadLock, _fireLock;

    public void Fire()
    {
        if (_reloadLock || _ammo == 0 || _fireLock) return;
        for (int i = 0; i < _bulletPerShot; i++)
        {
            RaycastHit hit;
            Vector2 spread = new Vector2(Random.Range(-_fireSpread / 2, _fireSpread / 2), Random.Range(-_fireSpread / 2, _fireSpread / 2));
            Vector3 raycastDirection = new Vector3(Mathf.Sin((firePoint.rotation.eulerAngles.y + spread.x)/ 180 * Mathf.PI),
                spread.y / 90,
                Mathf.Cos((firePoint.rotation.eulerAngles.y + spread.x) / 180 * Mathf.PI)).normalized;
            Physics.Raycast(firePoint.position, raycastDirection, out hit, _fireDistance, HitMask);
            Transform particle = Instantiate(_fireParticle);
            if (hit.collider != null)
            {
                float distance = Vector3.Distance(firePoint.position, hit.point);
                OnFireHit?.Invoke(hit);
                particle.transform.localScale = new Vector3(particle.localScale.x, particle.localScale.y, distance);
                particle.rotation = Quaternion.Euler(-Mathf.Asin(raycastDirection.y / raycastDirection.magnitude) * 180 / Mathf.PI,
                    firePoint.rotation.eulerAngles.y + spread.x,
                    0);
                particle.position = firePoint.position + raycastDirection * distance / 2;
            }
            else
            {
                Vector3 offset = raycastDirection * _fireDistance / 2;
                particle.transform.localScale = new Vector3(particle.localScale.x, particle.localScale.y, _fireDistance);
                particle.rotation = Quaternion.Euler(-Mathf.Asin(raycastDirection.y / raycastDirection.magnitude) * 180 / Mathf.PI,
                    firePoint.rotation.eulerAngles.y + spread.x,
                    0);
                particle.position = firePoint.position + offset;
            }
            StartCoroutine(ParticleDestroy(particle));
        }
        _ammo--;
        StartCoroutine(FireDelay());
    }

    public void ChangeParameters(GunScriptableObject gun)
    {
        _fireParticle = gun.fireParticle;
        _fireDistance = gun.fireDistance;
        _fireSpeed = gun.fireSpeed;
        _fireSpread = gun.fireSpread;
        _reloadTime = gun.reloadTime;
        _bulletPerShot = gun.bulletPerShot;
        _maxAmmo = gun.maxAmmo;
        _ammo = _maxAmmo;
    }

    public void Reload()
    {
        if (!_reloadLock)
        {
            StartCoroutine(UpdateAmmo());
        }
    }

    private IEnumerator UpdateAmmo()
    {
        _reloadLock = true;
        yield return new WaitForSeconds(_reloadTime);
        _ammo = _maxAmmo;
        _reloadLock = false;
    }

    private IEnumerator FireDelay()
    {
        _fireLock = true;
        yield return new WaitForSeconds(1 / _fireSpeed);
        _fireLock = false;
    }

    private IEnumerator ParticleDestroy(Transform particle)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(particle.gameObject);
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
