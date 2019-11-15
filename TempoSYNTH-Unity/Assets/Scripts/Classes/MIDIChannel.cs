using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIDIChannel : MonoBehaviour
{
    private int channel
    {
        set; get;
    }

    private string name
    {
        set; get;
    }

    private int minPitch
    {
        set; get;
    }

    private int maxPitch
    {
        set; get;
    }

    public MIDIChannel(int channel, string name, int minPitch, int maxPitch)
    {
        this.channel = channel;
        this.name = name;
        this.minPitch = minPitch;
        this.maxPitch = maxPitch;
    }

}
