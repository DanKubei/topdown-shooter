using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "ScriptableObjects/Gun", order = 1)]
public class GunScriptableObject : ScriptableObject
{
    public Transform fireParticle;
    public float fireDistance, fireSpread, fireSpeed, reloadTime;
    public int bulletPerShot, maxAmmo;
}
