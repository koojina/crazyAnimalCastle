using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Occupation : MonoBehaviour
{
    GameManager _GM;

    public GameObject Ground;
    public Material Mat_Red;
    public Material Mat_Blue;
    public Renderer[] renderer;

    public string occupationName;
    private string currTeam;
    private float timer;

    public GameObject occupationTimer;
    private bool on_time;
    public Slider slider;

    void Start()
    {
        renderer = Ground.gameObject.GetComponentsInChildren<Renderer>();
        _GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        _GM.Local.Add(occupationName);
    }

    private void Update()
    {
        if (on_time)
        {
            timer += Time.deltaTime;
            slider.value = timer;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Player_Red") && currTeam != "Red")
        {
            occupationTimer.SetActive(true);
            on_time = true;
            if (timer > 10)
            {
                for (int i = 0; i < renderer.Length; i++)
                {
                    if (renderer[i].gameObject.name == "Smoke"
                        || renderer[i].gameObject.name == "Fire"
                        || renderer[i].gameObject.name == "SnowFlakes"
                        || renderer[i].gameObject.name == "Ef_SnowFlakes02"
                        || renderer[i].gameObject.name == "Ef_SnowFlakes03"
                        || renderer[i].gameObject.name == "Ef_SnowFlakes04"
                        || renderer[i].gameObject.name == "Ef_SnowFlakes05"
                        || renderer[i].gameObject.name == "Skull"
                        || renderer[i].gameObject.name == "DoorMiddle")
                    {
                       
                    }
                    else
                    {
                        renderer[i].GetComponent<Renderer>().material = Mat_Red;
                        currTeam = "Red";
                    }
                }
            }
        }
        else if (collision.CompareTag("Player_Blue") && currTeam != "Blue")
        {
            occupationTimer.SetActive(true);
            on_time = true;
            if (timer > 10)
            {
                for (int i = 0; i < renderer.Length; i++)
                {
                    if (renderer[i].gameObject.name == "Smoke"
                        || renderer[i].gameObject.name == "Fire"
                        || renderer[i].gameObject.name == "SnowFlakes"
                        || renderer[i].gameObject.name == "Ef_SnowFlakes02"
                        || renderer[i].gameObject.name == "Ef_SnowFlakes03"
                        || renderer[i].gameObject.name == "Ef_SnowFlakes04"
                        || renderer[i].gameObject.name == "Ef_SnowFlakes05"
                        || renderer[i].gameObject.name == "Skull"
                        || renderer[i].gameObject.name == "DoorMiddle")
                    {
                        
                    }
                    else
                    {
                        renderer[i].GetComponent<Renderer>().material = Mat_Blue;
                        currTeam = "Blue";
                    }
                }
            }
        }
        _GM.setOccupation(occupationName, currTeam);
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Player_Blue") || coll.CompareTag("Player_Red"))
        {
            occupationTimer.SetActive(false);
            on_time = false;
            timer = 0;
        }
    }
}