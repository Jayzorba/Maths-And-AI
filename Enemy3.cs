using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Enemy3 : MonoBehaviour
//{
//    public enum EnemyStates
//    {
//        Patrolling,
//        Chasing,
//        Attacking
//    }

//    public int criticalDamageChance = 20;
//    public int missChance = 10;
//    int health = 100;
//    public bool playerInSight = false;
//    public bool hasChangedTarget = false;
//    float timeBetweenShots = 0;
//    float shootspeed = 1.2f;
//    public float chaseRange = 4f;
//    public float attackRange = 3f;

//    EnemyStates currentState = EnemyStates.Patrolling;

//    public TextMesh healthText;
//    public TextMesh StateText;
//    public TextMesh IDText;
//    public GameObject Bullet;
//    public GameObject player;
//    public GameObject GridDataTemp;
//    public GameObject Algorithms;
//    public GridGenerator GridData;
//    public Dijkstra dijkstra;

//    GameObject enemyTemp;
//    GameObject enemy2Temp;
//    Enemy enemy;
//    Enemy2 enemy2;


//    // Start is called before the first frame update
//    void Start()
//    {
//        healthText.transform.LookAt(Camera.main.transform);
//        StateText.transform.LookAt(Camera.main.transform);
//        IDText.transform.LookAt(Camera.main.transform);

//        GridDataTemp = GameObject.FindGameObjectWithTag("GridGenerator");
//        Algorithms = GameObject.FindGameObjectWithTag("Algorithms");
//        GridData = GridDataTemp.GetComponent<GridGenerator>();
//        dijkstra = Algorithms.GetComponent<Dijkstra>();

//        enemyTemp = GameObject.FindGameObjectWithTag("Enemy");
//        enemy2Temp = GameObject.FindGameObjectWithTag("Enemy2");
//        enemy = enemy.GetComponent<Enemy>();
//        enemy2 = enemy2.GetComponent<Enemy2>();

//        player = GridData.playerInstance;

//        dijkstra.Search();
//    }

//    private void Update()
//    {
//        healthText.transform.LookAt(Camera.main.transform);
//        StateText.transform.LookAt(Camera.main.transform);
//        IDText.transform.LookAt(Camera.main.transform);

//        timeBetweenShots += Time.deltaTime;

//        if (currentState == EnemyStates.Patrolling)
//        {
//            StateText.text = "State: Patrolling";
//            Patrolling();
//        }
//        else if (currentState == EnemyStates.Chasing)
//        {
//            StateText.text = "State: Chasing";
//            Chasing();
//        }
//        else if (currentState == EnemyStates.Attacking)
//        {
//            StateText.text = "State: Attacking";
//            Attack();
//        }

//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.tag == "Bullet")
//        {
//            var criticalDmgRandomNum = Random.Range(1, 100);
//            var missRandomNum = Random.Range(1, 100);
//            var damage = Random.Range(5, 10);

//            if (criticalDmgRandomNum <= criticalDamageChance)
//            {

//                damage *= 2;
//            }

//            if (missRandomNum <= missChance)
//            {

//            }
//            else
//            {
//                TakeDamage(damage);
//            }

//            Destroy(collision.gameObject);
//        }

//    }


//    private void Shoot()
//    {
//        transform.LookAt(player.transform);

//        var bulletPosition = transform.position + transform.forward * 1.5f;
//        bulletPosition.y = 1.5f;

//        var bulletGameObject = Instantiate(Bullet, bulletPosition, Quaternion.identity);
//        var bulletRb = bulletGameObject.GetComponent<Rigidbody>();
//        bulletRb.velocity = transform.forward * 20f;
//    }

//    private void TakeDamage(int damage)
//    {

//        health -= damage;

//        if (health <= 0)
//        {
//            Destroy(gameObject);
//        }

//        healthText.text = ("HP: " + health.ToString());
//    }



//    private void Patrolling()
//    {
//        if (!hasChangedTarget)
//        {
//            ChangeDirection(GridData.OriginalEndPosition3, GridData.OriginalStartPosition3);
//            dijkstra.Search();
//            hasChangedTarget = true;
//        }

//        var nextCell = GridData.EnemyPath3[GridData.pathIndex3];
//        var nextCellFinal = new Vector3(nextCell.x, 1, nextCell.z);


//        transform.position = Vector3.MoveTowards(transform.position, nextCellFinal, 2 * Time.deltaTime);
//        transform.LookAt(nextCellFinal);

//        if (transform.position == nextCellFinal)
//        {
//            GridData.pathIndex3--;
//        }

//        if (GridData.pathIndex3 < 0)
//        {
//            GridData.EnemyPath3.Reverse();
//            GridData.pathIndex3 = GridData.EnemyPath3.Count - 1;
//        }

//        if (Vector3.Distance(transform.position, player.transform.position) < chaseRange)
//        {
//            currentState = EnemyStates.Chasing;
//        }
//    }

//    private void Chasing()
//    {
//        enemy.currentState = Enemy.EnemyStates.Chasing;
//        enemy2.currentState = Enemy2.EnemyStates.Chasing;

//        //Round Positions so they are available for walkable cells dictionary
//        float playerPositionX = Mathf.Round(player.transform.position.x);
//        float playerPositionZ = Mathf.Round(player.transform.position.z);
//        float enemyPositionX = Mathf.Round(transform.position.x);
//        float enemyPositionZ = Mathf.Round(transform.position.z);

//        //compute new positions
//        Vector3 finalPosEnemy = new Vector3(enemyPositionX, 0, enemyPositionZ);
//        Vector3 finalPosPlayer = new Vector3(playerPositionX, 0, playerPositionZ);

