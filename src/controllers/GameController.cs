using ProceduralLevelGenerator.Unity.Generators.Common.LevelGraph;
using ProceduralLevelGenerator.Unity.Generators.DungeonGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarl.Flow;
using Yarl.Util;
using static Yarl.Util.Simulation;
namespace Yarl.Controllers 
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }
        public YarlModel model = Simulation.GetModel<YarlModel>();

        public GameObject menu;

        public DungeonGenerator generator;
        public LevelGraph baseLevelGraph;
        public bool generateOnStart = false;
        void OnEnable()
        {
            
            this.menu.SetActive(false);
            Instance = this;
            if (this.generateOnStart)
            {
                this.generator.SetLevelGraphToConfig(this.baseLevelGraph);
                generator.Generate();
               
            }
            
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(this.menu.activeInHierarchy)
                {
                    Time.timeScale = 1;
                    this.menu.SetActive(false);
                } else
                {
                    Time.timeScale = 0;
                    this.menu.SetActive(true);
                }

            }
        }

        public void LoadNextLevel()
        {
            this.RemoveLevelLeftovers();
            this.generator.Generate();
        }

        //public void LoadBossLevel()
        //{
        //    Debug.Log("BOSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS");
        //    GameObject a = GameObject.Get
        //    this.RemoveLevelLeftovers();
        //    this.bossLeveleGenerator.SetLevelGraphToConfig(this.bossLevelGraph);
        //}

        private void RemoveLevelLeftovers()
        {
            GameObject[] leftoverEnemies = GameObject.FindGameObjectsWithTag("enemy");
            foreach (GameObject enemy in leftoverEnemies)
            {
                Destroy(enemy);
            }            
            
            GameObject[] leftoverPickups = GameObject.FindGameObjectsWithTag("pickup");
            foreach (GameObject pickup in leftoverPickups)
            {
                Destroy(pickup);
            }
        }

        public void Test()
        {
            Debug.Log("test");
        }
    }
}