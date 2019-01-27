using System;
using UnityEngine;

public static class VoxelUtil
{
    public static void CubeWalk(int size, Action<int, int, int, int> action)
    {
        for (int index = 0, x = 0; x < size; x++)
        {
            for (var z = 0; z < size; z++)
            {
                for (var y = 0; y < size; y++)
                {
                    action(x, y, z, index);
                    index++;
                }
            }
        }
    }

    public static void CubeOverwalk(int size, Action<int, int, int, int> action)
    {
        for (int index = 0, x = 0; x <= size; x++)
        {
            for (var z = 0; z <= size; z++)
            {
                for (var y = 0; y <= size; y++)
                {
                    action(x, y, z, index);
                    index++;
                }
            }
        }
    }

    public static void Walk3(int xMax, int yMax, int zMax, Action<int, int, int> action)
    {
        for (var x = 0; x < xMax; x++)
        {
            for (var z = 0; z < zMax; z++)
            {
                for (var y = 0; y < yMax; y++)
                {
                    action(x, y, z);
                }
            }
        }
    }

    public static void Walk2(int xMax, int yMax, Action<int, int> action)
    {
        for (var x = 0; x < xMax; x++)
        {
            for (var y = 0; y < yMax; y++)
            {
                action(x, y);
            }
        }
    }

    public static void SquareWalker(int size, Action<int, int, int> action)
    {
        for (int index = 0, x = 0; x <= size; x++)
        {
            for (var y = 0; y <= size; y++)
            {
                action(x, y, index);
                index++;
            }
        }
    }

    public static bool IsInGrid(this Vector3Int position, int gridSize)
    {
        return position.x >= 0 && position.y >= 0 && position.z >= 0 &&
               position.x < gridSize && position.y < gridSize && position.z < gridSize;
    }

    public static void CopyCubeGrid<T>(T[] source, T[] copy)
    {
        double cube = 1.0 / 3.0;
        var sourceGridSize = (int) Math.Pow(source.Length, cube);
        var copyGridSize = (int) Math.Pow(copy.Length, cube);
        var gridSize = Mathf.Min(sourceGridSize, copyGridSize);

        CubeWalk(gridSize, (x, y, z, index) =>
        {
            var sourceIndex = x + y * sourceGridSize + z * sourceGridSize * sourceGridSize;
            var copyIndex = x + y * copyGridSize + z * copyGridSize * copyGridSize;
            copy[copyIndex] = source[sourceIndex];
        });
    }
}