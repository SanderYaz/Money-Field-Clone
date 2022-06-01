using System;
using UnityEngine;

namespace Data
{
    public static class Structures
    {
        [Serializable]
        public class FieldData
        {
            public Field[] fields;
        }
        
        [Serializable]
        public class PlayerData
        {
            public int stackCapacityLevel;
            public int radiusLevel;
            public int currentMoney;
            public Float3 lastPositionInWorld;
            public Float3 lastEulerInWorld;
        }
        
        [Serializable]
        public class Field
        {
            public string Name;
            public bool Unlocked;
            public int TookMoney;
        }
        
        [Serializable]
        public struct Float3
        {
            public Float3(float x, float y, float z)
            {
                X = x;
                Y = y;
                Z = z;
            }
            
            public float X { get; set; }

            public float Y { get; set; }

            public float Z { get; set; }


            
            
            public static bool operator ==(Float3 f1, Float3 f2) 
            {
                return f1.Equals(f2);
            }

            public static bool operator !=(Float3 f1, Float3 f2) 
            {
                return !f1.Equals(f2);
            }

            public static Vector3 ToVector3(Float3 float3)
            {
                return new Vector3(float3.X, float3.Y, float3.Z);
            }
            public static Float3 FromVector3(Vector3 vector3)
            {
                return new Float3(vector3.x, vector3.y, vector3.z);
            }
            
            public bool Equals(Float3 other)
            {
                return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
            }

            public override bool Equals(object obj)
            {
                return obj is Float3 other && Equals(other);
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y, Z);
            }
        }
    }
}
