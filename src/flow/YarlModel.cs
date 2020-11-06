using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarl.Controllers;
namespace Yarl.Flow
{
    
    [System.Serializable]
    public class YarlModel
    {

        //public Cinemachine.CinemachineVirtualCamera virtualCamera;
        public PlayerController player;
        public Transform spawnPoint;

        //public GameObject enemyPrefab;
        public List<GameObject> enemyPool;
        public List<GameObject> pickupPool;

        public GameObject healthPickup;
        public GameObject speedPickup;
        public GameObject damagePickup;
        public GameObject keyPickup;

        public GameObject ladder;
    }
}