using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public WeaponType weaponType;

    [Header("Ranged Weapon Only")]
    public float stabSpeed;

    [Header("Ranged Weapon Only")]
    public float projectileSpeed;
    public GameObject projectilePrefab;
    public Transform projectileExitPoint;

    public void Stab()
    {
        if (weaponType == WeaponType.Melee)
        {
            StartCoroutine(StabSequence());
        }
    }

    public void Shoot(Vector3 targetPos)
    {
        if (weaponType == WeaponType.Ranged)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileExitPoint.position, Quaternion.identity) as GameObject;
            projectile.GetComponent<Projectile>().Shoot(targetPos);
        }
    }

    IEnumerator StabSequence()
    {
        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = startPos;
        targetPos.z += 1f;

        for (float t = 0; t <= 1; t += Time.deltaTime * stabSpeed)
        {
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        targetPos = startPos;
        startPos = transform.localPosition;

        for (float t = 0; t <= 1; t += Time.deltaTime * stabSpeed)
        {
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.localPosition = targetPos;
    }
}

public enum WeaponType
{
    Melee,
    Ranged
}
