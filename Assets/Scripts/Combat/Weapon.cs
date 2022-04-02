using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        const string WEAPON_NAME = "Weapon"; // for searching

        [SerializeField] AnimatorOverrideController animatorOverride;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] float range = 2.0f;
        [SerializeField] float damage = 10.0f;
        [SerializeField] float attackPeriod = 1.5f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

         public void Spawn(Transform rHand, Transform lHand, Animator animator)
        {
            DestroyOldWeapon(rHand, lHand);

            if (weaponPrefab) {
                GameObject weapon;

                if (isRightHanded) {
                    weapon = Instantiate(weaponPrefab, rHand);
                    weapon.name = WEAPON_NAME;
                }
                else {
                    weapon = Instantiate(weaponPrefab, lHand);
                    weapon.name = WEAPON_NAME;
                }
            }

            if (animatorOverride) {
                animator.runtimeAnimatorController = animatorOverride;
                //Debug.Log("Animator: ")
            }
        }


        void DestroyOldWeapon(Transform rHand, Transform lHand)
        {
            Transform oldWeaponTransform = rHand.Find(WEAPON_NAME);

            if (!oldWeaponTransform) oldWeaponTransform = lHand.Find(WEAPON_NAME);

            if (oldWeaponTransform) {
                oldWeaponTransform.name = "old weapon";
                Destroy(oldWeaponTransform.gameObject);
            }
        }


        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject shooter)
        {
            Projectile p = (Projectile)Instantiate(projectile, rightHand.position, rightHand.rotation);
            p.SetTarget(target, shooter);
            p.SetLauncherDamage(damage);
            //Debug.Log("Projectile launched!");
        }

        public float GetDamage()
        {
            return damage;
        }

        public float GetRange()
        {
            return range;
        }

        public float GetAttackPeriod()
        {
            return attackPeriod;
        }
    }
}