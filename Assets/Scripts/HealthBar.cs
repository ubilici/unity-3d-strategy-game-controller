using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public Transform remainingHealth;

    private Quaternion rotation;

    void Awake()
    {
        rotation = transform.rotation;
    }

    void LateUpdate()
    {
        transform.rotation = rotation;
    }

    public void SetRemainingHealth(float scaleHP)
    {
        remainingHealth.localScale = new Vector3(scaleHP, 1, 1);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
