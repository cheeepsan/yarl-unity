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
        protected PlayerController player;

        public float detectionRadius = 7f;
        public float attackRange = 1f;
        public float attackCoolDown = 3f;
        public float activityWindow = 1f; // check for player after n frames

        public int scoreValue = 100;

        protected float currentAttackCooldown = 0f;
        private float currentActivityWindow = 0f;
        protected bool ableToMove = true;
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
               
                currentActivityWindow -= Time.deltaTime;
            }

        }

        protected override void Die()
        {
            if(this.pickup != null)
            {
              
                this.pickup.transform.position = this.gameObject.transform.position;
                this.pickup.SetActive(true);
            }
            player.AddToScore(this.scoreValue);
            Destroy(this.gameObject);

        }

        protected virtual void OnPlayerActivity()
        {
            if (DetectPlayer() && !isDead)
            {

                if (currentAttackCooldown <= 0 && InAttackRange())
                {
                    //play attack animation
                    //player.ReceiveDamage(this.damage);
                    AttackPlayer();
                    currentAttackCooldown = attackCoolDown;
                }
                else
                {
                    if(ableToMove)
                    {
                        Vector2 target = new Vector2(player.transform.position.x, player.transform.position.y);
                        float step = speed * Time.deltaTime;
                        transform.position = Vector2.MoveTowards(transform.position, target, step);
                    }


                }


            }
        }

        protected virtual void AttackPlayer()
        {
            this.DamagePlayer();
        }

        public virtual void DamagePlayer()
        {
            player.ReceiveDamage(this.damage);
        }

        protected bool DetectPlayer()
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

        protected bool InAttackRange()
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