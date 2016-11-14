using UnityEngine;
using System.Collections;

public class RangedWeapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileExitPoint;

    public float projectileSpeed;

    public void Shoot(Vector3 targetPos)
    {
        GameObject projectile = Instantiate(projectilePrefab, projectileExitPoint.position, Quaternion.identity) as GameObject;
        projectile.GetComponent<Projectile>().Shoot(targetPos);
    }
}
