using ProceduralLevelGenerator.Unity.Attributes;
using ProceduralLevelGenerator.Unity.Generators.Common.LevelGraph;
using ProceduralLevelGenerator.Unity.Generators.DungeonGenerator.Configs;
using ProceduralLevelGenerator.Unity.Generators.DungeonGenerator.PipelineTasks;
using ProceduralLevelGenerator.Unity.Pipeline;

namespace ProceduralLevelGenerator.Unity.Generators.DungeonGenerator
{
    /// <summary>
    /// Dungeon generator. All logic is currently inherited from DungeonGeneratorBase.
    /// </summary>
    public class DungeonGenerator : DungeonGeneratorBase
    {

        [Expandable]
        public FixedLevelGraphConfig FixedLevelGraphConfigCustom;
        protected override IPipelineTask<DungeonGeneratorPayload> GetInputTask()
        {
            return new FixedLevelGraphInputTask<DungeonGeneratorPayload>(FixedLevelGraphConfigCustom);
        }
       
        public void SetLevelGraphToConfig(LevelGraph graph)
        {
            FixedLevelGraphConfigCustom.LevelGraph = graph;
        }
    }
}