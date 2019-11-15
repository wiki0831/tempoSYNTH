using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TouchpadInput : MonoBehaviour
{
    public float joyMove = 0.1f;

    public SteamVR_Action_Vector2 moveAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("platformer", "Move");
    private Vector3 movement;

    private SteamVR_Input_Sources hand;
    private Interactable interactable;
    private LinearMapping lm;
    private int defaultValue = 60;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        lm = GetComponentInChildren<LinearMapping>();
        
    }

    private void Update()
    {
        if (interactable.attachedToHand)
        {
            hand = interactable.attachedToHand.handType;
            Vector2 m = moveAction[hand].axis;

            Debug.Log(m.ToString());
            movement = new Vector3(m.x, 0, m.y);

            Debug.Log(m.ToString());
            lm.value = (m.x+1)/2;
            //jump = jumpAction[hand].stateDown;
        }
        else
        {
            movement = Vector2.zero;
        }


        float rot = transform.eulerAngles.y;

        movement = Quaternion.AngleAxis(rot, Vector3.up) * movement;
    }
}

