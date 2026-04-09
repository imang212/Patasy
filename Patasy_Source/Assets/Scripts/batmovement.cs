using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class batmovement : MonoBehaviour {
    public Rigidbody2D bat_body;
    public Vector2 bat_movement;
    private Vector2 movementPerSecond;
    public Animator animator;
    public TextMeshProUGUI healthText;
    public LayerMask playerLayers;
    public Transform attack_point;
    public PlayerMovement player_movement;
    public Hit hit;
    
    public float Speed;
    private float playerrange;
    private int health, bat_damage;
    public float attackRange = 0.5f;
    private float latestDirectionChangeTime, latest_attack;
    private readonly float directionChangeTime = 3f, time_between_attack = 2f;
    private bool move, attack, isDead, isAttacking;

    GameObject player;
 
    void Start () {
        health = Random.Range(60, 95);
        player = GameObject.FindGameObjectWithTag("Player");
        move = true; attack = true; isDead = false;
    }
    private void Update() {
      if (isDead) return;
      float dis = Vector2.Distance(transform.position, player.transform.position);
      // bat died
      if (hit != null && hit.GetHealth() <= 0) {
          StopBat();
          bat_body.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
          isDead = true;
          return;
      }
      // player died
      if (player_movement.GetHealth() <= 0) {
          StopBat();
          bat_body.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
          return;
      }
      // in attack range
      if (dis <= 25f) {
          move = false;
          bat_movement = Vector2.zero;
          bat_body.constraints = RigidbodyConstraints2D.FreezeRotation;
      } else if (!isAttacking) {
          move = true;
          Speed = 35;
          bat_body.constraints = RigidbodyConstraints2D.FreezeRotation;
      }
      // roam randomly
      if (move) {
          if (Time.time - latestDirectionChangeTime > directionChangeTime) {
              latestDirectionChangeTime = Time.time;
              bat_movement = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
          }
          transform.position = new Vector2(
              transform.position.x + (bat_movement.x * Speed * Time.deltaTime),
              transform.position.y + (bat_movement.y * Speed * Time.deltaTime)
          );
      }
      animator.SetFloat("Horizontal",bat_movement.x);
      animator.SetFloat("Vertical",bat_movement.y);
      animator.SetFloat("Speed",bat_movement.sqrMagnitude);
      // attack
      Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attack_point.position,attackRange,playerLayers);
      foreach(Collider2D p in hitPlayer) {
        if(Time.time - latest_attack >= time_between_attack && attack) {
          latest_attack = Time.time;
          Debug.Log("Bat hit " + p.name);
          StartCoroutine(player_hit());
        
        }
      }
    }
    private void StopBat() {
        move = false; attack = false;
        Speed = 0; bat_movement = Vector2.zero;
        animator.SetFloat("Speed", 0);
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 0);
    }
    IEnumerator player_hit() {
      bat_damage = Random.Range(5,10);
      player_movement.GetComponent<PlayerMovement>().TakeDamage(bat_damage);
      yield return new WaitForSeconds((float)2);
    }
    void OnDrawGizmosSelected() {
        if(attack_point == null) return;
        Gizmos.DrawWireSphere(attack_point.position,attackRange);
    }
}
