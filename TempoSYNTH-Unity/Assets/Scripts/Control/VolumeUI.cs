using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VolumeUI : MonoBehaviour
{


    private LinearMapping lm;
    private TextMesh valueDisp;
    public int minVal = 0;
    public int maxVal = 100;
    private int curVal;
    private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);
    

    void Start()
    {
        valueDisp = GetComponent<TextMesh>();
        lm = GetComponent<LinearMapping>();
        curVal = lmValueMapping(lm.value,minVal,maxVal);
        valueDisp.text = "Pitch: " + String.Format("{0:000}", curVal.ToString());
    }
    private void HandHoverUpdate(Hand hand)
    {
        valueDisp.transform.rotation = Camera.main.transform.rotation;
    }


    // Update is called once per frame
    void Update()
    {
        bool curstate = SteamVR_Input.GetState("GrabPinch", SteamVR_Input_Sources.Any);

        if (curstate)
        {
            int newBPM = lmValueMapping(lm.value,minVal,maxVal);
            if (curVal != newBPM)
            {
                curVal = newBPM;
                valueDisp.text = "Pitch: " + String.Format("{0:000}", curVal.ToString()); 
            }
        }

    }

    int lmValueMapping(float x,int minVal, int MaxVal)
    {
        int y = (int)(Math.Round(x, 3) * (maxVal - minVal) + minVal);
        return y;
    }


}
