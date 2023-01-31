using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Game.Data
{
    [BurstCompile]
    public struct Grid
    {
        public int Row { get; private set; }
        public int Col { get; private set; }

        private NativeArray<int> arr;

        public Grid(int col, int row) : this()
        {
            Row = row;
            Col = col;
            arr = new NativeArray<int>(col * row, Allocator.Persistent);
        }

        public Grid(int col, int row, int data) : this(col, row)
        {
            Initialize(data);
        }

        public int this[int x, int y]
        {
            get => arr[x * Col + y];
            set => arr[x * Col + y] = value;
        }
        
        [BurstCompile]
        public void Initialize(int data)
        {
            for (int i = 0; i < Row * Col; i++)
            {
                arr[i] = data;
            }
            /*var memset = new MemsetNativeArray<T>
            {
                Source = arr,
                Value = data
            };
            memset.Run(Row * Col);*/
        }
        
        public void SetData(int data, int x, int y) => this[x, y] = data;

        public int GetData(int x, int y) => this[x, y];

        public void Dispose() => arr.Dispose();
    }
}