using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public OSC osc;
    public bool activityState = false;
    public int BPM = 140;
    Dictionary<string, GameObject> notesParse = new Dictionary<string, GameObject>();
    public List<string> fileName;
    public CubeRespawn[] soundResCues;

    // Start is called before the first frame update
    private void Start()
    {
        osc = this.GetComponent<OSC>();
        osc.SetAddressHandler("/metro", ColorOnPlay);
        osc.SetAddressHandler("/file", FileOnLoad);
        soundResCues = GameObject.FindGameObjectWithTag("SoundSpawnController").GetComponentsInChildren<CubeRespawn>();
        InitParse();
        UpdateState(true);
        UpdateBPM(BPM);
    }

    private void FileOnLoad(OscMessage message) {
        string a = message.ToString();
        a = a.Remove(a.IndexOf("/file Channel "), "/file Channel ".Length);
        List<string> numericList = a.Split(' ').ToList();

        string name = Regex.Replace(a, @"[\d-]", string.Empty);
        
        Debug.Log(name);
        
        int chanel = int.Parse(numericList[0]);
        int min = int.Parse(numericList[numericList.Count-2]);
        int max = int.Parse(numericList[numericList.Count-1]);
        Debug.Log(chanel+" "+min+" "+max);

        if (chanel <= 8)
        {
            soundResCues[chanel-1].name = name;
            soundResCues[chanel-1].valueDisp.text = name;
            soundResCues[chanel - 1].channel = chanel;
            soundResCues[chanel - 1].minPitch = min;
            soundResCues[chanel - 1].maxPitch = max;
        }

    }

    private void ColorOnPlay(OscMessage message)
    {
        GameObject[] higlights = GameObject.FindGameObjectsWithTag("TrackPosition");
        for (int i = 0; i < higlights.Length; i++) {
            if (higlights[i].name == message.GetInt(0).ToString())
            {
                higlights[i].GetComponent<MeshRenderer>().enabled = true;
            }
            else {
                higlights[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    public void InitParse()
    {
        resetMax();
        notesParse = new Dictionary<string, GameObject>();

        GameObject[] sounds = GameObject.FindGameObjectsWithTag("SoundCube");
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].transform.parent != null)
            {
                if (sounds[i].transform.parent.CompareTag("TrackPosition"))
                {
                    string soundKey = sounds[i].transform.name + "," + sounds[i].transform.parent.name;
                    GameObject soundObject = sounds[i];

                    if (!notesParse.ContainsKey(soundKey))
                    {
                        notesParse.Add(soundKey, soundObject);
                    }
                    Debug.Log(soundKey);
                }
            }
        }


        foreach (KeyValuePair<string, GameObject> entry in notesParse)
        {
            OscMessage message;
            message = new OscMessage();
            message.address = "AddSound";
            string[] soundKey = entry.Key.ToString().Split(',');
            message.values.Add(soundKey[0]);
            message.values.Add(int.Parse(soundKey[1]));
            osc.Send(message);
        }
    }

    public void updateVolume(string name, int state)
    {

        OscMessage message;
        message = new OscMessage();
        message.address = "UpdateVolume";
        message.values.Add(name);
        message.values.Add(state);
        osc.Send(message);
    }

    private void resetMax()
    {
        OscMessage message;
        message = new OscMessage();
        message.address = "resetMatrix";
        osc.Send(message);
    }

    public void addSound(int loc, int channel, int pitch, int dur)
    {
        OscMessage message;
        message = new OscMessage();
        message.address = "AddSound";
        
        message.values.Add(loc);
        message.values.Add(channel);
        message.values.Add(pitch);
        message.values.Add(dur);

        osc.Send(message);
    }

    public void removeSound(string name, int loc)
    {
        OscMessage message;
        message = new OscMessage();
        message.address = "RemoveSound";
        message.values.Add(name);
        message.values.Add(loc);
        osc.Send(message);
    }

    public void UpdateBPM(int newBPM)
    {
        BPM = newBPM;
        OscMessage message;
        message = new OscMessage();
        message.address = "BPM";
        message.values.Add(newBPM);
        osc.Send(message);
    }

    public void PreviewSound(int channel, int pitch, int duration) {

        OscMessage message;
        message = new OscMessage();
        message.address = "PreviewSound";
        message.values.Add(channel);
        message.values.Add(pitch);
        message.values.Add(duration);
        osc.Send(message);
    }

    public void UpdateState(bool newState)
    {
        activityState = newState;
        OscMessage message;
        message = new OscMessage();
        message.address = "State";
        if (newState)
        {
            message.values.Add(1);
            GetComponentInChildren<Light>().enabled = true;
        }
        else
        {
            message.values.Add(0);
            GetComponentInChildren<Light>().enabled = false;
        }

        osc.Send(message);
    }

    private void OnApplicationQuit()
    {
        UpdateState(false);
    }

}
