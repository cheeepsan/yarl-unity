using System.Collections.Generic;
using ProceduralLevelGenerator.Unity.Generators.Common;
using ProceduralLevelGenerator.Unity.Generators.DungeonGenerator.PipelineTasks;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace Yarl.Dungeon.PostProcess
{
    [CreateAssetMenu(menuName = "Dungeon generator/Post Process", fileName = "FixTileSizePostProcess")]
    public class TileSizePostProcess : DungeonGeneratorPostProcessBase
    {
        [Range(0, 1)]
        public float gridCellSize = 0.34f;

        public override void Run(GeneratedLevel level, LevelDescription levelDescription) { 
            List<Tilemap> tilemaps = level.GetSharedTilemaps();
            foreach(Tilemap t in tilemaps) {
               t.layoutGrid.cellSize = new Vector3 (gridCellSize, gridCellSize, 1f);
            }
        }
    }
}