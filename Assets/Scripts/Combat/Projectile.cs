using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        Health target;
        [SerializeField] float speed = 5.0f;
        [SerializeField] int damage = 5;
        [SerializeField] bool homing = false;
        [SerializeField] GameObject impactEffect = null;
        [SerializeField] GameObject[] destroyOnImpact;
        GameObject instigator = null;

        float launcherDamage = 0;
        const float LIFETIME_AFTER_IMPACT = 0.2f;

        //private void Awake()
        //{
        //    target = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        //    transform.LookAt(target.transform.position + Vector3.up);
        //}

        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, 5.0f);
        }

        // Update is called once per frame
        void Update()
        {
            if (homing && !target.isDead()) transform.LookAt(target.transform.position + Vector3.up); // homing missile
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health newTarget, GameObject shooter)
        {
            target = newTarget;
            transform.LookAt(target.transform.position + Vector3.up);
            instigator = shooter;
        }

        private void OnTriggerEnter(Collider other)
        {
            Health opponent = other.GetComponent<Health>();
            if (opponent == target) {
                if (!opponent.isDead()) {
                    if (impactEffect) {
                        GameObject impact = Instantiate(impactEffect, transform.position, transform.rotation);
                        Destroy(impact, 0.5f);
                    }

                    foreach (GameObject effect in destroyOnImpact) {
                        Destroy(effect);
                    }
                    
                    Destroy(gameObject, LIFETIME_AFTER_IMPACT);
                }

                opponent.takeDamage(instigator, damage + launcherDamage);
            }

            
        }

        public void SetLauncherDamage(float dmg)
        {
            launcherDamage = dmg;
        }
    }
}