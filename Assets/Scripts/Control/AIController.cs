using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

// Change this to a finite state machine
namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5.0f;
        [SerializeField] float suspicionTime = 3.0f;
        [SerializeField] float dwellingTime = 2.0f;
        [SerializeField] PatrolPath patrolPath;
        [Range(0,1)][SerializeField] float speedFraction;
        [SerializeField] float maxSpeed = 6.0f;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        int targetWaypoint;
        float waypointTolerance = 0.5f;
        float dwellingCountdown = 0.0f;
        Fighter fighter;
        Movement.Mover mover;

        private void Start()
        {
            guardPosition = transform.position;
            targetWaypoint = 0;
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Movement.Mover>();
        }

        //int chaseCounter = 0;
        private void Update()
        {
            if (GetComponent<Health>().isDead()) return;

            GameObject player = GameObject.FindWithTag("Player"); // change this to any character

            if (inChaseRange(player))
            {
                //chase and attack

                AttackBehaviour(player);
                timeSinceLastSawPlayer = 0.0f;
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            { //stop
                PatrolBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        
        private void PatrolBehaviour()
        {
            if (!patrolPath)
            {

                mover.moveToPosition(guardPosition);
            }
            else
            {
                Vector3 waypointPos = patrolPath.getWaypointPosition(targetWaypoint);

                if (dwellingCountdown <= 0.0f)
                {
                    mover.moveToPosition(waypointPos);
                }

                if (Vector3.Distance(transform.position, waypointPos) < waypointTolerance)
                {
                    ++targetWaypoint;
                   // Debug.Log("New target waypoint: " + targetWaypoint);
                    dwellingCountdown = dwellingTime;
                }
            }

            dwellingCountdown -= Time.deltaTime;
        }

        private void SuspicionBehaviour()
        {
            mover.setSpeed(speedFraction*maxSpeed);
            GetComponent<Core.ActionScheduler>().cancelCurrentAction();
        }

        private void AttackBehaviour(GameObject player)
        {
            mover.setSpeed(maxSpeed);
            fighter.attack(player);
        }

        bool inChaseRange(GameObject chasee)
        {
            return Vector3.Distance(transform.position, chasee.transform.position) < chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
            //Gizmos.color = new Color(1, 0, 0, 0.5f);
            //Gizmos.DrawSphere(transform.position, GetComponent<Combat.Fighter>().getRange());
        }
    }


}