using System;
using System.Collections.Generic;
using UnityEngine;

namespace AntoineFoucault.Utilities
{
    public static class VectorExtensions
    {
        #region Set One Param

        #region Vector3
        public static void SetX(this ref Vector3 v, float x)
        {
            v = new Vector3(x, v.y, v.z);
        }
        public static void SetY(this ref Vector3 v, float y)
        {
            v = new Vector3(v.x, y, v.z);
        }
        public static void SetZ(this ref Vector3 v, float z)
        {
            v = new Vector3(v.x, v.y, z);
        }
        #endregion

        #region Vector2
        public static void SetX(this ref Vector2 v, float x)
        {
            v = new Vector2(x, v.y);
        }
        public static void SetY(this ref Vector2 v, float y)
        {
            v = new Vector2(v.x, y);
        }
        #endregion

        #region Vector3Int
        public static void SetX(this ref Vector3Int v, int x)
        {
            v = new Vector3Int(x, v.y, v.z);
        }
        public static void SetY(this ref Vector3Int v, int y)
        {
            v = new Vector3Int(v.x, y, v.z);
        }
        public static void SetZ(this ref Vector3Int v, int z)
        {
            v = new Vector3Int(v.x, v.y, z);
        }
        #endregion

        #region Vector2Int
        public static void SetX(this ref Vector2Int v, int x)
        {
            v = new Vector2Int(x, v.y);
        }
        public static void SetY(this ref Vector2Int v, int y)
        {
            v = new Vector2Int(v.x, y);
        }
        #endregion

        #endregion

        #region Add One Param

        #region Vector3
        public static void AddX(this ref Vector3 v, float x)
        {
            v = new Vector3(v.x + x, v.y, v.z);
        }
        public static void AddY(this ref Vector3 v, float y)
        {
            v = new Vector3(v.x, v.y + y, v.z);
        }
        public static void AddZ(this ref Vector3 v, float z)
        {
            v = new Vector3(v.x, v.y, v.z + z);
        }
        #endregion

        #region Vector2
        public static void AddX(this ref Vector2 v, float x)
        {
            v = new Vector2(v.x + x, v.y);
        }
        public static void AddY(this ref Vector2 v, float y)
        {
            v = new Vector2(v.x, v.y + y);
        }
        #endregion

        #region Vector3Int
        public static void AddX(this ref Vector3Int v, int x)
        {
            v = new Vector3Int(v.x + x, v.y, v.z);
        }
        public static void AddY(this ref Vector3Int v, int y)
        {
            v = new Vector3Int(v.x, v.y + y, v.z);
        }
        public static void AddZ(this ref Vector3Int v, int z)
        {
            v = new Vector3Int(v.x, v.y, v.z + z);
        }
        #endregion

        #region Vector2Int
        public static void AddX(this ref Vector2Int v, int x)
        {
            v = new Vector2Int(v.x + x, v.y);
        }
        public static void AddY(this ref Vector2Int v, int y)
        {
            v = new Vector2Int(v.x, v.y + y);
        }
        #endregion

        #endregion

        #region Set Two Params

        #region Vector2
        public static void SetXX(this ref Vector2 v, Vector2 t)
        {
            v = new Vector2(t.x, t.x);
        }
        public static void SetYY(this ref Vector2 v, Vector2 t)
        {
            v = new Vector2(t.y, t.y);
        }
        public static void SetZZ(this ref Vector2 v, Vector3 t)
        {
            v = new Vector2(t.z, t.z);
        }
        public static void SetXY(this ref Vector2 v, Vector2 t)
        {
            v = new Vector2(t.x, t.y);
        }
        public static void SetYX(this ref Vector2 v, Vector2 t)
        {
            v = new Vector2(t.y, t.x);
        }
        public static void SetXZ(this ref Vector2 v, Vector3 t)
        {
            v = new Vector2(t.x, t.z);
        }
        public static void SetZX(this ref Vector2 v, Vector3 t)
        {
            v = new Vector2(t.z, t.x);
        }
        public static void SetYZ(this ref Vector2 v, Vector3 t)
        {
            v = new Vector2(t.y, t.z);
        }
        public static void SetZY(this ref Vector2 v, Vector3 t)
        {
            v = new Vector2(t.z, t.y);
        }
        #endregion

        #region Vector3
        public static void SetXX(this ref Vector3 v, Vector2 t)
        {
            v = new Vector2(t.x, t.x);
        }
        public static void SetYY(this ref Vector3 v, Vector2 t)
        {
            v = new Vector2(t.y, t.y);
        }
        public static void SetZZ(this ref Vector3 v, Vector3 t)
        {
            v = new Vector2(t.z, t.z);
        }
        public static void SetXY(this ref Vector3 v, Vector2 t)
        {
            v = new Vector2(t.x, t.y);
        }
        public static void SetYX(this ref Vector3 v, Vector2 t)
        {
            v = new Vector2(t.y, t.x);
        }
        public static void SetXZ(this ref Vector3 v, Vector3 t)
        {
            v = new Vector2(t.x, t.z);
        }
        public static void SetZX(this ref Vector3 v, Vector3 t)
        {
            v = new Vector2(t.z, t.x);
        }
        public static void SetYZ(this ref Vector3 v, Vector3 t)
        {
            v = new Vector2(t.y, t.z);
        }
        public static void SetZY(this ref Vector3 v, Vector3 t)
        {
            v = new Vector2(t.z, t.y);
        }
        #endregion

