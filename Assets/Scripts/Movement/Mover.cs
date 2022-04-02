using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        NavMeshAgent navMeshAgent;
        float range = 0.2f;
        bool controlDisabled = false;

        // Start is called before the first frame update
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            navMeshAgent.enabled = !GetComponent<Health>().isDead();

            if (Vector3.Distance(transform.position, navMeshAgent.destination) <= range) { 
                navMeshAgent.isStopped = true;
            }

            updateAnimator();
        }

        public void moveToPosition(Vector3 dest)
        {
            if (controlDisabled) return;

            GetComponent<ActionScheduler>().startAction(this);
            navMeshAgent.isStopped = false;
            range = 0.5f;
            navMeshAgent.destination = dest;
            cancelled = false;
        }

        public void moveToGameObject(GameObject destObject)
        {
            moveToPosition(destObject.transform.position);
        }


        void updateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            GetComponent<Animator>().SetFloat("Forward", speed);
            GetComponent<Animator>().SetTrigger("StopAttackTrigger");
        }

        public void stop()
        {
            navMeshAgent.isStopped = true;
        }

        public void immediateStop()
        {
            navMeshAgent.destination = transform.position;
            navMeshAgent.isStopped = true;
        }

        bool cancelled = false;
        public void cancel()
        {
            if (!cancelled)
            {
                //Debug.Log("Cancelled Mover");
                stop();
                cancelled = true;
            }
        }

        public string getName()
        {
            return "Mover";
        }

        public void setSpeed(float speed)
        {
            navMeshAgent.speed = speed;
        }

        public void DisableControl()
        {
            immediateStop();
            controlDisabled = true;
        }

        public void EnableControl()
        {
            controlDisabled = false;
        }

        public object CaptureState()
        {
            SerializableVector3 v = new SerializableVector3(transform.position);

            return v;
        }

        public void RestoreState(object state)
        {
            SerializableVector3 v = (SerializableVector3)state;

            if (navMeshAgent) navMeshAgent.enabled = false;
            transform.position = v.GetVector3();
            if (navMeshAgent) navMeshAgent.enabled = true;
            GetComponent<ActionScheduler>().cancelCurrentAction();
        }
    } // class
} // namespace