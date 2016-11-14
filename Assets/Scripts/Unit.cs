using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    public UnitState unitState;
    public UnitAttackType unitAttackType;
    public UnitFaction unitFaction;

    public float unitMoveSpeed;
    public float unitAttackSpeed;
    public float unitAttackRange;
    public float unitAttackDamage;

    public GameObject unitBorder;

    private float rotateSpeed = 4;
    private bool moveSequence;
    private Vector3 targetPosition;

    private Coroutine lookRoutine;

    void FixedUpdate()
    {
        if (moveSequence)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * unitMoveSpeed);

            if(transform.position == targetPosition)
            {
                unitState = UnitState.Idle;
                moveSequence = false;
            }
        }
    }

    public void Move(Vector3 targetPos)
    {
        Stop();

        targetPos.y = transform.position.y;
        targetPosition = targetPos;
        moveSequence = true;
        unitState = UnitState.Move;

        lookRoutine = StartCoroutine(LookSequence(targetPos));
    }

    public void Attack(Unit targetUnit)
    {
        Stop();
        float distanceBetweenUnits = Vector3.Distance(targetUnit.transform.position, transform.position);
        if (unitAttackRange > distanceBetweenUnits)
        {
            Debug.Log("Attack");
        }
        else
        {
            Vector3 vectorBetweenUnits = targetUnit.transform.position - transform.position;
            vectorBetweenUnits /= distanceBetweenUnits;
            vectorBetweenUnits *= (distanceBetweenUnits - unitAttackRange);

            Move(transform.position + vectorBetweenUnits);
        }
    }

    public void Stop()
    {
        targetPosition = transform.position;
        moveSequence = false;
        unitState = UnitState.Idle;

        if (lookRoutine != null)
        {
            StopCoroutine(lookRoutine);
        }
    }

    public void Enable()
    {
        unitBorder.SetActive(true);
    }

    public void Disable()
    {
        unitBorder.SetActive(false);
    }

    IEnumerator LookSequence(Vector3 targetPos)
    {
        Quaternion startRotation = transform.rotation;

        Vector3 lookDirection = targetPos - transform.position;
        lookDirection.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);

        for (float i = 0; i <= 1; i += Time.deltaTime * rotateSpeed)
        {
            transform.rotation = Quaternion.Slerp(startRotation, lookRotation, i);
            yield return null;
        }
    }
}

public enum UnitState
{
    Idle,
    Move,
    Attack
}

public enum UnitAttackType
{
    Melee,
    Ranged
}

public enum UnitFaction
{
    Ally,
    Enemy
}
