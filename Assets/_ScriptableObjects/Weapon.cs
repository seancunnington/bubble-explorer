using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bullet Type
    public enum BulletType
    {
        SimpleVector,
        Physics,
        StationaryMine
    };

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{

    public new string name;
    public string description;

    public BulletType bulletType;

    //Weapon VFX
    // 0 = Muzzle
    // 1 = Projectile
    // 2 = Explosion
    public List<GameObject> vfx = new List<GameObject>();

    //Weapon Stats
    public int damage;
    public float scaleSize;
    public float fireRate;
    public float speed;

}
