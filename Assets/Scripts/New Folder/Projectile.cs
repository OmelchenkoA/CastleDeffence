using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public GameObject vfx_hit;
	private Spawnable m_target;
	[HideInInspector] public float damage;
	private float speed = 3f;
	private float progress = 0f;
	private float percentOfColliderArea = 0.1f;
	private Vector3 offset = new Vector3(0f, 1f, 0f);
	private Vector3 initialPosition;
	private Vector3 pointOnTarget;
	private Collider collider;

	private void Awake()
	{
		initialPosition = transform.position;
		collider = GetComponent<Collider>();
	}

	public void SetTarget(Spawnable target)
	{
		m_target = target;
		Collider targetCollider;
		bool collExist = target.transform.TryGetComponent<Collider>(out targetCollider);
		pointOnTarget = collExist ? RandomPointInCollider(targetCollider) : target.transform.position;
	}

	public float Move()
	{
		progress += Time.deltaTime * speed;
		transform.position = Vector3.Lerp(initialPosition, pointOnTarget, progress);

		return progress;
	}


	private void Update()
	{
		float progressToTarget;

		if (m_target == null)
		{
			Destroy(gameObject);
		}
		else
		{
			progressToTarget = Move();
			if (progressToTarget >= 1f)
			{
				if (m_target.state != Spawnable.States.Dead) //target might be dead already as this projectile is flying
				{
					
				}
				DestroyProjectile();
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject == m_target.gameObject)
			DestroyProjectile();
	}



	private void DestroyProjectile()
	{
		//GameObject.Instantiate(vfx_hit);
		float newHP = m_target.SufferDamage(damage);
		GameObject.Instantiate(vfx_hit, transform.position, Quaternion.Inverse(transform.rotation));
		Destroy(gameObject);
	}


	private Vector3 RandomPointInCollider(Collider collider)
	{
		Bounds bounds = collider.bounds;
		return new Vector3(
			Random.Range(bounds.center.x - bounds.size.x * percentOfColliderArea, bounds.center.x + bounds.size.x * percentOfColliderArea),
			Random.Range(bounds.center.y - bounds.size.y * percentOfColliderArea, bounds.center.y + bounds.size.y * percentOfColliderArea),
			Random.Range(bounds.center.z - bounds.size.z * percentOfColliderArea, bounds.center.z + bounds.size.z * percentOfColliderArea)
		);
	}

}
