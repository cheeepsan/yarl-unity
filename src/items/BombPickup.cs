using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarl.Controllers;

namespace Yarl.Items
{
    public class BombPickup : PickupController
    {

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
            player.AddBombAmount(1); // should be increase by one
            return true;
        }
    }
}