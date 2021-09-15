using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyStates
    {
        Patrolling,
        Chasing,
        Attacking
    }

    public int criticalDamageChance = 20;
    public int missChance = 10;
    int health = 100;
    public bool playerInSight = false;
    public bool hasChangedTarget = false;
    float timeBetweenShots = 0;
    float shootspeed = 1.2f;
    public float chaseRange = 5f;
    public float attackRange = 2f;

    public EnemyStates currentState = EnemyStates.Patrolling;

    public TextMesh healthText;
    public TextMesh StateText;
    public TextMesh IDText;
    public GameObject Bullet;
    public GameObject player;
    public GameObject GridDataTemp;
    public GameObject Algorithms;
    public GridGenerator GridData;
    public AStar aStar;


    // Start is called before the first frame update
    void Start()
    {
        healthText.transform.LookAt(Camera.main.transform);
        StateText.transform.LookAt(Camera.main.transform);
        IDText.transform.LookAt(Camera.main.transform);

        GridDataTemp = GameObject.FindGameObjectWithTag("GridGenerator");
        Algorithms = GameObject.FindGameObjectWithTag("Algorithms");
        GridData = GridDataTemp.GetComponent<GridGenerator>();
        aStar = Algorithms.GetComponent<AStar>();

        player = GridData.playerInstance;

        aStar.Search();
    }

    private void Update()
    {
        healthText.transform.LookAt(Camera.main.transform);
        StateText.transform.LookAt(Camera.main.transform);
        IDText.transform.LookAt(Camera.main.transform);

        timeBetweenShots += Time.deltaTime;

        if (currentState == EnemyStates.Patrolling)
        {
            StateText.text = "State: Patrolling";
            StateText.color = Color.green;
            Patrolling();
        }
        else if (currentState == EnemyStates.Chasing)
        {
            StateText.text = "State: Chasing";
            StateText.color = Color.yellow;
            Chasing();
        }
        else if (currentState == EnemyStates.Attacking)
        {
            StateText.text = "State: Attacking";
            StateText.color = Color.red;
            Attack();

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            var criticalDmgRandomNum = Random.Range(1, 100);
            var missRandomNum = Random.Range(1, 100);
            var damage = Random.Range(5, 10);

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
        transform.LookAt(player.transform);
        
        var bulletPosition = transform.position + transform.forward * 1.5f;
        bulletPosition.y = 1.5f;

        var bulletGameObject = Instantiate(Bullet, bulletPosition, Quaternion.identity);
        var bulletRb = bulletGameObject.GetComponent<Rigidbody>();
        bulletRb.velocity = transform.forward * 20f;
    }

    private void TakeDamage(int damage)
    {
        
        health -= damage;

        if(health<=0)
        {
            Destroy(gameObject);
        }

        healthText.text = ("HP: " + health.ToString());
    }



    private void Patrolling()
    {
        if (!hasChangedTarget)
        {
            ChangeDirection(GridData.OriginalEndPosition, GridData.OriginalStartPosition);
            aStar.Search();
            hasChangedTarget = true;
        }

        var nextCell = GridData.EnemyPath[GridData.pathIndex];
        var nextCellFinal = new Vector3(nextCell.x, 1, nextCell.z);


        transform.position = Vector3.MoveTowards(transform.position, nextCellFinal, 2 * Time.deltaTime);
        transform.LookAt(nextCellFinal);

        if (transform.position == nextCellFinal)
        {
            GridData.pathIndex--;
        }

        if (GridData.pathIndex < 0)
        {
            GridData.EnemyPath.Reverse();
            GridData.pathIndex = GridData.EnemyPath.Count - 1;
        }

        if(Vector3.Distance(transform.position,player.transform.position)<chaseRange)
        {
            currentState = EnemyStates.Chasing;
        }
    }

    private void Chasing()
    {
        //Round Positions so they are available for walkable cells dictionary
        float playerPositionX = Mathf.Round(player.transform.position.x);
        float playerPositionZ = Mathf.Round(player.transform.position.z);
        float enemyPositionX = Mathf.Round(transform.position.x);
        float enemyPositionZ = Mathf.Round(transform.position.z);

        //compute new positions
        Vector3 finalPosEnemy = new Vector3(enemyPositionX, 0, enemyPositionZ);
        Vector3 finalPosPlayer = new Vector3(playerPositionX, 0, playerPositionZ);

        //change the target and rerun algorithm
        ChangeDirection(finalPosPlayer, finalPosEnemy);
        aStar.Search();


        var nextCell = GridData.EnemyPath[GridData.pathIndex];
        var nextCellFinal = new Vector3(nextCell.x, 1, nextCell.z);



        transform.position = Vector3.MoveTowards(transform.position, nextCellFinal, 2 * Time.deltaTime);
        transform.LookAt(nextCellFinal);

        if (transform.position == nextCellFinal)
        {
            GridData.pathIndex--;
        }

        hasChangedTarget = false;

        if(Vector3.Distance(transform.position,player.transform.position) <attackRange)
        {
            currentState = EnemyStates.Attacking;
        }
        if (Vector3.Distance(transform.position, player.transform.position) > chaseRange)
        {
            currentState = EnemyStates.Patrolling;
        }
    }

    private void Attack()
    {
        if (timeBetweenShots >= shootspeed)
        {
            transform.LookAt(player.transform);
            Shoot();
            timeBetweenShots = 0;
        }

        if (Vector3.Distance(transform.position, player.transform.position) > attackRange)
        {
            currentState = EnemyStates.Chasing;
        }
    }

    public void ChangeDirection(Vector3 playerPos, Vector3 enemyPos)
    {
        var newStartPosX = enemyPos.x;
        var newStartPosZ = enemyPos.z;

        var newEndPosX = playerPos.x;
        var newEndPosZ = playerPos.z;

        GridData.StartPosition = new Vector3(newStartPosX, 0, newStartPosZ);
        GridData.EndPosition = new Vector3(newEndPosX, 0, newEndPosZ);


    }
}
