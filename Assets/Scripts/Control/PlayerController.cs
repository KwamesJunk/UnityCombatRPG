using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Ray lastRay;
        private Vector3 destination;
        private Mover mover;

        // Start is called before the first frame update
        void Start()
        {
            destination = transform.position;
            mover = GetComponent<Mover>();
        }

        // Update is called once per frame
        void Update()
        {
            if (GetComponent<Health>().isDead()) return;

            if (handleCombat()) return;

            if (handleMovement()) return;

            //Debug.Log("Nothing but the shadow of death. " + death++);
        }

        private bool handleCombat()
        {
            bool success = false;

            if (Input.GetMouseButton(0))
            {
                RaycastHit[] hits = Physics.RaycastAll(castRayFromMouse());

                foreach (RaycastHit hit in hits)
                {
                    CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                    if (target)
                    {
                        //Debug.Log("Hit enemy! " + enemiesHit++);
                        Health health = target.GetComponent<Health>();
                        if (!health) continue;
                        if (health.isDead()) continue;

                        GetComponent<Fighter>().attack(target.gameObject);
                        success = true;
                        Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
                        debugStar(destination, Color.red, 5.0f);
                        break;
                    }
                }
            } // end if getMouseButtonDown

            return success;
        } // end handleCombat

        public bool handleMovement()
        {
            bool success;

            success = moveToCursor();

            Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
            debugStar(destination, Color.red, 5.0f);

            return success;
        }

        //public bool moveToCursor()
        //{
        //    //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hitinfo;

        //    if (Physics.Raycast(castRayFromMouse(), out hitinfo))
        //    {
        //        if (Input.GetMouseButton(0))
        //        {
        //            lastRay = castRayFromMouse();// for debugging
        //            destination = hitinfo.point;
        //            mover.moveToPosition(destination);
        //            GetComponent<Fighter>().clearTarget();
        //        }

        //        return true;
        //    }

        //    return false; // raycast hit nothing
        //}

        public bool moveToCursor()
        {
            if (!Input.GetMouseButton(0)) return false;
            
            RaycastHit[] hitinfo = Physics.RaycastAll(castRayFromMouse());
            //RaycastHit[] RaycastAll(Ray ray, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);

            foreach(RaycastHit hit in hitinfo)
            {
                if (hit.collider.tag == "Terrain")
                {
                    destination = hit.point;
                    mover.moveToPosition(destination);
                    GetComponent<Fighter>().clearTarget();
                    return true;
                }
            }

            return false; // raycast hit nothing
        }

        void slowStop()
        {
            Vector3 difference = destination - transform.position;
            const float STOP_DIST = 2.0f;

            if (difference.magnitude > STOP_DIST)
            {
                destination = transform.position + difference.normalized * STOP_DIST;
                mover.moveToPosition(destination);
            }
        }

        void debugStar(Vector3 origin, Color colour, float size)
        {
            Debug.DrawLine(origin + Vector3.left * size, origin + Vector3.right * size, colour);
            Debug.DrawLine(origin + Vector3.up * size, origin + Vector3.down * size, colour);
            Debug.DrawLine(origin + Vector3.forward * size, origin + Vector3.right * size, colour);

        }

        Ray castRayFromMouse()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        
    } // class
} // namespace
