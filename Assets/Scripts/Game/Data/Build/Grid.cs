using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Game.Data
{
    public struct Grid : IDisposable
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
            DataUtils.InitializeArray(ref arr, data);
        }

        public int this[int x, int y]
        {
            get => arr[x * Row + y];
            set => arr[x * Row + y] = value;
        }

        public void Dispose() => arr.Dispose();
    }
}