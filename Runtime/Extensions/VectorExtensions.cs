using UnityEngine;

namespace MVC.Runtime.Extensions
{
    public static class VectorExtensions
    {
        public static bool ContainsInBetween(this Vector2Int vector2Int, int value)
        {
            return vector2Int.x <= value && value < vector2Int.y;
        }

        public static bool ContainsInBetween(this Vector2 vector2, int value)
        {
            return vector2.x < value && value < vector2.y;
        }
        
        public static Vector3 MultipliedBy(this Vector3 vector, float value)
        {
            return new Vector3(vector.x * value, vector.y * value, vector.z * value);
        }

        public static Vector3 MultipliedBy(this Vector2 vector, float value)
        {
            return new Vector2(vector.x * value, vector.y * value);
        }
        
        public static Vector2 ToVector2ByXZ(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }

        public static Vector2 ToVector2ByXY(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }
    
        public static Vector2 ToVector2ByYX(this Vector3 vector3)
        {
            return new Vector2(vector3.y, vector3.x);
        }

        public static Vector2 ToVector2ByYZ(this Vector3 vector3)
        {
            return new Vector2(vector3.y, vector3.z);
        }

        public static Vector2 ToVector2ByZX(this Vector3 vector3)
        {
            return new Vector2(vector3.z, vector3.x);
        }

        public static Vector2 ToVector2ByZY(this Vector3 vector3)
        {
            return new Vector2(vector3.z, vector3.y);
        }

        public static Vector3 ToVector3OnXY(this Vector2 vector2)
        {
            return new Vector3(vector2.x, vector2.y, 0);
        }

        public static Vector3 ToVector3OnXZ(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0, vector2.y);
        }

        public static Vector3 ToVector3OnZX(this Vector2 vector2)
        {
            return new Vector3(vector2.y, 0, vector2.x);
        }

        public static Vector3 ToVector3OnYX(this Vector2 vector2)
        {
            return new Vector3(vector2.y, vector2.x, 0);
        }

        public static Vector3 ToVector3OnYZ(this Vector2 vector2)
        {
            return new Vector3(0, vector2.x, vector2.y);
        }

        public static Vector3 ToVector3OnZY(this Vector2 vector2)
        {
            return new Vector3(0, vector2.y, vector2.x);
        }
        
        public static Vector3 ScaleWith(this Vector3 vector, Vector3 scaler)
        {
            return Vector3.Scale(vector, scaler);
        }

        public static Vector2 ScaleWith(this Vector2 vector, Vector3 scaler)
        {
            return Vector2.Scale(vector, scaler);
        }

        public static Vector2 WithX(this Vector2 vector, float x)
        {
            return new Vector2(x, vector.y);
        }

        public static Vector2 WithY(this Vector2 vector, float y)
        {
            return new Vector2(vector.x, y);
        }

        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }

        public static Vector3 WithX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }

        public static Vector3 WithY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }

        public static Vector3 WithZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }
        
        
        public static Vector3 AddX(this Vector3 vector, float x)
        {
            return new Vector3(vector.x + x, vector.y, vector.z);
        }

        public static Vector3 AddY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, vector.y+y, vector.z);
        }

        public static Vector3 AddZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, vector.z +z);
        }
        
        public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(
                (float) (x == null ? vector.x : vector.x + x),
                (float) (y == null ? vector.y : vector.y + y),
                (float) (z == null ? vector.z : vector.z + z));
        }
        
        public static Vector3 AddAll(this Vector3 vector, float value)
        {
            return new Vector3(vector.x + value, vector.y + value, vector.z + value);

        }
    }
}