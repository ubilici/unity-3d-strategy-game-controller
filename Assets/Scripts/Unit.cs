using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    public UnitState unitState;
    public UnitAttackType unitAttackType;
    public UnitFaction unitFaction;

    public float unitMoveSpeed;
    public float unitAttackCooldown;
    public float unitAttackRange;
    public float unitAttackDamage;

    public GameObject unitBorder;
    public GameObject weapon;

    private Unit targetEnemy;
    private float rotateSpeed = 4;
    private Vector3 targetPosition;
    private bool moveSequence;
    private bool attackSequence;
    private float attackCooldown;
    private Coroutine lookRoutine;

    void Start()
    {
        attackCooldown = unitAttackCooldown;
    }

    void FixedUpdate()
    {
        if (moveSequence)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * unitMoveSpeed);

            if (transform.position == targetPosition)
            {
                unitState = UnitState.Idle;
                if (attackSequence)
                {
                    Attack(targetEnemy);
                }
                moveSequence = false;
            }
        }

        attackCooldown += Time.deltaTime;
        if (attackCooldown > unitAttackCooldown)
        {
            if (attackSequence)
            {
                Attack(targetEnemy);
                attackCooldown = 0;
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

    void AttackMove(Vector3 targetPos)
    {
        targetPos.y = transform.position.y;
        targetPosition = targetPos;
        moveSequence = true;
        unitState = UnitState.Move;

        lookRoutine = StartCoroutine(LookSequence(targetPos));
    }

    public void SelectTarget(Unit targetUnit)
    {
        targetEnemy = targetUnit;
        attackSequence = true;
    }

    void Attack(Unit targetUnit)
    {
        StopMove();

        lookRoutine = StartCoroutine(LookSequence(targetEnemy.transform.position));

        targetEnemy = targetUnit;
        float distanceBetweenUnits = Vector3.Distance(targetUnit.transform.position, transform.position);
        attackSequence = true;

        if (unitAttackRange > distanceBetweenUnits)
        {
            if (unitAttackType == UnitAttackType.Ranged)
            {
                weapon.GetComponent<RangedWeapon>().Shoot(targetUnit.transform.position);
            }
            attackCooldown = 0;
        }
        else
        {
            Vector3 vectorBetweenUnits = targetUnit.transform.position - transform.position;
            vectorBetweenUnits /= distanceBetweenUnits;
            vectorBetweenUnits *= (distanceBetweenUnits - unitAttackRange + 0.5f);

            AttackMove(transform.position + vectorBetweenUnits);
        }
    }

    public void Stop()
    {
        StopAttack();
        StopMove();

        if (lookRoutine != null)
        {
            StopCoroutine(lookRoutine);
        }
    }

    void StopAttack()
    {
        targetEnemy = null;
        attackSequence = false;

        unitState = UnitState.Idle;
    }

    void StopMove()
    {
        targetPosition = transform.position;
        moveSequence = false;

        unitState = UnitState.Idle;
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
