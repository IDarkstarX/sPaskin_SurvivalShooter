using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;

    public shakeController shaker;

    [SerializeField]
    Material hitMat;

    [SerializeField]
    ParticleSystem missParticles;

    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if (Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
                missParticles.Stop();

                StartCoroutine(resetMat(shootHit.collider.GetComponentInChildren<SkinnedMeshRenderer>().material, shootHit.collider.gameObject));

                shootHit.collider.GetComponentInChildren<SkinnedMeshRenderer>().material = hitMat;

                StartCoroutine(hitStun(shootHit.collider.gameObject));
            } else if (enemyHealth == null)
            {
                missParticles.transform.position = shootHit.point;
                missParticles.Play();
            }
            gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
            
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
            
        }

        shaker.start = true;
    }



    IEnumerator resetMat(Material oriM, GameObject baddie)
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(0.0525f);

        baddie.GetComponentInChildren<SkinnedMeshRenderer>().material = oriM;
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    IEnumerator hitStun(GameObject baddie)
    {
        baddie.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        yield return new WaitForSeconds(0.025f);
        baddie.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
    }
}