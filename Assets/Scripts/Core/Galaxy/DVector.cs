using UnityEngine;

namespace Core.Galaxy
{
    [System.Serializable]
    public struct DVector
    {
        public float x, y, z;

        public DVector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static DVector operator+ (DVector b, DVector c)
        {
            return new DVector(b.x + c.x, b.y + c.y, b.z + c.z);
        }

        public static DVector operator *(DVector b, int c)
        {
            return new DVector(b.x * c, b.y * c, b.z * c);
        }

        public Vector3 ToVector()
        {
            return new Vector3(x, y, z);
        }

        public static DVector FromVector3(Vector3 vector)
        {
            return new DVector(vector.x, vector.y, vector.z);
        }
    
        public string Log()
        {
            return $"[{x:F2},{y:F2},{z:F2}]";
        }

        public float Dist(DVector second)
        {
            return Vector3.Distance(this.ToVector(), second.ToVector());
        }
    }
}
