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

    private bool moveSequence;
    private Vector3 targetPosition;

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
        targetPos.y = transform.position.y;
        targetPosition = targetPos;
        moveSequence = true;
        unitState = UnitState.Move;
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
    }

    public void Enable()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public void Disable()
    {
        GetComponent<MeshRenderer>().material.color = Color.black;
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
