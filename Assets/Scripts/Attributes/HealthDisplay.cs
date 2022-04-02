using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        GameObject player;

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            health = player.GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            float current = health.GetCurrentHealth();
            float max = health.GetMaxHealth();
            float percentage = current / max * 100.0f;
            Text displayValue = GetComponent<Text>();

            displayValue.text = HealthDisplayString(health);// percentage.ToString("F1")+"% ("+current+"/"+max+")";


            Text targetDisplayValue = (Text)transform.GetChild(0).GetComponent<Text>();
            Health targetHealth = player.GetComponent<Fighter>().GetTarget();

            if (targetHealth)
                targetDisplayValue.text = targetHealth.name + ": " + HealthDisplayString(targetHealth);
            else
                targetDisplayValue.text = "";
        }

        string HealthDisplayString(Health h)
        {
            float current = h.GetCurrentHealth();
            float max = h.GetMaxHealth();
            float percentage = current / max * 100.0f;
            string displayString = percentage.ToString("F1") + "% (" + current + "/" + max + ")";

            return displayString;
        }
    }
}
