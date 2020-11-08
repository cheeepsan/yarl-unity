using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarl.Controllers;
using Yarl.Flow;
//https://weeklyhow.com/unity-2d-melee-combat-tutorial/
public class PlayerAttack : MonoBehaviour
{

    public List<GameObject> explosion;
    public GameObject weaponSpriteRndrObj;

    public float attackTime = 2;
    public float startTimeAttack = 2;
    
    public float attackRange = 20;
    public LayerMask enemies;

    public AudioSource attackAudioSource;

    private Animator anim;
    private Camera cam;
    private CircleCollider2D cl2d;

    private PlayerController player;
    private SpriteRenderer weaponSpriteRndr;
    private BoxCollider2D weaponCollider;

    private bool isExploding = false;

    readonly YarlModel model = Yarl.Util.Simulation.GetModel<YarlModel>();
    // Start is called before the first frame update
    void Start()
    {
        player = model.player;
        cam = player.camera;
        anim = GetComponent<Animator>();
        cl2d = GetComponent<CircleCollider2D>();
        this.weaponSpriteRndr = weaponSpriteRndrObj.GetComponent<SpriteRenderer>();
        this.weaponCollider = weaponSpriteRndrObj.GetComponent<BoxCollider2D>();
        foreach (GameObject expl in explosion)
        {
            expl.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        TriggerExplosion();
        if (attackTime <= 0)
        {
            RotateWeapon();
            if (Input.GetKey(KeyCode.Mouse1))
            {
                attackTime = startTimeAttack - (float)this.player.speed * 0.1f;

                RotateWeaponOnAttack();
                
            }

        }
        else
        {
            attackTime -= Time.deltaTime;
            
        }
    }

    private void TriggerExplosion()
    {
        if (Input.GetKey(KeyCode.Q) && player.GetBombAmount() > 0 && isExploding == false)
        {
            
            //anim.SetBool("explosion", true); 
            var damageList = Physics2D.OverlapCircleAll(transform.position, cl2d.radius, enemies);
            foreach(Collider2D damage in damageList)
            {
                var enemyObj = damage.gameObject;

                var enemy = enemyObj.GetComponent<EnemyController>();
                enemy.ReceiveDamage(player.damage / 2);
            }
            StartCoroutine("ShowExplosionSprites");

            player.DecreaseBonbAmountByOne();
            
            //anim.SetBool("explosion", false);


        }
    }

    private void RotateWeaponOnAttack()
    {
        
        var mousePos = Input.mousePosition;
        var worldMousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        var dir = worldMousePos - weaponSpriteRndr.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        IEnumerator coroutine = RotateWeaponOnAttackRoutine(angle);
        StartCoroutine(coroutine);
    }

    private void RotateWeapon()
    {
        var mousePos = Input.mousePosition;
        var worldMousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        var dir = worldMousePos - weaponSpriteRndr.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        weaponSpriteRndr.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected IEnumerator RotateWeaponOnAttackRoutine(float angle)
    {
        attackAudioSource.Play();
        player.SetDamageEnabled(true);
        for(int i = 0; i < 120; i += 10)
        {
            weaponSpriteRndr.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            angle -= 10;
            yield return new WaitForSeconds(0.005f);
        }

        player.SetDamageEnabled(false);
    }

    protected IEnumerator ShowExplosionSprites()
    {
        isExploding = true;
        foreach (GameObject expl in explosion)
        {
            expl.SetActive(true);
        }
        yield return new WaitForSeconds(0.3f);
        foreach (GameObject expl in explosion)
        {
            expl.SetActive(false);
        }
        isExploding = false;
    }

    void OnGUI()
    {
        if (this.attackTime > 0)
        {
            Vector2 targetPos;
            targetPos = Camera.main.WorldToScreenPoint(transform.position);

            float yOffset = 90; // sprite size should be used in calculations
            float xOffset = 60;
            var cooldown = System.String.Format("{0:0.00}", this.attackTime);
            var rectangle = new Rect(targetPos.x - xOffset, Screen.height - targetPos.y - yOffset, 120, 20);

            GUI.Box(rectangle, "Att cooldown: " + cooldown);
        }


    }
    // garbage
    //private Vector2 getAttackVector()
    //{
    //    var mousePos = Input.mousePosition;
    //    var worldMousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
    //
    //    var playerPos = transform.position;
    //
    //    var v = worldMousePos - playerPos;
    //    v = v.normalized;
    //    v = v * 4f;
    //
    //    v = v + playerPos;
    //
    //    return new Vector2(v.x, v.y);
    //}
    //private (Vector2, Vector2) calculateAttackVectors()
    //{
    //    var mousePos = Input.mousePosition;
    //    var worldMousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
    //
    //    var playerPos = transform.position;
    //
    //    var v = worldMousePos - playerPos;
    //    v = v.normalized;
    //    v = v * 4f;
    //
    //    v = v + playerPos;
    //    var pos2 = Quaternion.Euler(0, 0, -10) * v;
    //    var pos3 = Quaternion.Euler(0, 0, 10) * v;
    //
    //    return (new Vector2(pos2.x, pos2.y), new Vector2(pos3.x, pos3.y));
    //}
}
