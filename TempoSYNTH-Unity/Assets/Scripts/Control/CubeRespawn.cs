using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

public class CubeRespawn : MonoBehaviour
{
    public HoverButton hoverButton;

    public GameObject prefab;
    public float size;

    public bool randomColor = false;
    public float[] colorCode = { 0, 1, 0, 1, 0, 1, 0, 1 };

    public string name;
    public TextMesh valueDisp;
    public int channel;
    public int minPitch;
    public int maxPitch;


    private void Start()
    {
        hoverButton.onButtonDown.AddListener(OnButtonDown);
        valueDisp = this.transform.parent.GetComponentInChildren<TextMesh>();
        valueDisp.text = name;
    }

    private void OnButtonDown(Hand hand)
    {
        StartCoroutine(SpawnCube());
    }

    //-------------------------------------------------
    // Called when a Hand starts hovering over this object
    //-------------------------------------------------
    private void OnHandHoverBegin(Hand hand)
    {

    }

    //-------------------------------------------------
    // Called when a Hand stops hovering over this object
    //-------------------------------------------------
    private void OnHandHoverEnd(Hand hand)
    {

    }

    private IEnumerator SpawnCube()
    {
        GameObject cube = GameObject.Instantiate<GameObject>(prefab);
        cube.transform.position = this.transform.position;
        cube.transform.rotation = this.transform.rotation;

        cube.GetComponent<SoundCube>().channel = channel;
        cube.GetComponent<SoundCube>().minPitch = minPitch;
        cube.GetComponent<SoundCube>().maxPitch = maxPitch;


        if (randomColor == true)
        {
            Color newColor = UnityEngine.Random.ColorHSV(colorCode[0], colorCode[1], colorCode[2], colorCode[3], colorCode[4], colorCode[5], colorCode[6], colorCode[7]);
            cube.GetComponent<Renderer>().material.color = newColor;
        }

        cube.name = name;
        Vector3 initialScale = Vector3.one * 0.01f;
        Vector3 targetScale = Vector3.one * size;

        float startTime = Time.time;
        float overTime = 0.5f;
        float endTime = startTime + overTime;

        while (Time.time < endTime)
        {
            cube.transform.localScale = Vector3.Slerp(initialScale, targetScale, (Time.time - startTime) / overTime);
            yield return null;
        }
    }
}
