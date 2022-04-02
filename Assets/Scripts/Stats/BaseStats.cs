using RPG.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        const int LEVEL_UNINITIALIZED = -1;

        [Range(0, 99)][SerializeField] int startingLevel = 0;
        [SerializeField] int currentLevel = LEVEL_UNINITIALIZED;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpEffect = null;

        public event Action onLevelUp;

        private void Start()
        {
            currentLevel = CalcluateLevel();

            Experience experience = GetComponent<Experience>();
            if (experience) {
                experience.OnXPGained += UpdateLevel;
            }
        }

        public float GetStat(Stat stat)
        {
            //Debug.Log("CurrentLevel: " + currentLevel);
            return progression.GetStat(stat, characterClass, currentLevel);
        }

        public float GetXP()
        {
            return GetComponent<Experience>().GetXP();
        }

        void UpdateLevel()
        {
            Debug.Log("Update Level");
            Experience experience = GetComponent<Experience>();
            if (!experience) return;

            //Health health = GetComponent<Health>();
            //float ratio = health.GetCurrentHealthRatio();

            int newLevel = CalcluateLevel();
            if (newLevel != currentLevel) {
                
                currentLevel = newLevel;
                //GetComponent<Health>().UpdateMaxHealth();
                //health.SetCurrentHealthByRatio(ratio);
                Instantiate(levelUpEffect, transform);
                onLevelUp();
            }
        }

        public int CalcluateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (!experience) return startingLevel;

            float[] xpToLevelUp = progression.GetLevels(Stat.XpToLevelUp, characterClass);
            if (xpToLevelUp.Length <= 0) return -1;


            float currentXp = experience.GetXP();
            
            int level = 0;

            // assumes that list of xp requirements is in ascending order
            for (level = 0; level < xpToLevelUp.Length; level++) {
                if (currentXp < xpToLevelUp[level]) {
                    return level;
                }
            }

            return level - 1; // level is at xpToLevelUp.Length+1
        }

        public int GetLevel()
        {
            if (currentLevel == LEVEL_UNINITIALIZED) return CalcluateLevel();

            return currentLevel;
        }
    }
}