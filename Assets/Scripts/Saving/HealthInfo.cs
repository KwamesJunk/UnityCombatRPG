using System.Collections;
using UnityEngine;

namespace RPG.Saving
{
    [System.Serializable]
    public class HealthInfo

    {
        public float current, max;
        public bool dead;
        
        public HealthInfo(float c, float m, bool d)
        {
            current = c;
            max = m;
            dead = d;
        }
    }
}