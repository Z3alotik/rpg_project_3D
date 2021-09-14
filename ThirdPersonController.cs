using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonController : MonoBehaviour
{
    public Animator animator;
    public float walkSpeed;
    public float runSpeed;

    public float damage;
    public GameObject damageArea;

    public float hp;
    public float maxHealth;
    public Slider slider;
    public GameObject healthBarUI;
    public Enemy_Cyclops takeEnemyDamage;

    void Start()
    {
        animator = GetComponent<Animator>();
        hp = maxHealth;
        slider.value = CalculateHP();
        takeEnemyDamage = takeEnemyDamage.GetComponent<Enemy_Cyclops>();
        healthBarUI.SetActive(true);
    }

    void Update()
    {
        slider.value = CalculateHP();
        if (hp > maxHealth)
        {
            hp = maxHealth;
        }

        if (enabled)
        {
            //Get the horizontal and vertical movement input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            //Move the player 
            Vector3 movement = new Vector3(horizontal, 0f, vertical) * walkSpeed * Time.deltaTime;
            transform.Translate(movement, Space.Self);
            
            //Animate the player
            if (vertical != 0)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {        
                    walkSpeed = runSpeed;
                    animator.SetBool("isRunning", true);
                    animator.SetBool("isWalking", false);                       
                }
                else
                {
                    walkSpeed = 2;
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isWalking", true);
                }
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                damage = 10f;
                animator.SetBool("isAttacking", true);
            }
            else
            {
                animator.SetBool("isAttacking", false);
            }

            if (Input.GetMouseButtonDown(1))
            {
                damage = 20f;
                animator.SetBool("isStrongAttacking", true);

            }
            else
            {

                animator.SetBool("isStrongAttacking", false);

            }
        }
    }

    float CalculateHP()
    {
        return hp / maxHealth;
    }

    public void TakeEnemyDamage()
    {
        hp -= takeEnemyDamage.enemyDamage;
        if (hp <= 0)
        {
            animator.SetBool("isDying", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "EnemyDamageArea")
        {
            TakeEnemyDamage();
        }
    }

}
