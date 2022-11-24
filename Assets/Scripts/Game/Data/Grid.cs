using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Game.Data
{
    [Serializable]
    public struct Grid<T> : IDisposable where T : struct
    {
        public int Row { get; private set; }
        public int Col { get; private set; }

        private NativeArray<T> arr;

        public Grid(int col, int row) : this()
        {
            Row = row;
            Col = col;
            arr = new NativeArray<T>(col * row, Allocator.Persistent);
        }

        public Grid(int col, int row, T data) : this(col, row)
        {
            Initialize(data);
        }

        public T this[int x, int y]
        {
            get => arr[x * Col + y];
            set => arr[x * Col + y] = value;
        }

        public void Initialize(T data)
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

        public void SetData(T data, int x, int y) => this[x, y] = data;

        public T GetData(int x, int y) => this[x, y];

        public void Dispose() => arr.Dispose();
    }
}