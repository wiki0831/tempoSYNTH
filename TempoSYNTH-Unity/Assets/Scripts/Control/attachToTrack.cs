using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class attachToTrack : MonoBehaviour
{
    private Transform lastPoint;
    public GameObject statusDisp;
    private string status;
    private SoundManager sound;
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        sound = FindObjectOfType<SoundManager>();
        status = this.name + "\n";
        statusDisp.GetComponent<Text>().text = status;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastPoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, lastPoint.position, speed * Time.deltaTime);
            if (Mathf.Abs(this.transform.position.x - lastPoint.position.x) < 0.3)
            {
                this.transform.SetParent(lastPoint);
                //sound.addSound(this.name, int.Parse(lastPoint.name));
                status = this.name + "\n" + this.transform.parent.name;
                statusDisp.GetComponent<Text>().text = status;
                lastPoint = null;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TrackPosition") {
            Debug.Log(other.name);
            lastPoint = other.transform;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TrackPosition")
        {
            sound.removeSound(this.name, int.Parse(other.name));
            this.transform.SetParent(null);
            status = this.name + "\n" + this.transform.parent.name;
            statusDisp.GetComponent<Text>().text = status;
            Debug.Log("out");
        }

    }


}
