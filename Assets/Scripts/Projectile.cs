using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;

    public void Shoot(Vector3 targetPos)
    {
        StartCoroutine(MoveSequence(targetPos));
    }

    IEnumerator MoveSequence(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;

        for (float t = 0; t <= 1; t += Time.deltaTime * projectileSpeed)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        GameObject.Destroy(gameObject);
    }
}
