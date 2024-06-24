using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    private float legth, startpos, temp, dist;
    [SerializeField]
    private GameObject cam;
    [SerializeField]
    private float paraValue;

    // Start is called before the first frame update
    void Start()
    {
        startpos=this.transform.position.x;
        legth = this.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        temp=cam.transform.position.x*(1-paraValue);
        dist=cam.transform.position.x*paraValue;
        transform.position = new Vector3(startpos+dist,transform.position.y,transform.position.z);


        if(temp>startpos+legth)
        {
            startpos += legth;
        }
        else
            if (temp < startpos - legth)
            {
                startpos -= legth;
            }
    }
}
