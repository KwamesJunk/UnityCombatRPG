using RPG.Combat;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    
    public class HeadsUpDisplay : MonoBehaviour
    {
        Health health;
        Health targetHealth;
        Experience xp;
        BaseStats playerStats;
        
        [SerializeField] Text hpDisplay;
        [SerializeField] Text targetHpDisplay;
        [SerializeField] Text xpDisplay;
        [SerializeField] Text levelDisplay;

        GameObject player;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (!player) {
                player = GameObject.FindGameObjectWithTag("Player");
                health = player.GetComponent<Health>();
                xp = player.GetComponent<Experience>();
                playerStats = player.GetComponent<BaseStats>();
            }

            hpDisplay.text = "HP: " + HealthDisplayString(health);
            xpDisplay.text = "XP: " + xp.GetXP();

            targetHealth = player.GetComponent<Fighter>().GetTarget();
            if (targetHealth) {
                targetHpDisplay.text = targetHealth.name + ": " + HealthDisplayString(targetHealth);
            }
            else {
                targetHpDisplay.text = "";
            }

            levelDisplay.text = "Level: " + (playerStats.GetLevel()+1);
        }

        string HealthDisplayString(Health h)
        {
            float current = h.GetCurrentHealth();
            float max = h.GetMaxHealth();
            float percentage = h.GetCurrentHealthRatio() * 100.0f;
            string displayString = percentage.ToString("F1") + "% (" + current + "/" + max + ")";

            return displayString;
        } 
    }
}