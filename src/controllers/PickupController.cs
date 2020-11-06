using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yarl.Controllers
{

    public class PickupController : MonoBehaviour
    {
        private CircleCollider2D coll;
        // Start is called before the first frame update
        void Start()
        {
            coll = GetComponent<CircleCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.tag == "Player" && !collider.GetType().Equals(typeof(CircleCollider2D))) // hack to match only capsule collider
            {
                PlayerController p = collider.gameObject.GetComponent<PlayerController>();
                bool isUsed = PlayerPickupLogic(p);

                if (isUsed)
                {
                    this.DestroyOnUse();
                }
            }
            else if (collider.gameObject.tag == "enemy")
            {
                EnemyController e = collider.gameObject.GetComponent<EnemyController>();
                bool isUsed = EnemyPickupLogic(e);

                if (isUsed)
                {
                    this.DestroyOnUse();
                }
            }
        }

        private void DestroyOnUse()
        {
            Destroy(this.gameObject);
        }
        virtual protected bool PlayerPickupLogic(PlayerController player)
        {
            //implement in subclass
            return false;
        }
        virtual protected bool EnemyPickupLogic(EnemyController enemy)
        {
            //implement in subclass
            return false;
        }
    }
}