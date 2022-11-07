using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public GameObject rightDoor;
    public GameObject leftDoor;
    private bool m_rotate_open;
    private bool m_rotate_close;
  
    // Start is called before the first frame update
    void Start()
    {
        m_rotate_open = false;
        m_rotate_close = false;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (m_rotate_open && rightDoor.transform.localRotation.eulerAngles.y > -120 && leftDoor.transform.localRotation.eulerAngles.y < 120)
        {
            rightDoor.transform.Rotate(new Vector3(0, -20, 0) * Time.deltaTime);
            leftDoor.transform.Rotate(new Vector3(0, 20, 0) * Time.deltaTime);
        }

        if (m_rotate_close)
        {
            rightDoor.transform.Rotate(new Vector3(0, 20, 0) * Time.deltaTime);
            leftDoor.transform.Rotate(new Vector3(0, -20, 0) * Time.deltaTime);
            if (rightDoor.transform.localRotation.eulerAngles.y > 359)
            {
                m_rotate_close = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        m_rotate_open = true;
        m_rotate_close = false;
    }

    private void OnTriggerExit(Collider other)
    {
        m_rotate_open = false;
        m_rotate_close = true;
    }
}
