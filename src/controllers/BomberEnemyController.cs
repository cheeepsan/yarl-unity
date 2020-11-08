using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarl.Actors;

namespace Yarl.Controllers
{
    public class BomberEnemyController : EnemyController
    {

        public List<GameObject> explosion;
        public GameObject bombColliderObj;

        private bool isCharging = false;
        private bool isExploding = false;
        private SpriteRenderer r;
        private CircleCollider2D bombCollider;

        // Start is called before the first frame update
       public override void Start()
       {
           base.Start();
           r = GetComponent<SpriteRenderer>();
           r.color = Color.red;
           base.currentAttackCooldown = 0f;
           this.bombCollider = bombColliderObj.GetComponent<CircleCollider2D>();
           foreach (GameObject expl in explosion)
           {
               expl.SetActive(false);
           }
       }
        // Update is called once per frame

        protected override void OnPlayerActivity()
        {
            if (DetectPlayer() && !isDead)
            {

                if (InAttackRange() && this.currentAttackCooldown <= 0)
                {
                    AttackPlayer();
                }
                else
                {
                    if (ableToMove)
                    {
                        Vector2 target = new Vector2(player.transform.position.x, player.transform.position.y);
                        float step = speed * Time.deltaTime;
                        transform.position = Vector2.MoveTowards(transform.position, target, step);
                    }


                }


            }
        }
        protected override void AttackPlayer()
        {
            this.currentAttackCooldown = base.attackCoolDown;
            
            StartCoroutine("ShowExplosionSprites");
            
            this.currentAttackCooldown = base.attackCoolDown;
        }

        public override void DamagePlayer()
        {
            
            var damageList = Physics2D.OverlapCircleAll(transform.position, bombCollider.radius);
            foreach(Collider2D damage in damageList)
            {
                
                if (damage.tag == "enemy")
                {
                    var enemyObj = damage.gameObject;

                    var enemy = enemyObj.GetComponent<EnemyController>();
                    enemy.ReceiveDamage(base.damage / 2);
                }
                if (damage.tag == "Player")
                {
                    player.ReceiveDamage(base.damage);
                }
            }
        }

        protected IEnumerator ShowExplosionSprites()
        {
            isCharging = true;
            yield return new WaitForSeconds(this.currentAttackCooldown);
            isCharging = false;
            foreach (GameObject expl in explosion)
            {
                expl.SetActive(true);
            }
            this.DamagePlayer();
            yield return new WaitForSeconds(0.3f);
            foreach (GameObject expl in explosion)
            {
                expl.SetActive(false);
            }
            
        }

        public bool IsExploding()
        {
            return this.isExploding;
        }

        public override void OnGUI()
        {
            Vector2 targetPos;
            targetPos = Camera.main.WorldToScreenPoint(transform.position);

            float yOffset = 60; // sprite size should be used in calculations
            float xOffset = 30;

            GUI.Box(new Rect(targetPos.x - xOffset, Screen.height - targetPos.y - yOffset, 60, 20), health + "/" + maxHealth);

   
            float chargeYOffset = 90; // sprite size should be used in calculations
            var cooldown = "-"; 
            if (this.currentAttackCooldown > 0)
            {
                cooldown = System.String.Format("{0:0.00}", this.currentAttackCooldown);
            }
                
            GUI.Box(new Rect(targetPos.x - xOffset, Screen.height - targetPos.y - chargeYOffset, 60, 20), cooldown);
           

        }
    }

}