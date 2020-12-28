using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public GameObject vfx_hit;
	private static GameObject ProjectilesHolder;
	[HideInInspector] public float damage;
	public float speed = 3f;
	private float progress = 0f;
	private float percentOfColliderArea = 0.1f;
	private Vector3 initialPosition;
	private Vector3 pointOnTarget;
	private Vector3 shootDirection;
	private float maxFlyDistanse = 200;

	private void Awake()
	{
		initialPosition = transform.position;
		if(ProjectilesHolder == null)
		{
			ProjectilesHolder = new GameObject("ProjectilesHolder");
		}

		gameObject.transform.SetParent(ProjectilesHolder.transform);
	}

	public void SetTarget(Spawnable target)
	{
		Collider targetCollider;
		bool collExist = target.transform.TryGetComponent<Collider>(out targetCollider);
		pointOnTarget = collExist ? RandomPointInCollider(targetCollider) : target.transform.position;
		shootDirection = (pointOnTarget - transform.position).normalized;
	}

	public float Move()
	{
		progress += Time.deltaTime * speed;
		transform.position = Vector3.Lerp(initialPosition, pointOnTarget, progress);

		return progress;
	}

	public void MoveToDirection()
	{
		transform.position += Time.deltaTime * shootDirection * speed;
	}
	private void FixedUpdate()
	{
		MoveToDirection();

		if (Vector3.Distance(initialPosition, transform.position) > maxFlyDistanse)
			Destroy(gameObject);
	}


	private void OnCollisionEnter(Collision collision)
	{
		switch (collision.gameObject.tag)
		{
			case "Projectile":
				return;
			case "Enemy":
				if (collision.gameObject.TryGetComponent<Spawnable>(out Spawnable shooted))
					shooted.SufferDamage(damage);
				break;
			case "Environment":
				break;
			default:
				return;
		}
		DestroyProjectile();
	}



	private void DestroyProjectile()
	{
		GameObject.Instantiate(vfx_hit, transform.position, Quaternion.Inverse(transform.rotation), ProjectilesHolder.transform);
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
