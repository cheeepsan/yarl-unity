using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarl.Flow;
using Yarl.Util;

namespace Yarl.Controllers
{
    public class EnemyController : BaseActorController
    {
        readonly YarlModel model = Simulation.GetModel<YarlModel>();
        PlayerController player;

        public float detectionRadius = 7f;
        public float attackRange = 1f;
        public float attackCoolDown = 3f;
        public float activityWindow = 1f; // check for player after n frames

        public int scoreValue = 100;

        private float currentAttackCooldown = 0f;
        private float currentActivityWindow = 0f;
        public GameObject pickup {get; set;}

        public override void Start()
        {
            base.Start();

            player = model.player;
        }

        // Update is called once per frame
        public override void Update()
        {
           base.Update();
           if (currentAttackCooldown > 0)
           {
               currentAttackCooldown -= Time.deltaTime;
           }
           if (currentActivityWindow <= 0)
            {
                
                OnPlayerActivity();
                currentActivityWindow = activityWindow;
            } else
            {
                Debug.Log("Active cooldown");
                currentActivityWindow -= Time.deltaTime;
            }

        }

        protected override void Die()
        {
            if(this.pickup != null)
            {
                Debug.Log("pickup drop");
                this.pickup.transform.position = this.gameObject.transform.position;
                this.pickup.SetActive(true);
            }
            player.AddToScore(this.scoreValue);
            Destroy(this.gameObject);

        }

        private void OnPlayerActivity()
        {
            if (DetectPlayer() && !isDead)
            {
                Debug.Log("Player seen");
                if (currentAttackCooldown <= 0 && InAttackRange())
                {
                    //play attack animation
                    player.ReceiveDamage(this.damage);
                    currentAttackCooldown = attackCoolDown;
                }
                else
                {
                    Vector2 target = new Vector2(player.transform.position.x, player.transform.position.y);
                    float step = speed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, target, step);
                }


            }
        }

        private bool DetectPlayer()
        {
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
            Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);

            float distance = Vector2.Distance(playerPos, enemyPos);
 
            if (distance <= detectionRadius)
            {
                //Debug.Log(coll.name);
                return true;
            }
            
            return false;
        }

        private bool InAttackRange()
        {
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
            Vector2 enemyPos = new Vector2(transform.position.x, transform.position.y);
            float distance = Vector2.Distance(playerPos, enemyPos);

            if (distance <= attackRange)
            {
                Debug.Log("Attack player");
                return true;
            }

            return false;
        }

    }
}