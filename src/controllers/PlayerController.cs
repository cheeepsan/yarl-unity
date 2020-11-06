using System.Collections;
using System.Collections.Generic;
using Yarl.Util;
using UnityEngine;

using Yarl.Flow;
using System;
using UnityEngine.UI;

namespace Yarl.Controllers
{
    public class PlayerController : BaseActorController
    {

        public bool controlEnabled = true;
        public Camera camera;
        public Text hpText;
        public Text damageText;
        public Text speedText;
        public Text keyText;
        public Text currentLevelText;
        public Text bombText;
        public Text scoreText;

        public int amountOfBombs;
        public int score = 0;

        public int bossLevel; // really shouldn't be here
        private int currentLevel { get; set; } // this as well
        

        Vector2 move; //?
        SpriteRenderer spriteRenderer;

        readonly YarlModel model = Simulation.GetModel<YarlModel>();

        private CapsuleCollider2D coll;
        private LineRenderer line;

        private bool keyFound { get; set; }
        private bool damageEnabled = false;

        public override void Start()
        {
            base.Start();

            keyFound = false;
            currentLevel = 1;

            this.UpdateHpTextBox(); //TODO (no), move all to same init method
            this.UpdateDamageTextBox();
            this.UpdateSpeedTextBox();
            this.UpdateCurrentLevelTextBox();
            this.UpdateKeyTextBox();
            this.UpdateBombAmountTextBox();
            this.UpdateScoreTextBox();

            coll = GetComponent <CapsuleCollider2D> ();
            line = GetComponent<LineRenderer>();

        }
        public override void Update()
        {
            base.Update();
            ShowAttackVector();
            if (controlEnabled) {
                float mH = Input.GetAxis("Horizontal");
                float mV = Input.GetAxis("Vertical");
                rb2d.velocity = new Vector3(mH * speed, mV * speed, 0);
            }   
            else {
            }
        
        }


        public override void ReceiveDamage(int incomingDamage)
        {
            if (!isImmune)
            {
                this.health -= incomingDamage;
                this.immunityWindow = this.immunityWindowInitial;
                isImmune = true;
                doFlash = true;
                this.UpdateHpTextBox();
            }

            if (this.health == 0)
            {
                isDead = true;
                Debug.Log("DEAD");
            }

        }

        public void SetKeyFound(bool keyFound)
        {
            this.keyFound = keyFound;
            UpdateKeyTextBox();
        }
        public bool GetKeyFound()
        {
            return this.keyFound;
        }

        public void SetCurrentLevel(int level)
        {
            this.currentLevel = level;
        }
        public void IncrementCurrentLevel()
        {
            this.currentLevel += 1;
            this.UpdateCurrentLevelTextBox();
        }

        public int GetCurrentLevel()
        {
            return this.currentLevel;
        }
        public int GetBombAmount()
        {
            return this.amountOfBombs;
        }
        public void AddBombAmount(int bombs)
        {
            this.amountOfBombs += bombs;
            UpdateBombAmountTextBox();
        }
        public void DecreaseBonbAmountByOne()
        {
            this.amountOfBombs--;
            UpdateBombAmountTextBox();
        }
        public void SetScore(int score)
        {
            this.score = score;
            this.UpdateScoreTextBox();
        }
        public void AddToScore(int score)
        {
            this.score += score;
            this.UpdateScoreTextBox();
        }

        public void SetDamageEnabled(bool isEnabled)
        {
            this.damageEnabled = isEnabled;
        }

        public bool GetDamageEnabled()
        {
            return this.damageEnabled;
        }
        /**
         * 
         * BUFFS
         * 
         */
        public override void AddHealth(int hp)
        {
            base.AddHealth(hp);
            this.UpdateHpTextBox();
        }

        public void SetReceiveDamageBuff(int damageBuff, float duration = 3f)
        {
            IEnumerator coroutine = DamageBuff(damageBuff, duration);
            StartCoroutine(coroutine);
        }

        public void SetReceiveSpeedBuff(int speedBuff, float duration = 3f)
        {
            IEnumerator coroutine = SpeedBuff(speedBuff, duration);
            StartCoroutine(coroutine);
        }

        /**
         * 
         * COUROUTINES
         * 
         */
        private IEnumerator DamageBuff(int damageBuff, float duration = 3f)
        {
            this.damage += damageBuff;
            this.UpdateDamageTextBox();
            Debug.Log("Updating damage " + duration);
            yield return new WaitForSeconds(duration);
            this.damage -= damageBuff;
            this.UpdateDamageTextBox();
        }

        private IEnumerator SpeedBuff(int speedBuff, float duration = 3f)
        {
            this.speed += speedBuff;
            this.UpdateSpeedTextBox();
            Debug.Log("Updating damage " + duration);
            yield return new WaitForSeconds(duration);
            this.speed -= speedBuff;
            this.UpdateSpeedTextBox();
        }
        /**
         * 
         * 
         *  UI
         * 
         */
        private void UpdateHpTextBox()
        {
            this.hpText.text = "Health: " + this.health;
        }        
        private void UpdateDamageTextBox()
        {
            this.damageText.text = "Damage: " + this.damage;
        }
        private void UpdateSpeedTextBox()
        {
            this.speedText.text = "Speed: " + this.speed;
        }
        private void UpdateKeyTextBox()
        {
            this.keyText.text = "Key found: " + this.keyFound;
        }
        private void UpdateCurrentLevelTextBox()
        {
            this.currentLevelText.text = "Current level: " + this.currentLevel;
        }
        private void UpdateBombAmountTextBox()
        {
            this.bombText.text = "Bombs: " + this.amountOfBombs;
        }
        private void UpdateScoreTextBox()
        {
            this.scoreText.text = "Score: " + this.score;
        }
        /**
         * 
         * OTHER
         * 
         */

        public void TransitionToNewLevel()
        {
            this.IncrementCurrentLevel();
            this.SetKeyFound(false);
        }
        private void ShowAttackVector()
        {
            var mousePos = Input.mousePosition;
            var worldMousePos = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camera.nearClipPlane));
            var zeta = -9;

            var playerPos = transform.position;
            playerPos.z = zeta;
            worldMousePos.z = zeta;
            var v = worldMousePos - playerPos;

            v = v.normalized;

            v = v * 4f;

            v = v + playerPos;
            line.positionCount = 2;
            line.SetPosition(0, playerPos);
            line.SetPosition(1, v);
        }
    }
}
