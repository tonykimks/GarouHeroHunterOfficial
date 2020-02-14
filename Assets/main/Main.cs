using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject garou;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currPosition = this.garou.transform.position;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.garou.transform.position = new Vector3(currPosition.x - 32f, currPosition.y);
            //Vector3 endPosition = new Vector3(currPosition.x - 32f, currPosition.y);
            //this.garou.transform.position = Vector3.Lerp(currPosition, endPosition, 0.1f * Time.deltaTime);
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.garou.transform.position = new Vector3(currPosition.x + 32f, currPosition.y);
        } else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.garou.transform.position = new Vector3(currPosition.x, currPosition.y + 32f);
        } else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.garou.transform.position = new Vector3(currPosition.x, currPosition.y - 32f);
        }
    }
}
