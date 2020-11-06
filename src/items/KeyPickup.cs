using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarl.Controllers;

namespace Yarl.Items
{
    public class KeyPickup : PickupController
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
            player.SetKeyFound(true);
            return true;
        }
    }
}