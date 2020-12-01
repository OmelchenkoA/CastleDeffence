using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public GameObject vfx_hit;
	[HideInInspector] public Spawnable target;
	[HideInInspector] public float damage;
	private float speed = 3f;
	private float progress = 0f;
	private Vector3 offset = new Vector3(0f, 1.2f, 0f);
	private Vector3 initialPosition;

	private void Awake()
	{
		initialPosition = transform.position;
	}

	public float Move()
	{
		progress += Time.deltaTime * speed;
		transform.position = Vector3.Lerp(initialPosition, target.transform.position + offset, progress);

		return progress;
	}


	private void Update()
	{
		float progressToTarget;

		if (target == null)
		{
			DestroyProjectile();
		}
		else
		{
			progressToTarget = Move();
			if (progressToTarget >= 1f)
			{
				if (target.state != Spawnable.States.Dead) //target might be dead already as this projectile is flying
				{
					float newHP = target.SufferDamage(damage);
					GameObject.Instantiate(vfx_hit, transform.position, Quaternion.Inverse(transform.rotation));
				}
				DestroyProjectile();
			}
		}
	}


	private void DestroyProjectile()
	{
		//GameObject.Instantiate(vfx_hit);
		Destroy(gameObject);
	}

}
