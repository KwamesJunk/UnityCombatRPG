using UnityEngine;

namespace RPG.Saving
{
    [System.Serializable]
    public class SerializableVector3
    {
        float x, y, z;

        public SerializableVector3(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }


        override public string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }

        public Vector3 GetVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}