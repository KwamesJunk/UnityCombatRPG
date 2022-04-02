using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class ControlRemover : MonoBehaviour
    {
        Movement.Mover mover;
        GameObject player;
        GameObject archer; // this is a kludge for the video

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<PlayableDirector>().stopped += EnableControl;
            GetComponent<PlayableDirector>().played += DisableControl;
            player = GameObject.FindGameObjectWithTag("Player");
            mover = player.GetComponent<Mover>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void EnableControl(PlayableDirector director)
        {
            print("ControlRemover::EnableControl()");
            mover.EnableControl();
        }

        public void DisableControl(PlayableDirector dummy)
        {
            print("ControlRemover::DisableControl(" + dummy + ")");
            mover.DisableControl();
            player.GetComponent<ActionScheduler>().cancelCurrentAction();
        }
    }
}