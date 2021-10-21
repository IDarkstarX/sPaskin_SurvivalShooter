using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    public UnityEngine.AI.NavMeshAgent nav;
    Rigidbody rb;

    

    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
        rb = GetComponent<Rigidbody>();
    }


    void Update ()
    {
        if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
            nav.SetDestination (player.position);
        } /*else if (enemyHealth.currentHealth <= 0)
        {
            
            StartCoroutine(knockBack());
        }
        */
        else
        {
            nav.enabled = false;
        }
    }

    /*
    IEnumerator knockBack()
    {
        nav.enabled = false;
        rb.isKinematic = false;

        rb.velocity = kBDirection * -1000;

        yield return new WaitForSeconds(2.5f);

        nav.enabled = true;
        rb.isKinematic = true;
    }
    */
}
