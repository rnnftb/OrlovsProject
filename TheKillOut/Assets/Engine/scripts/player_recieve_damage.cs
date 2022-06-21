using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_recieve_damage : MonoBehaviour
{
    public GameObject player_main;

    public void take_dmg(int dmg)
    {


        player_main.GetComponent<player_controller>().receive_dmg(dmg, false);
    }
 


}
