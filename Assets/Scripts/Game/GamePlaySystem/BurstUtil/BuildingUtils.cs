using Game.Data;
using Unity.Burst;
using Unity.Mathematics;

namespace Game.GamePlaySystem.BurstUtil
{
    [BurstCompile]
    public static class BuildingUtils
    {
        [BurstCompile]
        private static void GetBlockPos_Internal(in float3 pos, out float3 returnPos)
        {
            var xzpos = math.floor(pos.xz);
            returnPos = new float3(xzpos[0], pos.y, xzpos[1]);
        }

        public static float3 GetBlockPos(float3 pos)
        {
            GetBlockPos_Internal(pos, out var outPos);
            return outPos;
        }

        [BurstCompile]
        private static void GetGridPos_Internal(in float3 pos, out int2 gridPos)
        {
            gridPos = math.int2(math.floor(pos.xz));
        }

        public static int2 GetGridPos(float3 pos)
        {
            GetGridPos_Internal(pos, out var gridPos);
            return gridPos;
        }

        [BurstCompile]
        public static void SetGridData(ref Grid grid, in float3 pos, int row, int col, int value)
        {
            GetGridPos_Internal(pos, out var gridPos);
            for (int i = gridPos[0]; i < gridPos[0] + row; i++)
            {
                for (int j = gridPos[1]; j < gridPos[1] + col; j++)
                {
                    grid[i, j] = value;
                }
            }
        }

        //[BurstCompile]
        public static bool HasBuilding(ref Grid grid, in float3 pos, int row, int col)
        {
            GetGridPos_Internal(pos, out var gridPos);
            for (int i = gridPos[0]; i < gridPos[0] + row; i++)
            {
                for (int j = gridPos[1]; j < gridPos[1] + col; j++)
                {
                    if (grid[i, j] != -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}