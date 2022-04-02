using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Stats {
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float xp;
        //[SerializeField] int level;

        // Start is called before the first frame update
        public void Start()
        {
           //GetComponent<BaseStats>().CalcluateLevel();
        }

        public void GainXP(float exp)
        {
            xp += exp;
            //Debug.Log("Level: " + GetComponent<BaseStats>().GetLevel());
            OnXPGained();
        }

        //public delegate void XPGainedDelegate();
        public event Action OnXPGained;

        public float GetXP()
        {
            return xp;
        }

        public object CaptureState()
        {
            return xp;
        }

        public void RestoreState(object state)
        {
            Debug.Log("Restoring XP for " + gameObject.name);
            
            
            xp = (float)state;
            Debug.Log(xp + "XP");

            if (OnXPGained != null) {
                Debug.Log("OnXPGained is null, somehow");

                OnXPGained();
            }
        }
    }
}
