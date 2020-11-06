using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarl.Controllers;

namespace Yarl.Items
{
    public class DamagePickup : PickupController
    {

        public int damageValue = 2;
        public float duration = 3f;
        private SpriteRenderer r;
        // Start is called before the first frame update
        void Start()
        {
            r = GetComponent<SpriteRenderer>();
            r.color = Color.yellow;
        }

        // Update is called once per frame
        void Update()
        {

        }
        override protected bool PlayerPickupLogic(PlayerController player)
        {
            player.SetReceiveDamageBuff(this.damageValue, this.duration);
            return true;
        }
    }
}