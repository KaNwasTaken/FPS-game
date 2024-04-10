using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewGunData.asset", menuName = "ScriptableObjects/Gun")]
public class Gun : ScriptableObject
{
    public GameObject gunPrefab;
    public Vector3 defaultPosition;
    public Vector3 defaultRotation;
    public string gunName;
    public int magazineSize;
    public float fireRate;
    public AudioClip shootSound;
    public GameObject projectilePrefab;
    public int damage;
    public float range;
    public enum FireModes
    {
        automatic,
        semiAutomatic
    }
    public FireModes fireMode;
}
