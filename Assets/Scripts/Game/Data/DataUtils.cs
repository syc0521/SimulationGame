using Unity.Burst;
using Unity.Collections;

namespace Game.Data
{
    [BurstCompile]
    public static class DataUtils
    {
        [BurstCompile]
        public static void InitializeArray(ref NativeArray<int> arr, int data)
        {
            for (int i = 0; i < arr.Length; i += 4)
            {
                arr[i] = data;
                arr[i + 1] = data;
                arr[i + 2] = data;
                arr[i + 3] = data;
            }
        }
    }
}