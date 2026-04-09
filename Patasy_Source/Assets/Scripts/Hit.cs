using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Hit : MonoBehaviour {    
    public PlayerMovement player_movement;
    public SpriteRenderer sprite;

    public int health_from;
    public int health_to;
    private int health;
    private int current_health;
    public health_bar HealthBar;
    public GameObject Damage_count;
    public Animator damageTextAnimator;
    public TextMeshProUGUI damageAmountText;
    public TextMeshProUGUI healthText;
    
    private EnemyGenerator enmemies_info;
    public GameObject Loading_screen;
    
    void Start() {
        health = Random.Range(health_from, health_to);
        //enemy_movement.GetComponent("health");
        healthText.text = health.ToString() + " hp";
        current_health = health;
        damageTextAnimator = Damage_count.GetComponent<Animator>();
        HealthBar.SetMaxHealth(health);
        enmemies_info = GameObject.Find("Enemies").GetComponent<EnemyGenerator>();
    }

    public void TakeDamage(int damage)
    {   
        current_health -= damage;
        GetHealth();
        StartCoroutine(Red());

        if (current_health <= 0){
            Debug.Log ("Dead");
            current_health = 0;
            player_movement.GetComponent<PlayerMovement>();
            player_movement.Score_up(health);
            player_movement.Point_up();
            Debug.Log("Enemy died!");
            StartCoroutine(WaitTime1()); 
            Debug.Log(enmemies_info.Return_enemy_count());
            if(enmemies_info.Return_enemy_count() == 0){
                player_movement.Win_Game_Time();
            }
           
        }
        damageAmountText.text = "-" + damage;
        healthText.text = current_health.ToString () + " hp";
        StartCoroutine(damage_count());
        HealthBar.SetHealth(current_health);
    }

    public int GetHealth(){
        return current_health;
    }

    
    IEnumerator Red() 
    {
        sprite.color = new Color (255, 0, 0, 255);   
        yield return new WaitForSeconds((float)0.5);
        sprite.color = new Color (255, 255, 255, 255);
    }
    IEnumerator damage_count() 
    {
        Damage_count.SetActive(true);
        damageTextAnimator.Play("Fade");
        yield return new WaitForSeconds((float)1.8);
        Damage_count.SetActive(false);
    }

    IEnumerator WaitTime1() 
    {
        yield return new WaitForSeconds((float)0.5);
        sprite.color = new Color (255, 0, 0, 255);   
        yield return new WaitForSeconds(2);
        sprite.color = new Color (255, 255, 255, 255);
    }
}
