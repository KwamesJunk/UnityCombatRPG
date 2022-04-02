using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction current = null;

        public void startAction(IAction action)
        {
            if (action != current)
            {
                if (current != null)
                {
                    current.cancel();
                    
                    //if (name == "Player")
                    //Debug.Log("ActionScheduler: Cancelling " + current.getName());
                }
                else
                {
                    //if (name == "Player")
                    //Debug.Log("ActionScheduler: Cancelling (null)");
                }

                current = action;
            }    
        }

        public void cancelCurrentAction()
        {
            startAction(null);
        }
    }
}