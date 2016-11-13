using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    public UnitState unitState;
    public UnitAttackType unitAttackType;
    public UnitFaction unitFaction;

    public float unitMoveSpeed;
    public float unitAttackSpeed;
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

    public void Move(Vector3 _targetPos)
    {
        targetPosition = _targetPos + new Vector3(0, transform.position.y);
        moveSequence = true;
        unitState = UnitState.Move;
    }

    void Attack()
    {

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
