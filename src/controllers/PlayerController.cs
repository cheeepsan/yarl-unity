using System.Collections;
using System.Collections.Generic;
using Yarl.Util;
using UnityEngine;

using Yarl.Flow;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

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
        public Text skillPointText;

        public GameObject highScorePanel;

        public int amountOfBombs;
        public int score = 0;

        public int bossLevel; // really shouldn't be here
        private int currentLevel { get; set; } // this as well
        private int availableSkillPoints = 0;

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

            this.highScorePanel.SetActive(false);

            keyFound = false;
            currentLevel = 1;

            this.UpdateHpTextBox(); //TODO (no), move all to same init method
            this.UpdateDamageTextBox();
            this.UpdateSpeedTextBox();
            this.UpdateCurrentLevelTextBox();
            this.UpdateKeyTextBox();
            this.UpdateBombAmountTextBox();
            this.UpdateScoreTextBox();
            this.UpdateAvailableSkillPointTextBox();

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
            
            if (Input.GetKeyDown("1"))
            {
                UpgradeHealthPermanently();

            } else if (Input.GetKeyDown("2"))
            {
                UpgradeDamagePermanently();
            }
            else if (Input.GetKeyDown("3"))
            {
                UpgradeSpeedPermanently();
            }

        }


        public override void ReceiveDamage(int incomingDamage)
        {
            if (!isImmune)
            {
                this.hitAudioSource.Play();
                this.health -= incomingDamage;
                this.immunityWindow = this.immunityWindowInitial;
                isImmune = true;
                doFlash = true;
                this.UpdateHpTextBox();
            }

            if (this.health <= 0)
            {
                isDead = true;
                this.Die();
            }

        }

        protected override void Die() {
            this.deathAudioSource.Play();
            Time.timeScale = 0;
            this.line.enabled = false;
            this.highScorePanel.SetActive(true);
            var text = this.highScorePanel.transform.Find("Total").GetComponent<Text>();
            text.text = "Your total score is: " + this.score;
        }

        public void TransitionToMenuOnDeath() //lazy to do this properly
        {
            var input = this.highScorePanel.transform.Find("InputField").transform.Find("Text").GetComponent<Text>();
            var newHighScore = new HighScore(input.text, this.score);

            string scoreJson = "";
            List<HighScore> hList = new List<HighScore>();
            
            if(System.IO.File.Exists("score.json"))
            {
                scoreJson = System.IO.File.ReadAllText("score.json");
                hList = JsonUtility.FromJson<HighScoreWrapper>(scoreJson).l;
                hList = hList.OrderByDescending(x => x.score).Take(9).ToList();
            }

            hList.Add(newHighScore);
            hList = hList.OrderBy(x => x.score).ToList();
            var wrapperList = new HighScoreWrapper() { l = hList };
            var jsonToWrite = JsonUtility.ToJson(wrapperList);
            System.IO.File.WriteAllText("score.json", jsonToWrite);


            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
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

        private void DecreaseAvailableSkillpoints()
        {
            this.availableSkillPoints--;
            UpdateAvailableSkillPointTextBox();
        }
        private void IncreaseAvailableSkillpoints()
        {
            this.availableSkillPoints++;
            UpdateAvailableSkillPointTextBox();
        }
        public bool GetIsDead()
        {
            return this.isDead;
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
        private void UpdateAvailableSkillPointTextBox()
        {
            this.skillPointText.text = "Available skillpoints: " + this.availableSkillPoints;
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
            this.IncreaseAvailableSkillpoints();
        }

        private void UpgradeSpeedPermanently()
        {
            if(this.availableSkillPoints > 0)
            {
                this.speed++;
                this.DecreaseAvailableSkillpoints();
                this.UpdateSpeedTextBox();
            }

        }
        private void UpgradeDamagePermanently()
        {
            if (this.availableSkillPoints > 0)
            {
                this.damage++;
                this.DecreaseAvailableSkillpoints();
                this.UpdateDamageTextBox();
            }

        }
        private void UpgradeHealthPermanently()
        {
            if (this.availableSkillPoints > 0)
            {
                this.maxHealth++;
                this.DecreaseAvailableSkillpoints();
                this.UpdateHpTextBox();
            }
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

        public void Quit() // too lazy to implement properly
        {
            Application.Quit();
        }
    }
}
