using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy_Cyclops : MonoBehaviour
{
	public GameObject player;
	public ThirdPersonController takeDamage;
	public Transform areaOfAggression;
	public Transform attackingPlayerOrientation;

	public float hp;
	public float maxHealth;
	public Slider slider;
	public GameObject healthBarUI;

	public float enemyDamage;
	public GameObject damageArea;

	private NavMeshAgent nav;
	private Animator anim;
	private string state = "idle";
	private float wait = 0f;


	void Start()
	{
		nav = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		takeDamage = takeDamage.GetComponent<ThirdPersonController>();

		nav.speed = 2f;

		healthBarUI.SetActive(false);
		hp = maxHealth;
		slider.value = CalculateHP();
	}

	//check if we can see the player
	public void CheckSight()
	{
		RaycastHit rayHit;
		if (Physics.Linecast(areaOfAggression.position, player.transform.position, out rayHit))
		{
			print("hit " + rayHit.collider.gameObject.name);
			if (rayHit.collider.gameObject.name == "Player_Collider")
			{
				if (state != "attacking")
				{
					state = "chase";
					nav.speed = 3.5f;
				}
			}
		}

	}

	void Update()
	{

		slider.value = CalculateHP();
		if (hp < maxHealth)
		{
			healthBarUI.SetActive(true);
		}

		if (hp > maxHealth)
		{
			hp = maxHealth;
		}

		//idle
		if (state == "idle")
		{
			anim.SetBool("isChasing", false);
			anim.SetBool("isWalking", false);
			anim.SetBool("isAttacking", false);
			//pick a random place to walk to
			Vector3 randomPos = Random.insideUnitSphere * 10;
			NavMeshHit navHit;
			NavMesh.SamplePosition(transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);

			nav.SetDestination(navHit.position);
			state = "walk";
		}
			//go near the player
			/*if (highAlert)
			{
				NavMesh.SamplePosition(player.transform.position + randomPos, out navHit, 20f, NavMesh.AllAreas);
				//each time, lose awareness of player general position
				alertness += 5f;

				if (alertness > 10f)
				{
					highAlert = false;
					nav.speed = 1.2f;
					anim.SetBool("isHunting", false);
					anim.SetBool("isChasing", false);
					state = "idle";

					if (!EndAmbientSound.isPlaying)
					{
						EndAmbientSound.Play();
					}
				}
			}

			nav.SetDestination(navHit.position);
			state = "walk";
		}*/

			//walk
			if (state == "walk")
			{
				anim.SetBool("isWalking", true);
				if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
				{
					state = "search";
					wait = 7f;
				}
			}

			//Search
			if (state == "search")
			{
			anim.SetBool("isWalking", false);
			if (wait > 0f)
				{
					wait -= Time.deltaTime;
				}
				else
				{
					state = "idle";
				}
			}

			//chase
			if (state == "chase")
			{
				anim.SetBool("isChasing", true);
				anim.SetBool("isWalking", false);
				anim.SetBool("isAttacking", false);

				nav.destination = player.transform.position;

				//lose sight of player
				float distance = Vector3.Distance(transform.position, player.transform.position);
				if (distance > 15f)
				{
					state = "idle";
				}
				else if (nav.remainingDistance <= nav.stoppingDistance + 2f && !nav.pathPending)
				{
				state = "attacking";
				}
			}

			//Attack player
			if (state == "attacking")
			{
			anim.SetBool("isAttacking", true);
			anim.SetBool("isChasing", false);
			anim.SetBool("isWalking", false);
			anim.SetBool("isTakingDamage", false);
			transform.LookAt(attackingPlayerOrientation);
			if (state == "attacking" && nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending) {
				state = "chase";
			} 
		}

			//Taking damage
			if (state == "takingDamage")
		{
			anim.SetBool("isTakingDamage", true);
			anim.SetBool("isAttacking", false);
			anim.SetBool("isChasing", false);
			anim.SetBool("isWalking", false);
			wait = 1f;			
			state = "chase";
		}

		//Dying
		if (state == "dying")
		{
			anim.SetBool("isDying", true);
			anim.SetBool("isAttacking", false);
			anim.SetBool("isChasing", false);
			anim.SetBool("isWalking", false);
			anim.SetBool("isTakingDamage", false);
			print("Enemy has died !");
			healthBarUI.SetActive(false);
		}
	}

	public void TakeDamage()
	{
		state = "takingDamage";
		hp -= takeDamage.damage;
		if (hp <= 0)
		{
			state = "dying";
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "DamageArea")
		{
			TakeDamage();
			print("Enemy takes damage !");
		}
	}

	float CalculateHP()
	{
		return hp / maxHealth;
	}
}
