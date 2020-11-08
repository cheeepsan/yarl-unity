using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarl.Controllers;

namespace Yarl.Actors
{
    public class BomberCollider : MonoBehaviour
    {
        private BomberEnemyController bomberController;
        // Start is called before the first frame update
        void Start()
        {
            bomberController = GetComponent<BomberEnemyController>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (bomberController.IsExploding() == true)
            {
                if (collider.gameObject.tag == "enemy") 
                {
                    EnemyController e = collider.gameObject.GetComponent<EnemyController>();
                    e.ReceiveDamage(bomberController.damage / 2);
                }

                if (collider.gameObject.tag == "Player")
                {
                    bomberController.DamagePlayer();
                }
            }

        }
    }
}