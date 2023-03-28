using Game.GamePlaySystem.GameState;
using Unity.Mathematics;

namespace Game.GamePlaySystem.Build
{
    public partial class BuildingManager
    {
        public void ConstructRoad()
        {
            buildStateMachine.ChangeState<AddRoadState>();
        }

        public void UndoRoad()
        {
            if (buildStateMachine.GetCurrentState() is AddRoadState state)
            {
                state.UndoRoad();
            }
        }

        public void RedoRoad()
        {
            if (buildStateMachine.GetCurrentState() is AddRoadState state)
            {
                state.RedoRoad();
            }
        }
    }
}