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
    private float attackCooldown;
    private bool attackAfterMove;
    private Vector3 targetPosition;
    private Coroutine lookRoutine;

    void Start()
    {
        attackCooldown = unitAttackCooldown;
        attackAfterMove = false;
    }

    void FixedUpdate()
    {
        if (unitState == UnitState.Move)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * unitMoveSpeed);

            if (transform.position == targetPosition)
            {
                unitState = UnitState.Idle;
                if (attackAfterMove)
                {
                    unitState = UnitState.Attack;
                }
            }
        }

        if (unitState == UnitState.Attack)
        {
            float distanceBetweenUnits = Vector3.Distance(targetEnemy.transform.position, transform.position);

            if (unitAttackRange > distanceBetweenUnits)
            {
                if (attackCooldown >= unitAttackCooldown)
                {
                    Attack();
                }
            }
            else
            {
                Vector3 vectorBetweenUnits = targetEnemy.transform.position - transform.position;
                vectorBetweenUnits /= distanceBetweenUnits;
                vectorBetweenUnits *= (distanceBetweenUnits - unitAttackRange + 0.5f);

                AttackMove(transform.position + vectorBetweenUnits);
            }
        }

        attackCooldown += Time.deltaTime;
    }

    public void Move(Vector3 targetPos)
    {
        Stop();

        targetPos.y = transform.position.y;
        targetPosition = targetPos;
        unitState = UnitState.Move;

        Look(targetPos);
    }

    void AttackMove(Vector3 targetPos)
    {
        StopMove();

        targetPos.y = transform.position.y;
        targetPosition = targetPos;
        unitState = UnitState.Move;
        attackAfterMove = true;

        Look(targetPos);
    }

    public void SelectTarget(Unit targetUnit)
    {
        targetEnemy = targetUnit;
        Look(targetEnemy.transform.position);
        unitState = UnitState.Attack;
    }

    void Attack()
    {
        Look(targetEnemy.transform.position);

        if (unitAttackType == UnitAttackType.Ranged)
        {
            weapon.GetComponent<RangedWeapon>().Shoot(targetEnemy.transform.position);
        }
        attackCooldown = 0;
    }

    public void Stop()
    {
        StopAttack();
        StopMove();
    }

    void StopAttack()
    {
        targetEnemy = null;
        unitState = UnitState.Idle;

        attackAfterMove = false;
    }

    void StopMove()
    {
        targetPosition = transform.position;
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

    void Look(Vector3 targetPos)
    {
        if (lookRoutine != null)
        {
            StopCoroutine(lookRoutine);
        }

        lookRoutine = StartCoroutine(LookSequence(targetPos));
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
