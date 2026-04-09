using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class jupitermovement : MonoBehaviour {
    public Rigidbody2D bat_body;
    public Animator animator, lightning_animation;
    public AudioSource thunder, casting_sound;
    public GameObject cast_bar, lightning_casting;
    public LineRenderer lightning_line;
    public TextMeshProUGUI healthText;
    public LayerMask playerLayers;
    public Transform attack_point;
    public PlayerMovement player_movement;
    public Hit hit;
    
    public float Speed;
    public float attackRange = 0.5f;
    private float playerrange;
    
    Vector2 jup_movement;
    private int jup_damage, health;
    private float latestDirectionChangeTime, latest_attack;
    private readonly float directionChangeTime = 3f, time_between_attack = 2f;
    //private readonly float cast_time = 8f;
    private int MaxDist = 180, MinDist = 180;
    private bool attack, move, cast, continue_, isDead, isCasting;
    
    GameObject player;

    void Start () {
        /*vlk = GameObject.Find("Wolf").transform;*/
        /*wolf_body = GameObject.Find("Game Object Script").GetComponent<Rigidbody2D>();*/
        latestDirectionChangeTime = 0f;
        player = GameObject.FindGameObjectWithTag("Player");
        move = true;
        cast = false;
        continue_ = true;
        isDead = false;
        lightning_line.enabled = false;        
    }
    
    private void Update() {
        if (isDead) return;
        float dis = Vector2.Distance(transform.position, player.transform.position);
        // Jupiter died
        if (hit != null && hit.GetHealth() <= 0) {
            StopJupiter();
            animator.SetFloat("speed", 0); animator.SetFloat("Horizontal", 0); animator.SetFloat("Vertical", 0);
            bat_body.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            isDead = true;
            return;
        }
        // Player died 
        if (player_movement.GetHealth() <= 0) {
            StopJupiter();
            bat_body.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            return;
        }
        // stop and prepare to cast
        if (dis <= 70f) {
            if (!isCasting) {
                //jup_movement.x = 0;
                //jup_movement.y = 0;
                move = false;
                cast = true;
                jup_movement = Vector2.zero;
                bat_body.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        } else {
            cast = false;
            if (!isCasting) {
                move = true;
                Speed = 35;
                bat_body.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
        // Roam randomly when far
        if (dis >= MinDist && move) {
            if (Time.time - latestDirectionChangeTime > directionChangeTime) {
                latestDirectionChangeTime = Time.time;
                jup_movement = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
                //movementPerSecond = jup_movement * Speed;
            }
            attack = true;
            transform.position = new Vector2(
                transform.position.x + (jup_movement.x * Speed * Time.deltaTime),
                transform.position.y + (jup_movement.y * Speed * Time.deltaTime)
            );
        }
        // Chase player
        else if (dis <= MaxDist && move) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Speed * Time.deltaTime);
            jup_movement.x = transform.position.x <= player.transform.position.x ? 1f : transform.position.x >= player.transform.position.x ? -1f : 0f;
            jup_movement.y = transform.position.y >= player.transform.position.y + 50 ? -1f : transform.position.y <= player.transform.position.y - 50 ? 1f : 0f;
        }
        animator.SetFloat("Horizontal", jup_movement.x);
        animator.SetFloat("Vertical", jup_movement.y);
        animator.SetFloat("speed", jup_movement.sqrMagnitude);
        // Attack trigger
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attack_point.position, attackRange, playerLayers);
        foreach (Collider2D p in hitPlayer) {
            if (Time.time - latest_attack >= time_between_attack && attack) {
                latest_attack = Time.time;
                if (cast && continue_) {
                    continue_ = false;
                    StartCoroutine(player_hit());
                }
            }
        }
    }

    private void StopJupiter() {
        move = false; attack = false; cast = false; isCasting = false;
        Speed = 0; jup_movement = Vector2.zero;
        animator.SetFloat("speed", 0);
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 0);
    }

    IEnumerator player_hit() {
        isCasting = true; move = false;
        Speed = 0;
        jup_movement = Vector2.zero;
        //jup_movement.x = 0;
        //jup_movement.y = 0;
        bat_body.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        // cast bar fill over 4 seconds
        cast_bar.GetComponent<health_bar>().SetMin();
        cast_bar.SetActive(true);
        lightning_casting.SetActive(true);
        lightning_animation.SetTrigger("casting_energy");
        casting_sound.Play();
        yield return new WaitForSeconds(1f);
        cast_bar.GetComponent<health_bar>().SetHealth(1);
        yield return new WaitForSeconds(1f);
        cast_bar.GetComponent<health_bar>().SetHealth(2);
        yield return new WaitForSeconds(1f);
        cast_bar.GetComponent<health_bar>().SetHealth(3);
        yield return new WaitForSeconds(1f);
        cast_bar.GetComponent<health_bar>().SetHealth(4);
        casting_sound.Stop();
        lightning_casting.SetActive(false);
        // realase lightning
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attack_point.position,attackRange,playerLayers);
        foreach(Collider2D player in hitPlayer) {
            Debug.Log("Jupiter hit " + player.name);
            jup_damage = Random.Range(50,90);
            thunder.Play();
            lightning_line.enabled = true;
            //var player_pos = player_movement.transform.position;
            //var jup_pos = transform.position;
            //Debug.Log(player_pos);
            CreateLine();
            player_movement.GetComponent<PlayerMovement>().TakeDamage(jup_damage);
        }
        yield return new WaitForSeconds(0.5f);
        cast_bar.SetActive(false);
        lightning_line.enabled = false;
        // Resume movement
        bat_body.constraints = RigidbodyConstraints2D.FreezeRotation;
        move = true;
        Speed = 35;
        isCasting = false;
        yield return new WaitForSeconds(6f);
        continue_ = true;
    }

    void CreateLine() {
        lightning_line.widthMultiplier = 0.2f;
        lightning_line.positionCount = 2;
        lightning_line.SetPositions(new Vector3[] {
            transform.position,
            player_movement.transform.position
        });
        Debug.Log("Drawed line");
    }
    
    void OnDrawGizmosSelected() {
        if(attack_point == null){ return; } 
        Gizmos.DrawWireSphere(attack_point.position,attackRange);
    }

}


