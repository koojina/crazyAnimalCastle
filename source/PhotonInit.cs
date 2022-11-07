using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    public static PhotonInit instance;

    private GameManager _GM;

    public InputField playerInput;
    bool isGameStart = false;
    bool isLoggIn = false;
    string playerName = "";
    string connectionState = "";
    bool isReady = false;

    PhotonView pv;

    Text connectionInfoText;

    [Header("LobbyCanvas")] 
    public GameObject LobbyCanvas;
    public GameObject LobbyPanel;
    public GameObject MakeRoomPanel;
    public GameObject RoomPanel;
    public InputField RoomInput;
    public InputField RoomPwInput;
    public Toggle PwToggle;
    public GameObject PwPanel;
    public GameObject PwErrorLog;
    public GameObject PwConfirmBtn;
    public GameObject PwPanelCloseBtn;
    public InputField PwCheckIF;
    public bool LockState = false;
    public string privateroom;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;
    public Button CreateRoomBtn;
    public int hashtablecount;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple, roomnumber;

    private void OnConnectedToServer()
    {
        Debug.Log("OnConnectedToServer");
        isReady = true;
    }

    private void Awake()
    {
        PhotonNetwork.GameVersion = "MyFps 1.0";
        PhotonNetwork.ConnectUsingSettings();

        if (GameObject.Find("ConnectionInfoText") != null)
            connectionInfoText = GameObject.Find("ConnectionInfoText").GetComponent<Text>();
        {
            connectionState = "마스터 서버에 접속 중...";
        }
      
        if (connectionInfoText)
        {
            connectionInfoText.text = connectionState;
        }
 
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("LogIn", 0);
    }

    public void Connect()
    {
        Debug.Log("dsadsad");
        if (PhotonNetwork.IsConnected && isReady)
        {  
            connectionState = "룸에 접속...";
            if (connectionInfoText)
            {
                connectionInfoText.text = connectionState;
            }

            LobbyPanel.SetActive(false);
            RoomPanel.SetActive(true);

            PhotonNetwork.JoinLobby();
        }
        else
        {
            connectionState = "오프라인: 마스터 서버와 연결되지 않음\n접속 재시도중...";
            if (connectionInfoText)
            {
                connectionInfoText.text = connectionState;
            }
    
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public static PhotonInit Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(PhotonInit)) as PhotonInit;

                if (instance == null)
                    Debug.Log("no singleton obj");
            }
            return instance;
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionState = "No Room";
        if (connectionInfoText)
        {
            connectionInfoText.text = connectionState;
        }
        Debug.Log("No Room");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        connectionState = "Finish make a room";
        if (connectionInfoText)
        {
            connectionInfoText.text = connectionState;
        }
        Debug.Log("Finish make a room");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        connectionState = "Joined Room";
        if (connectionInfoText)
        {
            connectionInfoText.text = connectionState;
        }
        Debug.Log("Joined room");
        isLoggIn = true;

        PlayerPrefs.SetInt("LogIn", 1);

        PhotonNetwork.LoadLevel("MainScene");
    }

    IEnumerator CreatePlayer()
    {
        while (!isGameStart)
        {
            yield return new WaitForSeconds(0.5f);
        }

        GameObject tempPlayer;
        if (_GM.player_Animal == 0)
        {
            tempPlayer = PhotonNetwork.Instantiate("0", new Vector3(190, 1, 190), Quaternion.identity, 0);
            tempPlayer.GetComponent<MoveScript>().SetPlayerName(playerName);
        }
      else if(_GM.player_Animal == 1)
        {
            tempPlayer = PhotonNetwork.Instantiate("1", new Vector3(190, 1, 190), Quaternion.identity, 0);
            tempPlayer.GetComponent<MoveScript>().SetPlayerName(playerName);
        }
        else if (_GM.player_Animal == 2)
        {
            tempPlayer = PhotonNetwork.Instantiate("2", new Vector3(190, 1, 190), Quaternion.identity, 0);
            tempPlayer.GetComponent<MoveScript>().SetPlayerName(playerName);
        }
        else if (_GM.player_Animal == 3)
        {
            tempPlayer = PhotonNetwork.Instantiate("3", new Vector3(190, 1, 190), Quaternion.identity, 0);
            tempPlayer.GetComponent<MoveScript>().SetPlayerName(playerName);
        }
        pv = GetComponent<PhotonView>();
        yield return null;
    }

    public void SetPlayerName()
    {
        Debug.Log(playerInput.text + "를 입력 하셨습니다!");
        if (isGameStart == false && isLoggIn == false)
        {
            playerName = playerInput.text;
            playerInput.text = string.Empty;
            Debug.Log("connect 시도!" + isGameStart + ", " + isLoggIn);
            OnConnectedToServer();
            Connect();
        }
        else if (isGameStart == true && isLoggIn == true)
        {
            playerInput.text = string.Empty;
        }
    }

        void OnGUI()
    {
        GUILayout.Label(connectionState);
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("LogIn") == 1)
        {
            isLoggIn = true;
        }
            
        if (isGameStart == false && SceneManager.GetActiveScene().name == "MainScene" && isLoggIn == true)
        {
            isGameStart = true;
            StartCoroutine(CreatePlayer());
        }
    }

    #region 방 생성 및 접속 관련 메서드
    public void CreateRoomBtnOnClick()
    {
        MakeRoomPanel.SetActive(true);
    }

    public void OKBtnOnClick()
    {
        MakeRoomPanel.SetActive(false);
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 100 });
        LobbyPanel.SetActive(false);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        MakeRoomPanel.SetActive(false);
        LobbyPanel.SetActive(true);
        connectionState = "마스터 서버에 접속 중...";
        if (connectionInfoText)
            connectionInfoText.text = connectionState;
        isGameStart = false;
        isLoggIn = false;
        PlayerPrefs.SetInt("LogIn", 0);
    }
   
    public void CreateNewRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 80;
        roomOptions.CustomRoomProperties = new Hashtable()
        {
            {"password", RoomPwInput.text }
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };

        if (PwToggle.isOn)
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : "*" + RoomInput.text, roomOptions);
        }
        else
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 100 });
        }

        MakeRoomPanel.SetActive(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate:" + roomList.Count);
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i]))
                {
                    myList.Add(roomList[i]);
                }
                else
                {
                    myList[myList.IndexOf(roomList[i])] = roomList[i];
                }
            }
            else if (myList.IndexOf(roomList[i]) != -1)
            {
                myList.RemoveAt(myList.IndexOf(roomList[i]));
            }
        }
        MyListRenewal();
    }
    #endregion

    public void MyListClick(int num)
    {
        if (num == -2)
        {
            --currentPage;
            MyListRenewal();
        }
        else if (num == -1)
        {
            ++currentPage;
            MyListRenewal();
        }
        else if (myList[multiple + num].CustomProperties["password"] != null)
        {
            PwPanel.SetActive(true);
        }
        else
        {
            PhotonNetwork.JoinRoom(myList[multiple + num].Name);
            MyListRenewal();
        }
    }

    public void RoomPw(int number)
    {
        switch (number)
        {
            case 0:
                roomnumber = 0;
                break;
            case 1:
                roomnumber = 1;
                break;
            case 2:
                roomnumber = 2;
                break;
            case 3:
                roomnumber = 3;
                break;
            default:
                break;
        }
    }

    public void EnterRoomWithPW()
    {
        if ((string)myList[multiple + roomnumber].CustomProperties["password"] == PwCheckIF.text)
        {
            PhotonNetwork.JoinRoom(myList[multiple + roomnumber].Name);
            MyListRenewal();
            PwPanel.SetActive(false);
        }
        else
        {
            StartCoroutine("ShowPwWrongMsg");
        }
    }

    IEnumerator ShowPwWrongMsg()
    {
        if (!PwErrorLog.activeSelf)
        {
            PwErrorLog.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            PwErrorLog.SetActive(false);
        }
    }

    void MyListRenewal()
    {
        maxPage = (myList.Count % CellBtn.Length == 0)
            ? myList.Count / CellBtn.Length
            : myList.Count / CellBtn.Length + 1;

        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        multiple = (currentPage - 1) * CellBtn.Length;

        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count)
             ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }
}