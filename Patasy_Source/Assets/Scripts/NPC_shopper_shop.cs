using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC_shopper_shop : MonoBehaviour
{
    public GameObject table;
    public TextMeshProUGUI pointstext;
    public PlayerMovement playerMovement;
    public Transform items;
    public TextMeshProUGUI alert_text;
    public GameObject shop_alert;
    public TextMeshProUGUI points_text;



    public void show_points()
    {
        pointstext.text = playerMovement.Return_points().ToString();
    }

    public void buy_small_potion()
    {
        if(playerMovement.if_poins(1)){
            playerMovement.Points_down(1);
            playerMovement.heal_up(40);
            StartCoroutine(buy_small_potion_alert());
            show_points();
        }
        else{
            StartCoroutine(Not_points());
        }
    }

    IEnumerator buy_small_potion_alert()
    {
        //GameObject small_potion_item = GameObject.Find("item1\buy_alert");

        shop_alert.SetActive(true);
        alert_text.color = Color.green;
        alert_text.text = "You healed 40 hp.";

        //small_potion_item.SetActive(true);
        yield return new WaitForSeconds((float)1.5);
        //small_potion_item.SetActive(false);
        alert_text.text = "";
        shop_alert.SetActive(false);
    }

    public void buy_big_potion()
    {
        if(playerMovement.if_poins(2)){
            playerMovement.Points_down(2);
            playerMovement.heal_up(80);
            StartCoroutine(buy_big_potion_alert());
            show_points();
        }
        else{
            StartCoroutine(Not_points());
        }
    }

    IEnumerator buy_big_potion_alert()
    {
        //GameObject big_potion_item = GameObject.Find("item2\buy_alert");

        shop_alert.SetActive(true);
        alert_text.color = Color.green;
        alert_text.text = "You healed 80 hp.";

        //big_potion_item.SetActive(true);
        yield return new WaitForSeconds((float)1.5);
        //big_potion_item.SetActive(false);

        alert_text.text = "";
        shop_alert.SetActive(false);
    }

    public void increase_hp()
    {
        if(playerMovement.if_poins(1)){
            playerMovement.Points_down(1);
            playerMovement.increase_hp(1);
            StartCoroutine(increase_hp_show_count());
            show_points();
        }
        else{
            StartCoroutine(Not_points());
        }
    }

    IEnumerator increase_hp_show_count()
    {
        //GameObject increase_hp_item = GameObject.Find("hp_count");

        shop_alert.SetActive(true);
        alert_text.color = Color.green;
        alert_text.text = "You increased your hp by 1.";

        //increase_hp_item.SetActive(true);
        yield return new WaitForSeconds((float)1.5);
        //increase_hp_item.SetActive(false);

        alert_text.text = "";
        shop_alert.SetActive(false);
    }

    public void increase_damage()
    {
        if(playerMovement.if_poins(1)){
            playerMovement.Points_down(1);
            playerMovement.increase_damage(2);
            StartCoroutine(increase_dmg_show_count());
            show_points();
        }
        else{
            StartCoroutine(Not_points());
        }

    }

    IEnumerator increase_dmg_show_count()
    {
        //GameObject increase_dmg_item = GameObject.Find("dmg_count");

        shop_alert.SetActive(true);
        alert_text.color = Color.green;
        alert_text.text = "You increased your damage by 2";

        //increase_dmg_item.SetActive(true);
        yield return new WaitForSeconds((float)1.5);
        //increase_dmg_item.SetActive(false);

        alert_text.text = "";
        shop_alert.SetActive(false);
    }

    
    IEnumerator Not_points()
    {   
        StartCoroutine(Red_point());
        shop_alert.SetActive(true);
        alert_text.color = Color.red;
        alert_text.text = "Not enough points.";
        yield return new WaitForSeconds((float)1.5);
        alert_text.text = "";
        shop_alert.SetActive(false); 
    }

    IEnumerator Red_point()
    {
        points_text.color = Color.red;
        yield return new WaitForSeconds((float)1);
        points_text.color = Color.white;

    }

    

}
