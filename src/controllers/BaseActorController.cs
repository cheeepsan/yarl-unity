using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Yarl.Controllers
{
    public class BaseActorController : MonoBehaviour
    {
        //public float attackRange = 2f;
        public float speed = 2f;
        public float immunityWindowInitial = 2f;
        public int maxHealth = 10;
        public int damage = 1;

        protected int health = 10;
        protected bool isDead = false;
        protected bool isImmune = false;
        protected bool doFlash = false;
        protected float immunityWindow = 0;

        protected Rigidbody2D rb2d;
        protected SpriteRenderer rend;

        public AudioSource hitAudioSource;
        public AudioSource deathAudioSource;
        public AudioSource explosionAudioSource;
        // Start is called before the first frame update
        public virtual void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            this.rend = GetComponent<SpriteRenderer>();
            rb2d.freezeRotation = true;
            this.health = this.maxHealth;
        }

        // Update is called once per frame
        public virtual void Update()
        {
            ImmunityWindowLogic();
        }

        protected void ImmunityWindowLogic()
        {
            if (isImmune && immunityWindow <= 0)
            {
                isImmune = false;
            }
            else
            {
                if (doFlash)
                {
                    doFlash = false;
                    StartCoroutine("FlashImmunityWindow");
                }
                immunityWindow -= Time.deltaTime;
            }
        }

        protected IEnumerator FlashImmunityWindow()
        {
            Debug.Log("FLASHING");
            bool isOriginal = true;
            int initOrder = rend.sortingOrder;
            int orderToChange = -1;

            while (isImmune)
            {
                if (isOriginal)
                {
                    this.rend.sortingOrder = orderToChange;
                    isOriginal = false;
                }
                else
                {
                    rend.sortingOrder = initOrder;
                    isOriginal = true;
                }
                yield return new WaitForSeconds(.1f);
            }
            rend.sortingOrder = initOrder;
        }

        public virtual void ReceiveDamage(int incomingDamage)
        {
            if (!isImmune)
            {
                this.hitAudioSource.Play();
                this.health -= incomingDamage;
                this.immunityWindow = this.immunityWindowInitial;
                isImmune = true;
                doFlash = true;
            }

            if (this.health <= 0)
            {
                isDead = true;
                Die();
            }

        }
        protected virtual void Die()
        {
            Debug.Log("Died");
        }

        public int GetHealth()
        {
            return this.health;
        }

        public void SetHealth(int hp)
        {
            this.health = hp;
        }

        virtual public void AddHealth(int hp)
        {
            this.health += hp;
        }
        public virtual void OnGUI()
        {

            Vector2 targetPos;
            targetPos = Camera.main.WorldToScreenPoint(transform.position);

            float yOffset = 60; // sprite size should be used in calculations
            float xOffset = 30;

            GUI.Box(new Rect(targetPos.x - xOffset, Screen.height - targetPos.y - yOffset, 60, 20), health + "/" + maxHealth);

        }
    }
}