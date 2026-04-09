using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    private Vector2 movement;
    public SpriteRenderer sprite;

    private int player_damage;
    public TextMeshProUGUI playerName;
    private int health = 100;
    private int current_health;
    public TextMeshProUGUI healthText;

    private int Score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI damageAmountText;
    public Animator damageTextAnimator;

    public TextMeshProUGUI pointText;
    private int points = 0;

    public health_bar HealthBar;
    public GameObject redplayer;
    public GameObject GameOver;
    public GameObject Damage_count;
    public GameObject pause_menu;

    public GameObject Audio_manager;
    public AudioSource walk1;
    public AudioSource walk2;
    private AudioSource player_audio;
    public LayerMask enemyLayers;
    public float attackRange = 0.5f;
    public Transform attack_point;

    public LayerMask NPClayer;
    public float clickRange;
    public Transform clickPoint;
    public GameObject shopTable;

    public GameObject Win_Game;
    public TextMeshProUGUI win_score;

    private GameObject hit;
    private EnemyGenerator enemies;
    private bool move;

    private float latest_attack;
    private readonly float time_between_attack = 2f;
    private bool attack;

    private int damage_from = 30;
    private int damage_to = 50;

    private int increase_dmg = 0;

    void Start() {
        current_health = health;
        //HealthBar = GameObject.Find("health bar object").GetComponent<health_bar>();
        player_audio = Audio_manager.GetComponent<AudioSource>();
        HealthBar.SetMaxHealth(health);
        scoreText.text = Score.ToString();
        healthText.text = health.ToString() + " hp";
        attack = true;
        move = true;
        enemies = GameObject.Find("Enemies").GetComponent<EnemyGenerator>();
    }

    void Update() {
        if(move){
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        animator.SetFloat("Horizontal",movement.x);
        animator.SetFloat("Vertical",movement.y);
        animator.SetFloat("Speed",movement.sqrMagnitude);

        if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1){
            animator.SetFloat("last_move_h", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("last_move_v", Input.GetAxisRaw("Vertical"));
            /*if(Time.time - latest_walk >= time_between_walk){
                latest_walk = Time.time;
                StartCoroutine(walk());
            }*/
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time - latest_attack >= time_between_attack || 
        Input.GetKeyDown(KeyCode.Space) && Time.time - latest_attack >= time_between_attack){
            player_damage = Random.Range(damage_from+increase_dmg,damage_to+increase_dmg);
            latest_attack = Time.time;
            if (EventSystem.current.IsPointerOverGameObject() || (attack == false)){
                Debug.Log("Clicked on the UI");
            }
            else{
                if(movement.x == 1 || animator.GetFloat("last_move_h") == 1){
                    animator.SetTrigger("Attack");
                }
                else if(movement.x == -1 || animator.GetFloat("last_move_h") == -1){ 
                    animator.SetTrigger("Attack_left");
                }
                else if(movement.y == -1 || animator.GetFloat("last_move_v") == -1){
                    animator.SetTrigger("Attack_up");
                }
                else if(movement.y == 1 || animator.GetFloat("last_move_v") == 1){
                    animator.SetTrigger("Attack_down");
                }
                
                player_audio.Play();
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attack_point.position,attackRange,enemyLayers);
                 
                foreach(Collider2D enemy in hitEnemies)
                {
                    Debug.Log("We hit " + enemy.name);
                    hit  = GameObject.Find(enemy.name);
                    hit.GetComponent<Hit>().TakeDamage(player_damage);
                    int mob_health = hit.GetComponent<Hit>().GetHealth();
                    if(mob_health<=0){
                        StartCoroutine(destroy());
                    }
                }
            }  
        }
        Collider2D[] clickNPC = Physics2D.OverlapCircleAll(clickPoint.position,clickRange,NPClayer);         
        foreach(Collider2D npc in clickNPC) {
            Debug.Log("NPC detected");
            if(Vector2.Distance(transform.position, GameObject.Find(npc.name).transform.position) < 40 && (Input.GetKeyDown(KeyCode.Mouse1))){
                shopTable.SetActive(true);
                GameObject.Find(npc.name).GetComponent<NPC_shopper_shop>().show_points();
            }
            else if(Vector2.Distance(transform.position, GameObject.Find(npc.name).transform.position) >= 40){
                Debug.Log("not");
                shopTable.SetActive(false);
            }
        }
    }
    void FixedUpdate() {
        if (current_health<=0){
            StartCoroutine(dead());
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("hit") || animator.GetCurrentAnimatorStateInfo(0).IsName("hit_down") 
        || animator.GetCurrentAnimatorStateInfo(0).IsName("hit_up") || animator.GetCurrentAnimatorStateInfo(0).IsName("hit_left")){
            StartCoroutine(if_hit());
        }
        else{
            if(Input.GetAxisRaw("Horizontal") == 1 && Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Horizontal") == 1 && Input.GetAxisRaw("Vertical") == -1 
            || Input.GetAxisRaw("Horizontal") == -1 && Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Horizontal") == -1 && Input.GetAxisRaw("Vertical") == -1){
                movement /= (float)1.3;
            } 
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);       
        }
    }

    IEnumerator walk(){
        walk1.volume /= 5;
        walk1.Play();
        yield return new WaitForSeconds((float)0.2);
        walk1.Stop();
        walk2.volume /= 5;
        walk2.Play();
        yield return new WaitForSeconds((float)0.1);
        walk2.Stop();
        walk1.volume *= 5;
        walk2.volume *= 5;
    }

    IEnumerator if_hit() {
        yield return new WaitForSeconds(1);
        movement.x=0;
        movement.y=0;
    }
    IEnumerator dead() {
        yield return new WaitForSeconds(1);
        movement.x=0;
        movement.y=0;
        yield return new WaitForSeconds(6);
    }
    IEnumerator destroy() {
        yield return new WaitForSeconds(2);
        Destroy(hit);
    }
    IEnumerator damage_count()  {
        Damage_count.SetActive(true);
        damageTextAnimator.Play("Fade");
        yield return new WaitForSeconds((float)1.8);
        Damage_count.SetActive(false);
    }
    
    public void TakeDamage(int damage) {
        current_health -= damage;
        StartCoroutine(Red());
        if (current_health <= 0){
            move = false;
            StartCoroutine(WaitTime1());
            StartCoroutine(WaitTime2());
            StartCoroutine(WaitTime3());
            current_health = 0;
            Debug.Log ("Dead");
        }
        damageAmountText.text = "-" + damage;
        healthText.text = current_health.ToString () + " hp";
        StartCoroutine(damage_count());
        HealthBar.SetHealth(current_health);
    }

    public void Score_up(int health) {
        Score += health;
        scoreText.text = Score.ToString();
        StartCoroutine(Score_color_blink());
    }
    public void Point_up() {
        points += 1;
        pointText.text = points.ToString();
    }
    public void Points_down(int amount) {
        points -= amount;
        pointText.text = points.ToString();
    }
    public int Return_points() {
        return points;
    }
    public bool if_poins(int amount_need) {
        if(points>=amount_need){
            return true;
        }
        else{
            return false;
        }
    }

    IEnumerator Red() {
        sprite.color = new Color (255, 0, 0, 255);   
        yield return new WaitForSeconds((float)0.4);
        sprite.color = new Color (255, 255, 255, 255);
    }
    IEnumerator Score_color_blink() { 
        for(int i = 0; i <= 1; i++) {  
          scoreText.color = Color.green;
          yield return new WaitForSeconds(1);
          scoreText.color = new Color(255, 240, 0, 255); 
        } 
    }
    IEnumerator WaitTime1() {
        pause_menu.GetComponent<PauseMenu>().EscDisable();
        yield return new WaitForSeconds(1);
        sprite.color = new Color (255, 0, 0, 255);   
    }
    IEnumerator WaitTime2() {
        yield return new WaitForSeconds(3);
        GameOver.SetActive(true);
        pause_menu.SetActive(false);
        finalScoreText.text = "Your score: " + Score.ToString();
    }
    IEnumerator WaitTime3() {
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
        GameOver.SetActive(false);
        Score = 0;
        sprite.color = new Color (255, 255, 255, 255);
        pause_menu.GetComponent<PauseMenu>().EscEnable();
        attack = true;
        move = true;
        Debug.Log("Scene loaded");      
    } 

    public void Win_Game_Time() {
        StartCoroutine(Win_Game_time2());
        StartCoroutine(Win_Game_time3());
    }
    IEnumerator Win_Game_time2() {
        move = false;
        pause_menu.GetComponent<PauseMenu>().EscDisable();
        yield return new WaitForSeconds(2);
        pause_menu.SetActive(false);
        Win_Game.SetActive(true);
        win_score.text = "Your score: " + Score.ToString();
    }
    IEnumerator Win_Game_time3() {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
        Win_Game.SetActive(false);
        Score = 0;
        pause_menu.GetComponent<PauseMenu>().EscEnable();
        attack = true;
        move = true;
        Debug.Log("Scene loaded");      
    }
    public void GetName(string name) {
        playerName.text = name;
    }
    public int GetScore() {
        return Score;
    }
    public string ReturnName() {
        return playerName.text;
    }

    public int GetHealth() {
        return current_health;
    }
    public void heal_up(int amount_hp) {
        current_health += amount_hp;
        if(current_health>health){
            current_health=health;
        }
        healthText.text = current_health.ToString () + " hp";
        HealthBar.SetHealth(current_health);
    }
    public void increase_hp(int amount_hp) {
        health += amount_hp;
        current_health += amount_hp;
        HealthBar.SetMaxHealth(health);
        healthText.text = current_health.ToString () + " hp";
        HealthBar.SetHealth(current_health);
    }
    public int return_health() {
        return health;
    }
    public void increase_damage(int amount_damage) {
        increase_dmg += amount_damage;
    }
    public int get_increased_damage() {
        return increase_dmg;
    }
    public string return_increased_damage() {
        return (damage_from+increase_dmg).ToString() + " to " + (damage_to+increase_dmg).ToString();
    }

    public void SavePlayer(string name) {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement player_stats = player.GetComponent<PlayerMovement>();
        enemies.Detect_actual_enemy_names_and_positions();
        SaveSystem.SavePlayer(player, player_stats, enemies, name);
    }
    
    public void LoadPlayer(string name) {
        Player_data data = SaveSystem.LoadPlayer(name);
        playerName.text = data.name;
        Score = data.score;
        scoreText.text = Score.ToString();
        health = data.max_health;
        HealthBar.SetMaxHealth(health);
        increase_dmg = data.max_damage;
        current_health = data.health;
        healthText.text = current_health.ToString () + " hp";
        HealthBar.SetHealth(current_health);
        points = data.points;
        pointText.text = points.ToString();
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;

        Debug.Log("Hra nactena");
        string jmena = "";
        foreach(var enemy_name in data.enemy_names){
            jmena += enemy_name + ", ";
        }
        Debug.Log("Enemies Generated "+ jmena + "Length: "  +data.enemy_names.Length);
        
        Vector3[] enemy_string_positions = new Vector3[data.active_enemies_locations.Length];
        for(int i=0;i<data.active_enemies_locations.Length;i++){
            if (data.active_enemies_locations[i].StartsWith ("(") && data.active_enemies_locations[i].EndsWith (")")) {
                  data.active_enemies_locations[i] = data.active_enemies_locations[i].Substring(1, data.active_enemies_locations[i].Length-2);
              }
            string[] s = data.active_enemies_locations[i].Split(',');
            float[] VectorPosition = new float[s.Length];
            Vector3 result = new Vector3();
            for (int j = 0; j < s.Length;j++){
                VectorPosition[j] = float.Parse(s[j], System.Globalization.CultureInfo.InvariantCulture);
                result.x = VectorPosition[0];
                result.y = VectorPosition[1];
                result.z = 1;
            }
            enemy_string_positions[i] = result;
        }
             
        string pozice = "";
        foreach(var enemy_vector in enemy_string_positions){
            pozice += enemy_vector + ", ";
        }
        Debug.Log("Positions Generated "+ pozice + "Length: "  +data.active_enemies_locations);
        enemies.Delete_all_enemies();
        enemies.Generate_new_enemies_by_count(data.enemy_names,enemy_string_positions);
    }

    void OnDrawGizmosSelected() {
        if(attack_point == null){
            return;
        } 
        Gizmos.DrawWireSphere(attack_point.position,attackRange);
    }
    
    /*
    bool hit_right;
    public bool GetPlayerLocation_right()
    {
        if(movement.x == 1 || animator.GetFloat("last_move_h") == 1){
            hit_right = true;
        }
        else{
            hit_right = false;
        }
        return hit_right;
    }
    bool hit_down;
    public bool GetPlayerLocation_down()
    {
        if(movement.y == -1 || animator.GetFloat("last_move_v") == -1){
            hit_down = true;
        }
        else{
            hit_down = false;
        }
        return hit_down;
    }
    bool hit_up;
    public bool GetPlayerLocation_up()
    {
        if(movement.y == 1 || animator.GetFloat("last_move_v") == 1){
            hit_up = true;
        }
        else{
            hit_up = false;
        }
        return hit_up;
    }
    bool hit_left;
    public bool GetPlayerLocation_left()
    {
        if(movement.x == -1 || animator.GetFloat("last_move_h") == -1){
            hit_left = true;
        }
        else{
            hit_right = false;
        }
        return hit_left;
    }*/
}