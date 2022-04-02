using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System.Collections;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float currentHealth;
        bool dead;
        [SerializeField] float maxHealth;
        GameObject damageSource = null;
        bool restored = false;
        
        // Start is called before the first frame update
        void Start()
        {
            if (!restored) {
                maxHealth = GetComponent<BaseStats>().GetStat(Stat.HP);
                currentHealth = maxHealth;
                dead = false;
            }

            GetComponent<BaseStats>().onLevelUp += UpdateMaxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void takeDamage(GameObject instigator, float damage)
        {
            damageSource = instigator;
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
           // if (!dead)
            {
                currentHealth = 0;
                GetComponent<Animator>().SetTrigger("DeathTrigger");
                GetComponent<ActionScheduler>().cancelCurrentAction();
                dead = true;

                Debug.Log("Daed!");

                if (damageSource) {
                    Experience experience = damageSource.GetComponent<Experience>();

                    if (experience) {
                        BaseStats baseStats = GetComponent<BaseStats>(); // make sure to get stats from dying object to calc XP
                        experience.GainXP(baseStats.GetStat(Stat.XPReward));
                        print(damageSource.name+" defeated "+ gameObject.name+" gained " + baseStats.GetStat(Stat.XPReward)+"XP!");
                    }
                   
                }
            }
        }

        public bool isDead()
        {
            return dead;
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        public float GetMaxHealth()
        {
            return maxHealth;
        }

        public object CaptureState()
        {
            
            if (currentHealth == 0 && maxHealth == 0) {
                currentHealth = 45;
                maxHealth = 45;
                Debug.Log("Health::CaptureState: "+gameObject.name + " has 0/0");
            }

            return new HealthInfo(currentHealth, maxHealth, dead);
        }

        public void RestoreState(object state)
        {
            HealthInfo info = (HealthInfo)state;

            Debug.Log("Health Restore: " +name+": "+ info.current + "/" + info.max);
            currentHealth = info.current;
            maxHealth = info.max;
            dead = info.dead;

            if (dead) Die();

            restored = true;
        }

        public void UpdateMaxHealth()
        {
            float ratio = currentHealth / maxHealth;

            maxHealth = GetComponent<BaseStats>().GetStat(Stat.HP);
            currentHealth = ratio * maxHealth;
        }

        public float GetCurrentHealthRatio()
        {           
            return currentHealth / maxHealth;
        }
    }
}
