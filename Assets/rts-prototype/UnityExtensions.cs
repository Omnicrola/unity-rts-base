using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class UnityExtensions
    {
        public static Vector3 SetY(this Vector3 vec, float val)
        {
            return new Vector3(vec.x, val, vec.z);
        }

        public static Vector3 TranslateX(this Vector3 vec, float x)
        {
            return new Vector3(vec.x + x, vec.y, vec.z);
        }

        public static Vector3 TranslateY(this Vector3 vec, float y)
        {
            return new Vector3(vec.x, vec.y + y, vec.z);
        }

        public static Vector3 TranslateZ(this Vector3 vec, float z)
        {
            return new Vector3(vec.x, vec.y, vec.z + z);
        }

        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        public static Vector3 AsFloat(this Vector3Int vec)
        {
            return new Vector3(vec.x, vec.y, vec.z);
        }

        public static Vector3Int AsInt(this Vector3 vec)
        {
            return new Vector3Int((int) vec.x, (int) vec.y, (int) vec.z);
        }

        public static Vector3 Shift(this Vector3 vec, float amount)
        {
            return new Vector3(vec.x + amount, vec.y + amount, vec.z + amount);
        }

        public static List<GameObject> GetChildren(this Transform transform)
        {
            var children = new List<GameObject>();
            var childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                children.Add(transform.GetChild(i).gameObject);
            }

            return children;
        }

        public static List<GameObject> GetChildren(this GameObject obj)
        {
            return GetChildren(obj.transform);
        }

        public static Vector3 Rounded(this Vector3 vec)
        {
            return new Vector3(
                (int) Math.Round(vec.x),
                (int) Math.Round(vec.y),
                (int) Math.Round(vec.z)
            );
        }

        public static Vector3Int AsIntRounded(this Vector3 vec)
        {
            return new Vector3Int(
                (int) Math.Round(vec.x),
                (int) Math.Round(vec.y),
                (int) Math.Round(vec.z)
            );
        }

        public static Vector3 Absolute(this Vector3 vec)
        {
            return new Vector3(
                Math.Abs(vec.x),
                Math.Abs(vec.y),
                Math.Abs(vec.z)
            );
        }

        public static Vector2 WorldToScreen(this Camera camera, Vector3 worldPosition)
        {
            var screenPosition = camera.WorldToScreenPoint(worldPosition);
            return new Vector2(screenPosition.x, Screen.height - screenPosition.y);
        }
    }
}