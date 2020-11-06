using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarl.Controllers;

namespace Yarl.Items
{
    public class SpeedPickup : PickupController
    {
        public int speedValue = 2;
        public float duration = 3f;
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
            player.SetReceiveSpeedBuff(this.speedValue, this.duration);
            return true;
        }
    }
}