        #region Vector2Int
        public static void SetXX(this ref Vector2Int v, Vector2Int t)
        {
            v = new Vector2Int(t.x, t.x);
        }
        public static void SetYY(this ref Vector2Int v, Vector2Int t)
        {
            v = new Vector2Int(t.y, t.y);
        }
        public static void SetZZ(this ref Vector2Int v, Vector3Int t)
        {
            v = new Vector2Int(t.z, t.z);
        }
        public static void SetXY(this ref Vector2Int v, Vector2Int t)
        {
            v = new Vector2Int(t.x, t.y);
        }
        public static void SetYX(this ref Vector2Int v, Vector2Int t)
        {
            v = new Vector2Int(t.y, t.x);
        }
        public static void SetXZ(this ref Vector2Int v, Vector3Int t)
        {
            v = new Vector2Int(t.x, t.z);
        }
        public static void SetZX(this ref Vector2Int v, Vector3Int t)
        {
            v = new Vector2Int(t.z, t.x);
        }
        public static void SetYZ(this ref Vector2Int v, Vector3Int t)
        {
            v = new Vector2Int(t.y, t.z);
        }
        public static void SetZY(this ref Vector2Int v, Vector3Int t)
        {
            v = new Vector2Int(t.z, t.y);
        }
        #endregion

        #region Vector3Int
        public static void SetXX(this ref Vector3Int v, Vector2Int t)
        {
            v = new Vector3Int(t.x, t.x);
        }
        public static void SetYY(this ref Vector3Int v, Vector2Int t)
        {
            v = new Vector3Int(t.y, t.y);
        }
        public static void SetZZ(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.z, t.z);
        }
        public static void SetXY(this ref Vector3Int v, Vector2Int t)
        {
            v = new Vector3Int(t.x, t.y);
        }
        public static void SetYX(this ref Vector3Int v, Vector2Int t)
        {
            v = new Vector3Int(t.y, t.x);
        }
        public static void SetXZ(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.x, t.z);
        }
        public static void SetZX(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.z, t.x);
        }
        public static void SetYZ(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.y, t.z);
        }
        public static void SetZY(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.z, t.y);
        }
        #endregion


        #endregion

        #region Set Three Params

        #region Vector3
        public static void SetXXX(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.x, t.x, t.x);
        }
        public static void SetYYY(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.y, t.y, t.y);
        }
        public static void SetZZZ(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.z, t.z, t.z);
        }
        public static void SetYXX(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.y, t.x, t.x);
        }
        public static void SetXYX(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.x, t.y, t.x);
        }
        public static void SetXXY(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.x, t.x, t.y);
        }
        public static void SetZXX(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.z, t.x, t.x);
        }
        public static void SetXZX(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.x, t.z, t.x);
        }
        public static void SetXXZ(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.x, t.x, t.z);
        }
        public static void SetXYY(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.x, t.y, t.y);
        }
        public static void SetYXY(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.y, t.x, t.y);
        }
        public static void SetYYX(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.y, t.y, t.x);
        }
        public static void SetZYY(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.z, t.y, t.y);
        }
        public static void SetYZY(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.y, t.z, t.y);
        }
        public static void SetYYZ(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.y, t.y, t.z);
        }
        public static void SetXZZ(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.x, t.z, t.z);
        }
        public static void SetZXZ(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.z, t.x, t.z);
        }
        public static void SetZZX(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.z, t.z, t.x);
        }
        public static void SetYZZ(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.y, t.z, t.z);
        }
        public static void SetZYZ(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.z, t.y, t.z);
        }
        public static void SetZZY(this ref Vector3 v, Vector3 t)
        {
            v = new Vector3(t.z, t.z, t.y);
        }
        #endregion

        #region Vector3Int
        public static void SetXXX(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.x, t.x, t.x);
        }
        public static void SetYYY(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.y, t.y, t.y);
        }
        public static void SetZZZ(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.z, t.z, t.z);
        }
        public static void SetYXX(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.y, t.x, t.x);
        }
        public static void SetXYX(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.x, t.y, t.x);
        }
        public static void SetXXY(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.x, t.x, t.y);
        }
        public static void SetZXX(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.z, t.x, t.x);
        }
        public static void SetXZX(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.x, t.z, t.x);
        }
        public static void SetXXZ(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.x, t.x, t.z);
        }
        public static void SetXYY(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.x, t.y, t.y);
        }
        public static void SetYXY(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.y, t.x, t.y);
        }
        public static void SetYYX(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.y, t.y, t.x);
        }
        public static void SetZYY(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.z, t.y, t.y);
        }
        public static void SetYZY(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.y, t.z, t.y);
        }
        public static void SetYYZ(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.y, t.y, t.z);
        }
        public static void SetXZZ(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.x, t.z, t.z);
        }
        public static void SetZXZ(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.z, t.x, t.z);
        }
        public static void SetZZX(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.z, t.z, t.x);
        }
        public static void SetYZZ(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.y, t.z, t.z);
        }
        public static void SetZYZ(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.z, t.y, t.z);
        }
        public static void SetZZY(this ref Vector3Int v, Vector3Int t)
        {
            v = new Vector3Int(t.z, t.z, t.y);
        }
        #endregion

        #endregion

    }

    public static class CollectionsExtensions
    {
        public static T GetRandomItem<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int index = UnityEngine.Random.Range(0, list.Count);
                list.ExchangeAt(index, i);
            }
            return list;
        }

        public static void ExchangeAt<T>(this IList<T> list, int i1, int i2)
        {
            T value = list[i1];
            list[i1] = list[i2];
            list[i2] = value;
        }
    }

    public static class TransformExtensions
    {
        public static void ResetTransform(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void SetTransform(this Transform transform, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;
        }
    }

}