//        //change the target and rerun algorithm
//        ChangeDirection(finalPosPlayer, finalPosEnemy);
//        dijkstra.Search();


//        var nextCell = GridData.EnemyPath3[GridData.pathIndex3];
//        var nextCellFinal = new Vector3(nextCell.x, 1, nextCell.z);



//        transform.position = Vector3.MoveTowards(transform.position, nextCellFinal, 2 * Time.deltaTime);
//        transform.LookAt(nextCellFinal);

//        if (transform.position == nextCellFinal)
//        {
//            GridData.pathIndex3--;
//        }

//        hasChangedTarget = false;

//        if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
//        {
//            currentState = EnemyStates.Attacking;
//        }
//        if (Vector3.Distance(transform.position, player.transform.position) > chaseRange)
//        {
//            currentState = EnemyStates.Patrolling;
//        }
//    }

//    private void Attack()
//    {
//        if (timeBetweenShots >= shootspeed)
//        {
//            transform.LookAt(player.transform);
//            Shoot();
//            timeBetweenShots = 0;
//        }

//        if (Vector3.Distance(transform.position, player.transform.position) > attackRange)
//        {
//            currentState = EnemyStates.Chasing;
//        }
//    }

//    public void ChangeDirection(Vector3 playerPos, Vector3 enemyPos)
//    {
//        var newStartPosX = enemyPos.x;
//        var newStartPosZ = enemyPos.z;

//        var newEndPosX = playerPos.x;
//        var newEndPosZ = playerPos.z;

//        GridData.StartPosition3 = new Vector3(newStartPosX, 0, newStartPosZ);
//        GridData.EndPosition3 = new Vector3(newEndPosX, 0, newEndPosZ);


//    }
//}

public class Enemy3: MonoBehaviour
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
public float attackRange = 4f;

public EnemyStates currentState = EnemyStates.Patrolling;

public TextMesh healthText;
public TextMesh StateText;
public TextMesh IDText;
public GameObject Bullet;
public GameObject player;
public GameObject GridDataTemp;
public GameObject Algorithms;
public GridGenerator GridData;
public Dijkstra dijkstra;


   public GameObject enemyTemp;
   public GameObject enemy2Temp;
   public  Enemy enemy;
   public  Enemy2 enemy2;

    // Start is called before the first frame update
    void Start()
{
    healthText.transform.LookAt(Camera.main.transform);
    StateText.transform.LookAt(Camera.main.transform);
    IDText.transform.LookAt(Camera.main.transform);

    GridDataTemp = GameObject.FindGameObjectWithTag("GridGenerator");
    Algorithms = GameObject.FindGameObjectWithTag("Algorithms");
    GridData = GridDataTemp.GetComponent<GridGenerator>();
    dijkstra = Algorithms.GetComponent<Dijkstra>();

    enemyTemp = GameObject.FindGameObjectWithTag("Enemy");
    enemy2Temp = GameObject.FindGameObjectWithTag("Enemy2");
    enemy = enemyTemp.GetComponent<Enemy>();
    enemy2 = enemy2Temp.GetComponent<Enemy2>();

    player = GridData.playerInstance;

    dijkstra.Search();
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
    if (collision.gameObject.tag == "Bullet")
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

    if (health <= 0)
    {
        Destroy(gameObject);
    }

    healthText.text = ("HP: " + health.ToString());
}



private void Patrolling()
{
    if (!hasChangedTarget)
    {
        ChangeDirection(GridData.OriginalEndPosition3, GridData.OriginalStartPosition3);
        dijkstra.Search();
        hasChangedTarget = true;
    }

    var nextCell = GridData.EnemyPath3[GridData.pathIndex3];
    var nextCellFinal = new Vector3(nextCell.x, 1, nextCell.z);


    transform.position = Vector3.MoveTowards(transform.position, nextCellFinal, 2 * Time.deltaTime);
    transform.LookAt(nextCellFinal);

    if (transform.position == nextCellFinal)
    {
        GridData.pathIndex3--;
    }

    if (GridData.pathIndex3 < 0)
    {
        GridData.EnemyPath3.Reverse();
        GridData.pathIndex3 = GridData.EnemyPath3.Count - 1;
    }

    if (Vector3.Distance(transform.position, player.transform.position) < chaseRange)
    {
        currentState = EnemyStates.Chasing;
    }
}

private void Chasing()
{
        if(enemy.currentState == Enemy.EnemyStates.Patrolling)
        {
            enemy.currentState = Enemy.EnemyStates.Chasing;
        }
        if(enemy2.currentState == Enemy2.EnemyStates.Patrolling)
        {
            enemy2.currentState = Enemy2.EnemyStates.Chasing;
        }


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
    dijkstra.Search();


    var nextCell = GridData.EnemyPath3[GridData.pathIndex3];
    var nextCellFinal = new Vector3(nextCell.x, 1, nextCell.z);



    transform.position = Vector3.MoveTowards(transform.position, nextCellFinal, 2 * Time.deltaTime);
    transform.LookAt(nextCellFinal);

    if (transform.position == nextCellFinal)
    {
        GridData.pathIndex--;
    }

    hasChangedTarget = false;

    if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
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
        if (enemy.currentState == Enemy.EnemyStates.Patrolling)
        {
            enemy.currentState = Enemy.EnemyStates.Chasing;
        }
        if (enemy2.currentState == Enemy2.EnemyStates.Patrolling)
        {
            enemy2.currentState = Enemy2.EnemyStates.Chasing;
        }

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

    GridData.StartPosition3 = new Vector3(newStartPosX, 0, newStartPosZ);
    GridData.EndPosition3 = new Vector3(newEndPosX, 0, newEndPosZ);


}
}
