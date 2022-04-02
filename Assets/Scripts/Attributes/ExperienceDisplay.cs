using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Text xpValue;
        GameObject player;

        // Start is called before the first frame update
        void Start()
        {
            xpValue = GetComponent<Text>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {

            xpValue.text = ""+player.GetComponent<Experience>().GetXP();
        }
    }
}