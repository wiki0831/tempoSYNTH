using System;
using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SoundCube : MonoBehaviour
{
    private TextMesh locationUI;
    private TextMesh nameUI;
    private TextMesh pitchUI;

    private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);
    private Interactable interactable;
    private SoundManager sound;
    private LinearMapping lm;

    public int channel;
    public int minPitch;
    public int maxPitch;
    public int curPitch;
    public int velocity;
    
    private 

    //-------------------------------------------------
    void Awake()
    {
        var textMeshs = GetComponentsInChildren<TextMesh>();
        locationUI = textMeshs[0];
        nameUI = textMeshs[1];
        pitchUI = textMeshs[2];

        interactable = this.GetComponent<Interactable>();
        sound = FindObjectOfType<SoundManager>();
        lm = GetComponent<LinearMapping>();
        curPitch = lmValueMapping(lm.value,minPitch,maxPitch);
    }

    private void Start()
    {
        locationUI.text = "";
        nameUI.text = this.name;
        pitchUI.text = "Pitch: " + curPitch;

        locationUI.transform.rotation = Camera.main.transform.rotation;
        nameUI.transform.rotation = Camera.main.transform.rotation;
        pitchUI.transform.rotation = Camera.main.transform.rotation;
    }

    public void Update()
    {
        int newPitch = lmValueMapping(lm.value, minPitch, maxPitch);
        if (curPitch != newPitch)
        {
            curPitch = newPitch;
            pitchUI.text = "Pitch: " + String.Format("{0:000}", curPitch.ToString());
            sound.PreviewSound(channel, curPitch, 100);
        }
    }

    private void OnHandHoverBegin(Hand hand)
    {
        if (this.transform.parent == null)
        {
            sound.PreviewSound(channel,curPitch, 100);
        }
    }

    private void OnHandHoverEnd(Hand hand)
    {
        sound.PreviewSound(channel, curPitch, 0);
    }


    //-------------------------------------------------
    // Called every Update() while a Hand is hovering over this object
    //-------------------------------------------------
    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

        if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
        {
            // Call this to continue receiving HandHoverUpdate messages,
            // and prevent the hand from hovering over anything else
            hand.HoverLock(interactable);

            // Attach this object to the hand
            hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
        }
        else if (isGrabEnding)
        {
            // Detach this object from the hand
            hand.DetachObject(gameObject);

            // Call this to undo HoverLock
            hand.HoverUnlock(interactable);
        }

        

        locationUI.transform.rotation = Camera.main.transform.rotation;
        nameUI.transform.rotation = Camera.main.transform.rotation;
        pitchUI.transform.rotation = Camera.main.transform.rotation;
    }

    private bool attached;
    //-------------------------------------------------
    // Called when this GameObject becomes attached to the hand
    //-------------------------------------------------
    private void OnAttachedToHand(Hand hand)
    {
        attached = true;
    }


    //-------------------------------------------------
    // Called when this GameObject is detached from the hand
    //-------------------------------------------------
    private void OnDetachedFromHand(Hand hand)
    {
        attached = false;
        if (lastPoint != null && Vector3.Distance(transform.position, lastPoint.position) > 0.01f)
        {
            StartCoroutine(SnapToPos());
        }
        if (lastPoint == null) {
            transform.parent = null;
        }
    }

    private IEnumerator SnapToPos()
    {
            Vector3 initialScale = this.transform.position;
            Vector3 targetScale = lastPoint.transform.position;

            Quaternion initialRot = this.transform.rotation;
            Quaternion targetrot = Quaternion.LookRotation(lastPoint.parent.transform.position - transform.position);

            float startTime = Time.time;
            float overTime = 0.3f;
            float endTime = startTime + overTime;

            while (Time.time < endTime)
            {
                this.transform.position = Vector3.Slerp(initialScale, targetScale, (Time.time - startTime) / overTime);
                this.transform.rotation = Quaternion.Slerp(initialRot, targetrot, (Time.time - startTime) / overTime);

            if (Vector3.Distance(transform.position, lastPoint.position) < 0.01f)
            {
                this.transform.SetParent(lastPoint);
                sound.addSound(int.Parse(lastPoint.name),channel,curPitch,1000);
            }

            yield return null;
        }
    }
    //-------------------------------------------------
    // Called every Update() while this GameObject is attached to the hand
    //-------------------------------------------------
    private void HandAttachedUpdate(Hand hand)
    {
    }

    private Transform lastPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TrackPosition")
        {
            if (attached)
            {
                locationUI.text = toMusical(other.name);
                lastPoint = other.transform;
            }
        }
    }

    private string toMusical(string par)
    {
        int toCal = int.Parse(par) + 1;
        int a = (toCal - 1) / 4 + 1;
        int b = toCal % 4;
        if (b == 0)
        {
            b = 4;
        }
        string res = a + ": " + b;
        return res;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TrackPosition")
        {
            if (attached)
            {
                //sound.removeSound(this.name, int.Parse(other.name));
                locationUI.text = null;
                if (lastPoint != null)
                {
                    sound.removeSound(this.name, int.Parse(lastPoint.name));
                }
                lastPoint = null;
                
            }
        }
    }


    //-------------------------------------------------
    // Called when this attached GameObject becomes the primary attached object
    //-------------------------------------------------
    private void OnHandFocusAcquired(Hand hand)
    {
    }


    //-------------------------------------------------
    // Called when another attached GameObject becomes the primary attached object
    //-------------------------------------------------
    private void OnHandFocusLost(Hand hand)
    {
    }

    int lmValueMapping(float x, int minVal, int maxVal)
    {
        int y = (int)(x * (maxVal - minVal) + minVal);
        return y;
    }
}

