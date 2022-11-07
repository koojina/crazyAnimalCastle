using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title_2 : MonoBehaviour
{
    public Text List_A;
    public Text List_T;

    public GameObject Animal;
    public GameObject Team;
    public GameObject BBI;
    public GameObject Nang;
    public GameObject Peng;
    public GameObject Yang;
    public GameObject Red;
    public GameObject Blue;

    public GameManager _GM;

    int list_Animal = 0;
    int list_Team = 0;

    void Start()
    {
        List_A.text = "0";
        List_T.text = "0";
        _GM = GetComponent<GameManager>();
    }
    void Update()
    {
        if (Animal)
        {
            if (list_Animal == 0)// »ß
            {
                BBI.SetActive(true);
                Nang.SetActive(false);
                Peng.SetActive(false);
                Yang.SetActive(false);
            }
            else if (list_Animal == 1) //³É
            {
                BBI.SetActive(false);
                Nang.SetActive(true);
                Peng.SetActive(false);
                Yang.SetActive(false);
            }
            else if (list_Animal == 2) //Æë
            {
                BBI.SetActive(false);
                Nang.SetActive(false);
                Peng.SetActive(true);
                Yang.SetActive(false);
            }
            else //¾ç
            {
                BBI.SetActive(false);
                Nang.SetActive(false);
                Peng.SetActive(false);
                Yang.SetActive(true);
            }
            _GM.player_Animal = list_Animal;
        }

        if (Team)
        {
            if (list_Team == 0) //»¡
            {
                Red.SetActive(true);
                Blue.SetActive(false);
            }
            else if (list_Team == 1) //ÆÄ
            {
                Red.SetActive(false);
                Blue.SetActive(true);
            }
            _GM.player_Team = list_Team;
        }
    }

    public void ListA_Plus()
    {
        if (list_Animal < 3)
            list_Animal += 1;
        List_A.text = list_Animal.ToString();
    }
    public void ListA_Minus()
    {
        if (list_Animal > 0)
            list_Animal -= 1;
        List_A.text = list_Animal.ToString();
    }

    public void ListT_Plus()
    {
        if (list_Team < 1)
            list_Team += 1;
        List_T.text = list_Team.ToString();
    }

    public void ListT_Minus()
    {
        if (list_Team > 0)
            list_Team -= 1;
        List_T.text = list_Team.ToString();
    }
}