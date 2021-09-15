using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    NavMeshAgent navMeshAgent;

    public GameObject Bullet;
    public TextMesh healthText;
    public float health = 100;
    public int criticalDamageChance = 20;
    public int missChance = 10;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        healthText.transform.LookAt(Camera.main.transform);
    }

    // Update is called once per frame
    void Update()
    {
        healthText.transform.LookAt(Camera.main.transform);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

       if( Input.GetMouseButton(0))
       {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit raycastHit;

           if( Physics.Raycast(ray, out raycastHit))
           {
                
                navMeshAgent.SetDestination(raycastHit.point);
           }
       }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            var criticalDmgRandomNum = UnityEngine.Random.Range(1, 100);
            var missRandomNum = UnityEngine.Random.Range(1, 100);
            var damage = UnityEngine.Random.Range(5, 10);

            if (criticalDmgRandomNum <= criticalDamageChance)
            {
                
                damage *= 2;
            }

            if (missRandomNum <= missChance)
            {
              
            }
            else
            {
                TakeDamage(damage);
            }

            Destroy(collision.gameObject);
        }
    }

    private void Shoot()
    {
        var bulletPosition = transform.position + transform.forward * 1.5f;
        bulletPosition.y = 1.5f;

        var bulletGameObject = Instantiate(Bullet, bulletPosition, Quaternion.identity);
        var bulletRb = bulletGameObject.GetComponent<Rigidbody>();
        bulletRb.velocity = transform.forward * 20f;
    }

    public void TakeDamage(float damage)
    {
        
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
        Mathf.Round(health);
        healthText.text = health.ToString("f0");
    }
}
