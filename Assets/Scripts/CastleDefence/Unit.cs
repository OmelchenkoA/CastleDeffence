using Assets.Data.ScriptableObjects;
using Assets.Scripts.CastleDefence.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Spawnable
{
    private float speed;
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private float dropCoins;

    private void Awake()
    {
        type = Spawnable.Type.Unit;

        //find references to components
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>(); //will be disabled until Activate is called
    }



    public void Init(UnitData uData)
    {
		state = States.Idle;

        faction = uData.Faction;
        health = uData.HitPoints;
        maxHealth = uData.HitPoints;
        targetType = uData.TargetType;
        attackRange = uData.AttackRange;
        attackRatio = uData.AttackRatio;
        speed = uData.Speed;
        damage = uData.DamagePerAttack;

        dropCoins = uData.DropCoins;

        navMeshAgent.speed = speed;
        navMeshAgent.enabled = true;
	    animator?.SetFloat("MoveSpeed", speed);

    }
    public override void Seek()
    {
        if (target == null)
            return;

        base.Seek();

        navMeshAgent.SetDestination(target.transform.position);
        navMeshAgent.isStopped = false;
        animator.SetBool("IsMoving", true);
    }

    //Unit has gotten to its target. This function puts it in "attack mode", but doesn't delive any damage (see DealBlow)
    public override void StartAttack()
    {
        base.StartAttack();

        navMeshAgent.isStopped = true;
        animator.SetBool("IsMoving", false);
    }

    //Starts the attack animation, and is repeated according to the Unit's attackRatio
    public override void DealBlow()
    {
        base.DealBlow();

        //animator.SetTrigger("Attack");
        FireProjectile();
        transform.forward = (target.transform.position - transform.position).normalized; //turn towards the target
    }

    public override void Stop()
    {
        base.Stop();

        navMeshAgent.isStopped = true;
        animator.SetBool("IsMoving", false);
    }

    protected override void Die()
    {
        base.Die();

        navMeshAgent.enabled = false;

        DropLoot();
        //animator.SetTrigger("IsDead");
    }

	private void DropLoot()
	{
        CurrencyManager.instance.AddCurrency((int)dropCoins);
	}
}
