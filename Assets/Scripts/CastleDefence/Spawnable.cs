using Assets.Scripts.CastleDefence.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawnable : MonoBehaviour
{
    public Type type;
    [HideInInspector] public States state = States.Idle;
    [HideInInspector] public AttackType attackType;
    [HideInInspector] public TargetType targetType;
    [HideInInspector] public Faction faction;


    [HideInInspector] public Spawnable target;
    [HideInInspector] public float health;
    [HideInInspector] public float maxHealth;
    [HideInInspector] public float attackRange;
    [HideInInspector] public float attackRatio;
    [HideInInspector] public float lastBlowTime = -1000f;
    [HideInInspector] public float damage;

    [HideInInspector] public float timeToActNext = 0f;

    [Header("Projectile for Ranged")]
    public GameObject projectilePrefab;
    public GameObject gun;
    public Transform [] projectileSpawnPoints;

    public HealthBar healthBar;
    private int _nextGunToShoot = 0;

    public UnityAction<Spawnable> OnDie, OnDealDamage, OnProjectileFired;

    public enum Type
    {
        Unit,
        Tower,
        Castle,
    }

    public enum TargetType
    {
        Castle,
        Units,
        All,
        None,
    }

    public enum AttackType
    {
        Melee,
        Ranged,
        None
    }

    public enum Faction
    {
        Player,
        Enemy,
        None
    }

    public enum States
    {
        Idle,
        Seeking,
        Attacking,
        Dead,
    }


    public virtual void SetTarget(Spawnable t)
    {
        target = t;
        t.OnDie += TargetIsDead;
    }

    public virtual void StartAttack()
    {
        state = States.Attacking;
    }

    public void FireProjectile()
    {
        gun.transform.forward = (target.transform.position - transform.position).normalized;

        Vector3 adjTargetPos = target.transform.position;
        adjTargetPos.y = 1.5f;
        Quaternion rot = Quaternion.LookRotation(adjTargetPos - projectileSpawnPoints[_nextGunToShoot].position);

        Projectile prj = Instantiate<GameObject>(projectilePrefab, projectileSpawnPoints[_nextGunToShoot].position, rot).GetComponent<Projectile>();
        prj.SetTarget(target);
        prj.damage = damage;
        _nextGunToShoot = (_nextGunToShoot + 1) % projectileSpawnPoints.Length;

        OnProjectileFired?.Invoke(this);
	}
    public virtual void Seek()
    {
        state = States.Seeking;
    }
    protected void TargetIsDead(Spawnable p)
    {
        //Debug.Log("My target " + p.name + " is dead", gameObject);
        state = States.Idle;

        target.OnDie -= TargetIsDead;

        timeToActNext = lastBlowTime + attackRatio;
    }

    public float SufferDamage(float amount)
    {
        health -= amount;
        healthBar.UpdateHealth(health, maxHealth);
        //Debug.Log("Suffering damage, new health: " + hitPoints, gameObject);
        if (state != States.Dead
            && health <= 0f)
        {
            Die();
        }

        return health;
    }
    public bool IsTargetInRange()
    {
        return (transform.position - target.transform.position).sqrMagnitude <= attackRange * attackRange;
    }

    public virtual void DealBlow()
    {
        lastBlowTime = Time.time;
    }
    protected virtual void Die()
    {
        state = States.Dead;
        //audioSource.pitch = Random.Range(.9f, 1.1f);
        //audioSource.PlayOneShot(dieAudioClip, 1f);
        if(target != null)
            target.OnDie -= TargetIsDead;
        OnDie?.Invoke(this);
	}
    public virtual void Stop()
    {
        state = States.Idle;
    }

}
