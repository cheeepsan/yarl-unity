using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarl.Controllers;
using Yarl.Flow;

public class PlayerWeaponAttackCollider : MonoBehaviour
{
    // Start is called before the first frame update
    readonly YarlModel model = Yarl.Util.Simulation.GetModel<YarlModel>();
    private PlayerController player;
    void Start()
    {
        player = model.player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "enemy" && player.GetDamageEnabled() == true) //bad design
        {
            EnemyController e = collider.gameObject.GetComponent<EnemyController>();
            e.ReceiveDamage(player.damage);
        }
    }
}
