using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool _isGameOver = false;

    public List<string> BlueTeam = new List<string>();
    public List<string> RedTeam = new List<string>();
    public List<string> Local = new List<string>();

    public int player_Team;
    public int player_Animal;

    public bool Change_Blue = false;
    public bool Change_Red = false;
    public string Blue_property;
    public string Red_property;

    public static GameManager Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;
                if(instance == null)
                {
                    Debug.Log("not singleton");
                }              
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (BlueTeam.Count == 4  && !_isGameOver)
        {
            _isGameOver = true;
            isBlueWin();
            Invoke("Quit_Game", 5);
        }
        else if(RedTeam.Count == 4 && !_isGameOver)
        {
            _isGameOver = true;
            isRedWin();
            Invoke("Quit_Game", 5);
        }
    }

    public void Quit_Game()
    {
        Application.Quit();
    }

    public void isBlueWin()
    {
        GameObject.Find("GameOverUi").transform.FindChild("BlueCanvas").gameObject.SetActive(true);
    }
    public void isRedWin()
    {
        GameObject.Find("GameOverUi").transform.FindChild("RedCanvas").gameObject.SetActive(true);
    }

    public void setOccupation(string name ,string team)
    {
        if (BlueTeam.Contains(name))
        {
            BlueTeam.Remove(name);
        }
        else if (RedTeam.Contains(name))
        {
            RedTeam.Remove(name);
        }

        if(team == "Red")
        {
            RedTeam.Add(name);
            Red_property = name;
            Change_Red = true;
        }
        else if(team == "Blue")
        {
            BlueTeam.Add(name);
            Blue_property = name;
            Change_Blue = true;
        }
    }
}
