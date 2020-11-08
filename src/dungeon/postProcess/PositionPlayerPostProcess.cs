using System.Collections;
using ProceduralLevelGenerator.Unity.Generators.Common;
using ProceduralLevelGenerator.Unity.Generators.DungeonGenerator.PipelineTasks;
using System.Collections.Generic;
using UnityEngine;
using Yarl.Controllers;
using Yarl.Flow;
using Yarl.Util;
using System.Linq;
using ProceduralLevelGenerator.Unity.Generators.Common.Rooms;

namespace Yarl.Dungeon.PostProcess
{
    [CreateAssetMenu(menuName = "Dungeon generator/Player Pos", fileName = "PositionPlayerPostProcess")]
    public class PositionPlayerPostProcess : DungeonGeneratorPostProcessBase
    {
        readonly YarlModel model = Simulation.GetModel<YarlModel>();
        public override void Run(GeneratedLevel level, LevelDescription levelDescription) {
            bool playerPositioned = false;
            bool keyIsSet = false;

            PlayerController player = model.player;
            //GameObject enemyPrefab = model.enemyPrefab;
            List<GameObject> enemyPrefabPool = model.enemyPool;
            List<GameObject> pickupPrefabPool = model.pickupPool;

            //GameObject healthPickup = model.healthPickup;
            //GameObject speedPickup = model.speedPickup;
            //GameObject damagePickup = model.damagePickup;
            GameObject keyPickup = model.keyPickup;
            GameObject ladderPrefab = model.ladder;

            List<RoomInstance> rooms = level.GetRoomInstances();
            List<EnemyController> enemiesAcc = new List<EnemyController>();

            int currentLevel = player.GetCurrentLevel();
            int difficultyMultiplier = 0;
           
            if (currentLevel >= 5)
            {
                difficultyMultiplier = (currentLevel / 5);
            }

            /** too lazy to break into methods **/
            foreach (RoomInstance room in rooms)
            {
                bool playerInSameRoom = false;
                if(!playerPositioned)
                {
                    Transform spawnPoint = room.RoomTemplateInstance.transform.Find("spawn");

                    if (spawnPoint != null)
                    {
                        player.transform.position = spawnPoint.transform.position;
                        playerPositioned = true;
                        playerInSameRoom = true;
                        //break;
                    }
                }


                if(!playerInSameRoom)
                {
                    Transform enemySpawnPoints = room.RoomTemplateInstance.transform.Find("enemies");
                    if(enemySpawnPoints != null)
                    {
                        int amountOfChildren = enemySpawnPoints.childCount;
                        for (int i = 0; i < amountOfChildren; i++)
                        {
                            GameObject enemyPrefab = GetRandomEnemyFromPool(enemyPrefabPool); 
                            GameObject o = Instantiate(enemyPrefab, enemySpawnPoints.GetChild(i).position, Quaternion.identity);
                            EnemyController enemy = o.GetComponent<EnemyController>();
                            enemy.maxHealth += (difficultyMultiplier * 2);
                            enemy.damage += difficultyMultiplier;
                            enemiesAcc.Add(enemy);
                            if (!keyIsSet)
                            {
                                
                                GameObject key = Instantiate(keyPickup, enemySpawnPoints.GetChild(i).position, Quaternion.identity);
                                key.SetActive(false);
                                enemy.pickup = key;
                                keyIsSet = true;
                            }
                        }
                    }
                }

            }
            //massive foreach ends

            AddPickupToEnemyPool(pickupPrefabPool, enemiesAcc);

            rooms.Reverse();
            foreach (RoomInstance room in rooms)
            {
                var exitPos = room.RoomTemplateInstance.transform.Find("exitPos");
                if(exitPos != null)
                {
                    Instantiate(ladderPrefab, exitPos.position, Quaternion.identity);
                    break;
                }
            }

        }

        private GameObject GetRandomEnemyFromPool(List<GameObject> enemyPrefabPool)
        {
            var randNum = UnityEngine.Random.Range(0, enemyPrefabPool.Count());
            return enemyPrefabPool[randNum];
        }

        private void AddPickupToEnemyPool(List<GameObject> pickups, List<EnemyController> enemies)
        {
            var enemyCount = enemies.Count();
            int pickupEveryNEnemies = (int)(enemyCount * 0.4);
            if (pickupEveryNEnemies == 0)
            {
                pickupEveryNEnemies = 1; 
            }
            for(int i = 0; i < enemyCount; i++)
            {
                if (i % pickupEveryNEnemies == 0)
                {
                    var enemyToAddPicupTo = enemies[i];
                    if (enemyToAddPicupTo.pickup == null)
                    {
                        var randNum = UnityEngine.Random.Range(0, pickups.Count());
                        GameObject pickupPrefab = pickups[randNum];
                        GameObject pickup = Instantiate(pickupPrefab, enemyToAddPicupTo.transform.position, Quaternion.identity);
                        pickup.SetActive(false);
                        enemyToAddPicupTo.pickup = pickup;
                    }
                }
            }
        }
    }
}