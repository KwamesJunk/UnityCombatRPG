using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform followThis;
        private Vector3 cameraOffset;

        // Start is called before the first frame update
        void Start()
        {
            cameraOffset = transform.position - followThis.position;
            
        }

        //float revolveAngle = 0.0f;
        // Update is called once per frame
        void LateUpdate()
        {
            controlCamera();

            
            Camera.main.transform.LookAt(followThis.transform); // point camera at player

            //Camera.main.transform.position = followThis.position + transform.position;
            Camera.main.transform.position = followThis.position + cameraOffset;
        }

        void controlCamera()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                //revolveAngle -= 0.25f;
                cameraOffset = Quaternion.Euler(0, 0.25f, 0) * cameraOffset; // turn camera around player
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                //revolveAngle += 0.25f;
                cameraOffset = Quaternion.Euler(0, -0.25f, 0) * cameraOffset; // turn camera around player
            }
        }
    } // class
} // namespace