using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Player_data {
    public string name;
    public int score;
    public int health;
    public int max_health;
    public int max_damage;
    public int points;
    public float[] position;

    public string[] enemy_names;
    public string[] active_enemies_locations;

    public Player_data (GameObject player, PlayerMovement player_stats, EnemyGenerator enemies) {
        name = player_stats.ReturnName();
        score = player_stats.GetScore();
        health = player_stats.GetHealth();
        max_health = player_stats.return_health();
        max_damage = player_stats.get_increased_damage();
        points = player_stats.Return_points();
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        enemy_names = enemies.Return_actual_enemy_names();
        active_enemies_locations = enemies.Return_actual_enemy_positions();
    }
}
