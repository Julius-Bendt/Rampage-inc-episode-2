using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Inventory/Weapon", order = 2)]
public class Weapon : ScriptableObject
{
    [Header("Gun settings")]
    public int maxAmmo;
    public float fireRate, attackDist;
    public bool autoFire;
    public GameObject bullet, muzzle, gun;
    public float shakeTime = 0.05f;
    public bool destroyAfterThrow = false;

    [Header("Melee settings")]
    public bool melee;
    public float swingTime;
    public Vector3 swingRotate;
    public float move;
    public int durability = 999;

    public AudioClip[] fireClip;

    public AudioClip GetRandomClip()
    {
        return fireClip[Random.Range(0, fireClip.Length - 1)];
    }
}
