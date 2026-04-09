using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class wolfmovement : MonoBehaviour {
    public Rigidbody2D wolf_body;
    private Vector2 wolf_movement;
    public Animator animator;
    public AudioSource attack_sound, wolf_angry;
    public TextMeshProUGUI healthText;
    public LayerMask playerLayers;
    public Transform attack_point;
    public PlayerMovement player_movement;
    public Hit hit;

    public float Speed;
    public int health;
    private int current_health;
    private int wolf_damage;
    public float attackRange = 0.5f;

    private float latestDirectionChangeTime, latest_attack;
    private readonly float directionChangeTime = 3f, time_between_attack = 2f;

    private int MaxDist = 100, MinDist = 101;

    private bool attack, move, play_angry, isDead, isAttacking;

    GameObject player;
 
    void Start (){
        //vlk = GameObject.Find("Wolf").transform;
        //wolf_body = GameObject.Find("Game Object Script").GetComponent<Rigidbody2D>();
        latestDirectionChangeTime = 0f;
        health = Random.Range(100, 160);
        current_health = health;
        healthText.text = health.ToString() + " hp";
        player = GameObject.FindGameObjectWithTag ("Player");
        move = true; play_angry = true; isDead = false;
    }
    private void Update(){
      if (isDead) return;
      float dis = Vector2.Distance(transform.position, player.transform.position);
      // wolf died
      if (hit != null && hit.GetHealth() <= 0) {
        StopWolf();
        animator.SetFloat("Speed", 0); animator.SetFloat("Horizontal", 0); animator.SetFloat("Vertical", 0);
        wolf_body.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        isDead = true;
        return;
      }
      // player died
      if (player_movement.GetHealth() <= 0) {
        StopWolf();
        wolf_body.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        return;
      }
      // in attack range
      if (dis <= 26.2f) {
        move = false;
        wolf_movement = Vector2.zero;
        wolf_body.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation; 
      } 
      else if (!isAttacking) {
        move = true;
        Speed = 35;
        wolf_body.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
      }
      // roam randomly when far away
      if (dis >= MinDist && move) {
          if (Time.time - latestDirectionChangeTime > directionChangeTime) {
              latestDirectionChangeTime = Time.time;
              wolf_movement = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
          }
          attack = true; play_angry = true;
          transform.position = new Vector2(transform.position.x + (wolf_movement.x * Speed * Time.deltaTime), transform.position.y + (wolf_movement.y * Speed * Time.deltaTime));
      }
      // chase player when close enough 
      else if (dis <= MaxDist && move) {
          transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 1);
          wolf_movement.x = transform.position.x <= player.transform.position.x - 20 ? 1f : transform.position.x >= player.transform.position.x + 20 ? -1f : 0f;
          wolf_movement.y = transform.position.y >= player.transform.position.y ? -1f : transform.position.y <= player.transform.position.y ? 1f : 0f;
      }
      animator.SetFloat("Horizontal", wolf_movement.x);
      animator.SetFloat("Vertical", wolf_movement.y);
      animator.SetFloat("Speed", wolf_movement.sqrMagnitude);
      // attack
      Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attack_point.position,attackRange,playerLayers);                 
        foreach(Collider2D p in hitPlayer){
          if(Time.time - latest_attack >= time_between_attack && attack){
            latest_attack = Time.time;
            Debug.Log("Wolf hit " + p.name);
            StartCoroutine(player_hit());
          }
        }
      // angry sound when enter range
      if (dis >= 95 && dis <= 100 && play_angry) {
        play_angry = false;
        StartCoroutine(angry());
      }
    }

    private void StopWolf() { 
        move = false; attack = false; 
        Speed = 0; wolf_movement = Vector2.zero; 
        animator.SetFloat("Speed", 0); animator.SetFloat("Horizontal", 0); animator.SetFloat("Vertical", 0);
    }

    IEnumerator player_hit() {
        wolf_damage = Random.Range(10, 20);
        var player_pos = player_movement.transform.position;
        var wolf_pos = transform.position;
        float dx = player_pos.x - wolf_pos.x, dy = player_pos.y - wolf_pos.y;
        if (Mathf.Abs(dx) >= Mathf.Abs(dy)) { animator.SetTrigger(dx < 0 ? "roar_left" : "roar_right"); } 
        else { animator.SetTrigger(dy < 0 ? "roar_down" : "roar_up"); }
        isAttacking = true; Speed = 0; wolf_movement = Vector2.zero;
        attack_sound.Play();
        player_movement.GetComponent<PlayerMovement>().TakeDamage(wolf_damage);
        yield return new WaitForSeconds((float)0.1);
        Speed = 35;
        yield return new WaitForSeconds((float)0.6);
        isAttacking = false;
    }

    IEnumerator angry(){
        wolf_angry.Play();
        yield return new WaitForSeconds((float)2);
    }

    void OnDrawGizmosSelected(){
        if(attack_point == null) return;
        Gizmos.DrawWireSphere(attack_point.position, attackRange);
    }
}

