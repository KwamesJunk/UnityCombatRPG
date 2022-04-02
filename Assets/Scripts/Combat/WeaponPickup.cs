using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon;
        [SerializeField] float respawnTime = 5.0f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(new Vector3(0.0f, 90.0f * Time.deltaTime, 0.0f ));

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") {
                print("trigger enter");
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnTime));
            }
        }

        IEnumerator HideForSeconds(float seconds)
        {
            SetVisible(false);

            yield return new WaitForSeconds(seconds);

            SetVisible(true);
        }

        
        void SetVisible(bool isVisible)
        {
            GetComponent<SphereCollider>().enabled = isVisible;

            foreach(Transform child in transform) { 
                child.gameObject.SetActive(isVisible);
            }
        }
    }
}
