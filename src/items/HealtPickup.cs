using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarl.Controllers;

namespace Yarl.Items
{
    public class HealtPickup : PickupController
    {

        public int healthValue = 3;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        override protected bool PlayerPickupLogic(PlayerController player)
        {
            Debug.Log("at pickup");
            int currentHealth = player.GetHealth();
            int maxHealth = player.maxHealth;
            if (currentHealth < maxHealth)
            {
                int diff = maxHealth - currentHealth;
                if (diff < this.healthValue)
                {
                    player.AddHealth(diff);
                } else
                {
                    player.AddHealth(this.healthValue);
                }
                
                return true;
            } else
            {
                return false;
            }
        }
    }
}