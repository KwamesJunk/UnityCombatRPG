using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, Saving.ISaveable
    {
        Health target = null;
        
        Mover mover;
        float timeSinceLastAttack = Mathf.Infinity;
        
        [SerializeField] Transform rHandTransform = null;
        [SerializeField] Transform lHandTransform = null;
        //[SerializeField] Weapon defaultWeapon = null;
        [SerializeField] string defaultWeaponName = "Unarmed";
        [SerializeField] Weapon currentWeapon = null;
        
        // Start is called before the first frame update
        void Awake()
        {
            mover = GetComponent<Mover>();
            if (!currentWeapon) {
                LoadWeaponFromResources(defaultWeaponName);
            }
        }

        void Start()
        {
            //if (!currentWeapon) {
            //    LoadWeaponFromResources(defaultWeaponName);
            //}
        }

        // Update is called once per frame
        void Update()
        {
            if (target)
            {
                if (target.isDead())
                {
                    cancel();
                    return;
                }
                else
                {
                    //Debug.Log("Hitting " +target.name+" "+ target.getCurrentHealth());
                }

                if (!inRange(target.transform.position))
                {
                    mover.moveToGameObject(target.gameObject);
                }
                else
                {
                    mover.immediateStop();
                    mover.cancel();
                    attackBehaviour();
                }
            }

            //if (Input.GetKeyDown(KeyCode.Y)) {
            //    DropWeapon();
            //}
        }

        //void DropWeapon()
        //{
        //    EquipWeapon(defaultWeapon);
        //}
        
        public void EquipWeapon(Weapon newWeapon)
        {
            //print("Equip");
            
            currentWeapon = newWeapon;
            currentWeapon.Spawn(rHandTransform, lHandTransform, GetComponent<Animator>());
        }

        public void attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().startAction(this);
            target = combatTarget.GetComponent<Health>();
            GetComponent<Animator>().ResetTrigger("StopAttackTrigger");

           // Debug.Log("Attacked " + combatTarget.name + "!");
            //target.transform.position = (target.transform.position + new Vector3(0.5f, 0, 0));
        }

        public void clearTarget()
        {
            target = null;
        }

        public void cancel()
        {
            //Debug.Log("Cancelled Fighter for " + name);
            GetComponent<Animator>().SetTrigger("StopAttackTrigger");
            GetComponent<Animator>().ResetTrigger("AttackTrigger");
            target = null;
            timeSinceLastAttack = Mathf.Infinity;
        }

        // Animation event
        void Hit()
        {
            if (target)
            {
                if (currentWeapon) {
                    if (!currentWeapon.HasProjectile())
                        target.takeDamage(gameObject, currentWeapon.GetDamage());
                    else
                        currentWeapon.LaunchProjectile(rHandTransform, lHandTransform, target, gameObject);
                }
            }           
        }

        // Animation event
        void Shoot()
        {
            if (target && currentWeapon.HasProjectile()) {
                currentWeapon.LaunchProjectile(rHandTransform, lHandTransform, target, gameObject);
            }
        }

        void attackBehaviour()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack >= currentWeapon.GetAttackPeriod())
            {
                GetComponent<Animator>().SetTrigger("AttackTrigger");
                timeSinceLastAttack = 0.0f;
                transform.LookAt(target.transform); // look at the enemy (even if it's moving)
                //Debug.Log("attackBehaviour: lookAt "+name); 
            }


        }

        public string getName()
        {
            return "Fighter";
        }

        public bool inRange(Vector3 point)
        {
            return Vector3.Distance(transform.position, point) < currentWeapon.GetRange();
        }

        public object CaptureState()
        {
            if (currentWeapon)
                print("Capturing weapon " + currentWeapon.name);
            else
                print("Weapon is null! " + gameObject.name);

            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            LoadWeaponFromResources((string)state);
        }

        void LoadWeaponFromResources(string weaponName)
        {
            Weapon weapon = Resources.Load<Weapon>(weaponName);

            if (!weapon) print("Weapon not loaded: " + weaponName);

            EquipWeapon(weapon);
        }

        public Health GetTarget()
        {
            return target;
        }
    } // class
} // namespace
