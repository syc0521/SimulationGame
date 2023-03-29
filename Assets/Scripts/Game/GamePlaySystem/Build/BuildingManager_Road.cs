using Game.GamePlaySystem.GameState;

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

        public bool CanUndoRoad()
        {
            if (buildStateMachine.GetCurrentState() is AddRoadState state)
            {
                return state.CanUndo();
            }

            return false;
        }
        
        public bool CanRedoRoad()
        {
            if (buildStateMachine.GetCurrentState() is AddRoadState state)
            {
                return state.CanRedo();
            }

            return false;
        }
    }
}