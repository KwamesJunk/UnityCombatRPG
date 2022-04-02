using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }

        // use lookup table
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookupTable();
            
            float value = 0.0f;
            int lv = level;
            if (lv < 0) lv = 0; // kludgy protection 
            //Debug.Log(characterClass);
            try {
                value = lookupTable[characterClass][stat][lv];
            }
            catch (KeyNotFoundException e) {
                Debug.Log(e.Message + ": " + stat);
            }

            return value;
        }

        void BuildLookupTable()
        {
            if (lookupTable != null) return;

            Debug.Log("Building Lookup Table");
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progCharClass in characterClasses) {
                Dictionary<Stat, float[]> levelLookup = new Dictionary<Stat, float[]>();
                
                foreach (ProgressionStat progStat in progCharClass.stats) {
                    levelLookup[progStat.stat] = progStat.levels;
                    

                }
                lookupTable[progCharClass.characterClass] = levelLookup;
            }
        }

        public float[] GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookupTable();

            Debug.Log(stat+" ** "+characterClass);
            return lookupTable[characterClass][stat];
        }
    }
}