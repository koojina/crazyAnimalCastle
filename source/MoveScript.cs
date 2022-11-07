using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MoveScript : MonoBehaviour, IPunObservable
{
    public GameManager _GM;
    private PhotonView pv = null;

    private Animator Anim;

    private float _Horizontal = 0.0f;
    private float _Vertical = 0.0f;
    public Vector3 BubbleVec;
    public Transform BubblePos;
    public float Power = 0.0f;
    public Transform followTr;

    public GameObject FollowCamera;

    public float time;

    GameObject NearBubble;

    public int damage;
    public float DoteDamege;

    public int MaxHealth;
    public int CurHealth;
    public int OtherCurHealth;
    public float respawnTime = 10.0f;

    public bool _isDie;
    bool _Run;
    bool _Jump;
    bool _Fire;
    bool _Poison;
    bool _Ice;
    bool _Water;
    bool isJump;

    public float RotSpeed = 1000.0f;

    public Camera followCamera;
    CharacterController _controller;

    public float Speed = 5.0f;
    public float DashSpeed = 15.0f;
    public float finalSpeed;
    public bool run;
    public bool toggleCameraRotation;
    public float smoothness = 10f;
    public float gravity = -20;
    public float jumpPower = 5f;
    public float yVelocity = 0;
    private Vector3 Junpdir;
    private Vector3 playerRotate;

    private Vector3 currPos;
    private Quaternion currRot;

    public int isTeam;
    public int otherTeam;
    public string myTeam;
    public string otherTeam_string;
    public int isAnimal;
    public string myProperty;
    public string otherProperty;

    public TextMesh playerName;
    public string name = "";
    public Slider slider;
    public Image backGround;
    public Image fill;
    public Sprite[] teamSprites;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;

        _GM = GameObject.Find("GameManager").GetComponent<GameManager>();
      
        isTeam = _GM.player_Team; // 0 = Red , 1 = Blue
        isAnimal = _GM.player_Animal;

        followTr = transform.Find("FllowCam").transform;
        Anim = GetComponentInChildren<Animator>();
        followCamera = Camera.main;
        FollowCamera = GameObject.Find("Camera");
        _controller = GetComponent<CharacterController>();
        if (pv.IsMine)
        {
            FollowCamera.GetComponent<FllowCamara>().objectTofollow = followTr;
        }
        NearBubble = GameObject.Find("None_Mesh");
    
        teamSprites = Resources.LoadAll<Sprite>("Sprite");

        if (pv.IsMine)
        {
            if (isTeam == 0)
            {
                gameObject.tag = "Player_Red";
                backGround.sprite = teamSprites[3];
                fill.sprite = teamSprites[2];
            }
            if (isTeam == 1)
            {
                gameObject.tag = "Player_Blue";
                backGround.sprite = teamSprites[1];
                fill.sprite = teamSprites[0];
            }
            myTeam = gameObject.tag;
        }
        else if (!pv.IsMine)
        {
            if (otherTeam == 0)
            {
                gameObject.tag = "Player_Red";
                backGround.sprite = teamSprites[3];
                fill.sprite = teamSprites[2];
            }
            if (otherTeam == 1)
            {
                gameObject.tag = "Player_Blue";
                backGround.sprite = teamSprites[1];
                fill.sprite = teamSprites[0];
            }
            otherTeam_string = gameObject.tag;
        }

        damage = 0;
        MaxHealth = 100;
        CurHealth = 100;

        _isDie = false;
        _Fire = false;
        _Poison = false;
        _Ice = false;
        _Water = false;
    }

    void getInput()
    {
        _Horizontal = Input.GetAxisRaw("Horizontal");
        _Vertical = Input.GetAxisRaw("Vertical");
        _Run = Input.GetButton("Run");
        _Jump = Input.GetButtonDown("Jump");
    }

    void Jump()
    {
        finalSpeed = (run) ? Speed : DashSpeed;
        Junpdir = transform.TransformDirection(Junpdir);
        if (_Jump && !isJump)
        {
            isJump = true;
            yVelocity = jumpPower;
            pv.RPC("JumpAniStart", RpcTarget.All);
        }

        Junpdir.y = yVelocity;
        yVelocity += gravity * Time.deltaTime;

        _controller.Move(Junpdir * finalSpeed * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == "Floor")
        {
            isJump = false;
            pv.RPC("JumpAniEnd", RpcTarget.All);
        }
    }

    void OnDoteDamege()
    {
        if (_Fire)
        {
            damage = 5;
            if (CurHealth > 0)
            {
                CurHealth -= damage;
                Debug.Log(CurHealth);
            }
        }

        if (_Poison)
        {
            damage = 3;
            if (CurHealth > 0)
            {
                CurHealth -= damage;
                Debug.Log(CurHealth);
            }
        }
    }

    void OffDoteDamege()
    {
        if (_Fire)
        {
            _Fire = false;
            CancelInvoke("OnDoteDamege");
        }

        if (_Poison)
        {
            _Poison = false;
            CancelInvoke("OnDoteDamege");
        }
    }

    void Stun()
    {
        Speed = 0;
        DashSpeed = 0;
        pv.RPC("WalkEnd", RpcTarget.All);
    }

    void OffStun()
    {
        _Ice = false;
        Speed = 5;
        DashSpeed = 10;
        _isDie = false;
        CancelInvoke("Stun");
    }

    void Slow()
    {
        Speed /= 2f;
        DashSpeed /= 2f;
    }

    void OffSlow()
    {
        Speed = 5;
        DashSpeed = 10;
        _Water = false;
        CancelInvoke("Slow");
    }

    void OnTriggerEnter(Collider other)
    {
        if (myTeam == "Player_Red")
        {
            if (other.gameObject.CompareTag("Player_BlueFBubble"))
            {
                if (!_Fire)
                {
                    _Fire = true;
                    damage = 10;
                    CurHealth -= damage;

                    InvokeRepeating("OnDoteDamege", 1f, 1f);
                    Invoke("OffDoteDamege", 4f);
                }
            }

            if (other.gameObject.CompareTag("Player_BluePBubble"))
            {
                if (!_Poison)
                {
                    _Poison = true;
                    damage = 10;
                    CurHealth -= damage;

                    InvokeRepeating("OnDoteDamege", 1f, 1f);
                    Invoke("OffDoteDamege", 6f);
                }
            }

            if (other.gameObject.CompareTag("Player_BlueIBubble"))
            {
                if (!_Ice)
                {
                    _Ice = true;
                    damage = 10;
                    CurHealth -= damage;

                    Invoke("Stun", 0.1f);
                    Invoke("OffStun", 2f);
                }
            }

            if (other.gameObject.CompareTag("Player_BlueWBubble"))
            {
                if (!_Water)
                {
                    _Water = true;
                    damage = 10;
                    CurHealth -= damage;

                    Invoke("Slow", 0.1f);
                    Invoke("OffSlow", 3f);
                }
            }
        }
        else if (myTeam == "Player_Blue")
        {
            if (other.gameObject.CompareTag("Player_RedFBubble"))
            {
                if (!_Fire)
                {
                    _Fire = true;
                    damage = 10;
                    CurHealth -= damage;

                    InvokeRepeating("OnDoteDamege", 1f, 1f);
                    Invoke("OffDoteDamege", 4f);
                }
            }

            if (other.gameObject.CompareTag("Player_RedPBubble"))
            {
                if (!_Poison)
                {
                    _Poison = true;
                    damage = 10;
                    CurHealth -= damage;

                    InvokeRepeating("OnDoteDamege", 1f, 1f);
                    Invoke("OffDoteDamege", 6f);
                }
            }

            if (other.gameObject.CompareTag("Player_RedIBubble"))
            {
                if (!_Ice)
                {
                    _Ice = true;
                    damage = 10;
                    CurHealth -= damage;

                    Invoke("Stun", 0.1f);
                    Invoke("OffStun", 2f);
                }
            }

            if (other.gameObject.CompareTag("Player_RedWBubble"))
            {
                if (!_Water)
                {
                    _Water = true;
                    damage = 10;
                    CurHealth -= damage;

                    Invoke("Slow", 0.1f);
                    Invoke("OffSlow", 3f);
                }
            }
        }
    }

    IEnumerator RespawnPlayer(float waitTime)
    {
        _isDie = true;
        StartCoroutine(PlayerVisible(false, 0.0f));
        yield return new WaitForSeconds(3);
        transform.position = new Vector3(190, 2, 190);
        CurHealth = MaxHealth;
        Invoke("Stun", 0.1f);
        Invoke("OffStun", 3f);
        StartCoroutine(PlayerVisible(true, 5.0f));
    }

    IEnumerator PlayerVisible(bool visibled, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GetComponentInChildren<MeshRenderer>().enabled = visibled;
    }

    IEnumerator CreateBubble()
    {
        GameObject intantBubble = Instantiate(NearBubble, BubblePos.position, BubblePos.rotation);
        Rigidbody Bubblerigid = intantBubble.GetComponent<Rigidbody>();
        Bubblerigid.velocity = BubblePos.forward * Power;
        Power = 0;
        yield return null;
    }

    void Attack()
    {
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(CreateBubble());
            pv.RPC("AttackRPC", RpcTarget.Others);
        }
    }

    [PunRPC]
    void AttackRPC()
    {
        StartCoroutine(CreateBubble());
    }

    void BubblePower()
    {
        if(Input.GetMouseButton(0))
        {
            GetPower();
            pv.RPC("GetPower", RpcTarget.Others);
        }
    }

    void Update()
    {
        if (pv.IsMine)
        {
            getInput();
            if(!_isDie)
            {
                InputMovement();
                Jump();
                BubblePower();
                Attack();
            }

            if (CurHealth <= 0)
            {
                StartCoroutine(RespawnPlayer(respawnTime));
            }

            if (Input.GetKey(KeyCode.LeftAlt))
            {
                toggleCameraRotation = true;
            }
            else
            {
                toggleCameraRotation = false;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                run = true;
            }
            else
            {
                run = false;
            }

            if (_GM.Change_Blue)
            {
                BlueProperty();
                pv.RPC("BlueProperty", RpcTarget.Others);
                _GM.Change_Blue = false;
            }
           else if (_GM.Change_Red)
            {
                RedProperty();
                pv.RPC("RedProperty", RpcTarget.Others);
                _GM.Change_Red = false;
            }

            slider.value = CurHealth;
        }
        else if (!pv.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, currPos, Time.deltaTime * smoothness);
            if (_GM.Change_Blue)
            {
                BlueProperty();
                pv.RPC("BlueProperty", RpcTarget.Others);
                _GM.Change_Blue = false;
            }
            else if (_GM.Change_Red)
            {
                RedProperty();
                pv.RPC("RedProperty", RpcTarget.Others);
                _GM.Change_Red = false;
            }

            slider.value = OtherCurHealth;
        }
    }

    void LateUpdate()
    {
        if(pv.IsMine)
        {
            if (toggleCameraRotation != true)
            {
                playerRotate = Vector3.Scale(followCamera.transform.forward, new Vector3(1, 0, 1));
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
            }
        }
        else if(!pv.IsMine)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, currRot, Time.deltaTime * smoothness);
        }
    }
    void InputMovement()
    {
        finalSpeed = (run) ? DashSpeed : Speed;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 moveDirection = forward * _Vertical + right * _Horizontal;

        pv.RPC("InputAni", RpcTarget.All, moveDirection);

        _controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime);
    }

    public void SetPlayerName(string name)
    {
        this.name = name;
        GetComponent<MoveScript>().playerName.text = this.name;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(name);
            stream.SendNext(isTeam);
            stream.SendNext(CurHealth);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            SetPlayerName((string)stream.ReceiveNext());
            otherTeam = (int)stream.ReceiveNext();
            OtherCurHealth = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void InputAni(Vector3 v)
    {
        Anim.SetBool("isWalk", v != Vector3.zero);
        Anim.SetBool("isIdle", _Run);
    }

    [PunRPC]
    void JumpAniStart()
    {
        Anim.SetBool("isJump", true);
        Anim.SetTrigger("doJump");
    }

    [PunRPC]
    void JumpAniEnd()
    {
        Anim.SetBool("isJump", false);
    }

    [PunRPC]
    void WalkEnd()
    {
        Anim.SetBool("isWalk", false);
    }

    [PunRPC]
    void GetPower()
    {
        if (Power < 15f)
        {
            Power += 0.1f;
        }
    }

    [PunRPC]
    void BlueProperty()
    {
        if (pv.IsMine)
        {
            if (myTeam == "Player_Blue")
            {
                myProperty = _GM.Blue_property;
                if (myProperty == "Veld")
                {
                    NearBubble = GameObject.Find("Blue_Water_Mesh");
                }
                else if (myProperty == "Desert")
                {
                    NearBubble = GameObject.Find("Blue_Fire_Mesh");
                }
               else if (myProperty == "Dungeon")
                {
                    NearBubble = GameObject.Find("Blue_Poison_Mesh");
                }
                else if (myProperty == "Winter")
                {
                    NearBubble = GameObject.Find("Blue_Ice_Mesh");
                }
            }
        }
        else if (!pv.IsMine)
        {
            if (otherTeam_string == "Player_Blue")
            {
                otherProperty = _GM.Blue_property;
                if (otherProperty == "Veld")
                {
                    NearBubble = GameObject.Find("Blue_Water_Mesh");
                }
                else if (otherProperty == "Desert")
                {
                    NearBubble = GameObject.Find("Blue_Fire_Mesh");
                }
                else if (otherProperty == "Dungeon")
                {
                    NearBubble = GameObject.Find("Blue_Poison_Mesh");
                }
                else if (otherProperty == "Winter")
                {
                    NearBubble = GameObject.Find("Blue_Ice_Mesh");
                }
            }
        }
    }

    [PunRPC]
    void RedProperty()
    {
        if (pv.IsMine)
        {
            if (myTeam == "Player_Red")
            {
                myProperty = _GM.Red_property;
                if (myProperty == "Veld")
                {
                    NearBubble = GameObject.Find("Red_Water_Mesh");
                }
                else if (myProperty == "Desert")
                {
                    NearBubble = GameObject.Find("Red_Fire_Mesh");
                }
                else if (myProperty == "Dungeon")
                {
                    NearBubble = GameObject.Find("Red_Poison_Mesh");
                }
                else if (myProperty == "Winter")
                {
                    NearBubble = GameObject.Find("Red_Ice_Mesh");
                }
            }
        }
        else if (!pv.IsMine)
        {
            if (otherTeam_string == "Player_Red")
            {
                otherProperty = _GM.Red_property;
                if (otherProperty == "Veld")
                {
                    NearBubble = GameObject.Find("Red_Water_Mesh");
                }
                else if (otherProperty == "Desert")
                {
                    NearBubble = GameObject.Find("Red_Fire_Mesh");
                }
                else if (otherProperty == "Dungeon")
                {
                    NearBubble = GameObject.Find("Red_Poison_Mesh");
                }
                else if (otherProperty == "Winter")
                {
                    NearBubble = GameObject.Find("Red_Ice_Mesh");
                }
            }
        }
    }
}
