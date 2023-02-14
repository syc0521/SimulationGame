using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Grid = Game.Data.Grid;

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
        
        public static bool HasBuilding(ref Grid grid, in float3 pos, int row, int col)
        {
            GetGridPos_Internal(pos, out var gridPos);
            if (gridPos.x < 0 || gridPos.y < 0 || gridPos.x > grid.Row || gridPos.y > grid.Col)
            {
                return true;
            }
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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float Angle(in float2 from, in float2 to)
        {
            var num = (float) math.sqrt(math.lengthsq(from) * (double) math.lengthsq(to));
            return num < 1.0000000036274937E-15 ? 0.0f : (float) math.acos((double) math.clamp(math.dot(from, to) / num, -1f, 1f)) * 57.29578f;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float SignedAngle(in float2 from, in float2 to) => Angle(from, to) * math.sign((float) ((double) from.x * (double) to.y - (double) from.y * (double) to.x));

        
        //以下为Vector3.Angle用math库替换的版本，用于BurstCompile
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public static float Angle(in float3 from, in float3 to)
        {
            var num = (float) math.sqrt(math.lengthsq(from) * (double) math.lengthsq(to));
            return num < 1.0000000036274937E-15 ? 0.0f : (float) math.acos((double) math.clamp(math.dot(from, to) / num, -1f, 1f)) * 57.29578f;
        }
        
        [BurstCompile]
        public static float SignedAngle(in float3 from, in float3 to, in float3 axis)
        {
            float num1 = Angle(from, to);
            float num2 = (float) ((double) from.y * (double) to.z - (double) from.z * (double) to.y);
            float num3 = (float) ((double) from.z * (double) to.x - (double) from.x * (double) to.z);
            float num4 = (float) ((double) from.x * (double) to.y - (double) from.y * (double) to.x);
            float num5 = math.sign((float) ((double) axis.x * (double) num2 + (double) axis.y * (double) num3 + (double) axis.z * (double) num4));
            return num1 * num5;
        }

        public static void BurstTest_UnBurst(ref int[,] testArr, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    testArr[j, i] = (int)(Mathf.Sin(i * size + j) * 10);
                }
            }
        }
        
        [BurstCompile]
        public static void BurstTest_Burst(ref NativeArray<int> testArr, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    testArr[j* size + i] = (int)(math.sin(i * size + j) * 10);
                }
            }
        }

    }
}