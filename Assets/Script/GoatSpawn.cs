using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GoatSpawn : MonoBehaviour
{
    public List<GameObject> GoatPos;
    public GameObject Goat;
    public GameObject turn;
    public Transform GoatParent;
    public int Totalgoat = 20;
    public Text texts;
    bool GoatSelect = false;
    public GameObject GoatLastObject;
    public GameObject GoatCurrentpos = null;
    public Material GoatSelection;
    public Material GoatUnselection;

    public List<GameObject> GridPos;
    public GameObject SpawnHit = null;
    public RaycastHit hit2;
    GameObject LastSpawnPoint;

    //NETWORKING
    private PhotonView PV;

    public void Start()
    {
        PV = GetComponent<PhotonView>();
        FindObjectOfType<Text>();
        texts.text = "Goat =" + Totalgoat;
    }

    void Update()
    {
        if (turn.GetComponent<GridMovement>().TigerTurn == false && PlayerPrefs.GetString("Choice") == "Goat" && PlayerPrefs.GetString("Mode") == "Online" ||
             turn.GetComponent<GridMovement>().TigerTurn == false && PlayerPrefs.GetString("Mode") == "Offline") 
        {
            var ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit2;
            if (Physics.Raycast(ray2, out hit2))
            {

                if (Input.GetMouseButton(0) && hit2.collider.tag == "SpawnPoint"
                    && turn.GetComponent<GridMovement>().SpawnHit != hit2.collider.gameObject &&
                    Totalgoat != 0 &&
                    hit2.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Fill == false)
                {
                    //Debug.Log("GOAT");
                    GameObject goat = PhotonNetwork.Instantiate("Goat", hit2.transform.position, Quaternion.Euler(-90, 0, 0), 0 );

                    GoatPos.Add(goat);

                    if (PlayerPrefs.GetString("Mode") == "Offline")
                    {
                        Totalgoat = Totalgoat - 1;
                        turn.GetComponent<GridMovement>().TigerTurn = true;
                        turn.GetComponent<GridMovement>().TurnTiger.SetActive(true);
                        turn.GetComponent<GridMovement>().TurnGoat.SetActive(false);
                    }
                    if (PlayerPrefs.GetString("Mode") == "Online" )
                    {
                        PV.RPC("RPC_Turn", RpcTarget.AllBuffered, true, true, false);
                        PV.RPC("RPC_Data", RpcTarget.AllBuffered);
                    }

                    //hit2.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Fill = true;

                }
                else if (Totalgoat == 0)
                {
                    if (Input.GetMouseButton(0) &&
                        GoatSelect == false &&
                        turn.GetComponent<GridMovement>().SpawnHit != hit2.collider.gameObject &&
                        hit2.collider.gameObject.tag == "Goat")
                    {
                        GoatSelect = true;
                        GoatLastObject = hit2.collider.gameObject;
                        GoatLastObject.GetComponent<Renderer>().material = GoatSelection;
                        Debug.Log(GoatSelect);
                    }

                    if (GoatSelect == true && Input.GetMouseButton(0) &&
                        hit2.collider.tag == "SpawnPoint" &&
                        turn.GetComponent<GridMovement>().SpawnHit != hit2.collider.gameObject &&
                        hit2.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Fill == false)
                    {
                        Vector3 Delta = hit2.collider.gameObject.transform.position - GoatLastObject.transform.position;
                        if (Delta == new Vector3(11f, 0f, 0f) ||
                            Delta == new Vector3(0f, 0f, 11f) ||
                            Delta == new Vector3(11f, 0f, 11f) && hit2.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Diagonal == true ||

                            Delta == new Vector3(-11f, 0f, 0f) ||
                            Delta == new Vector3(0f, 0f, -11f) ||
                            Delta == new Vector3(-11f, 0f, -11f) && hit2.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Diagonal == true ||

                            Delta == new Vector3(11f, 0f, -11f) && hit2.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Diagonal == true ||
                            Delta == new Vector3(-11f, 0, 11f) && hit2.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Diagonal == true)
                        {
                            SpawnHit = hit2.collider.gameObject;
                            GoatLastObject.transform.position = hit2.collider.transform.position;

                            GoatLastObject.GetComponent<Renderer>().material = GoatUnselection;
                            GoatSelect = false;

                            Debug.Log("last check" + GoatSelect);

                            //hit2.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Fill = true;


                            if (PlayerPrefs.GetString("Mode") == "Offline")
                            {
                                turn.GetComponent<GridMovement>().TigerTurn = true;
                                turn.GetComponent<GridMovement>().TurnTiger.SetActive(true);
                                turn.GetComponent<GridMovement>().TurnGoat.SetActive(false);
                            }
                            if (PlayerPrefs.GetString("Mode") == "Online" )
                            {
                                PV.RPC("RPC_Turn", RpcTarget.AllBuffered, true, true, false);
                            }

                        }
                       if(hit2.collider.name != GoatLastObject.name || hit2.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Fill == true)
                        {
                            GoatSelect = false;
                            GoatLastObject.GetComponent<Renderer>().material = GoatUnselection;

                        }

                    }

                }
            }
        }
        texts.text = "Goat =" + Totalgoat;
    }
    [PunRPC]
    void RPC_Turn(bool RPC_Tiger_turn, bool RPC_TurnTiger, bool RPC_TurnGoat)
    {
        turn.GetComponent<GridMovement>().TigerTurn = RPC_Tiger_turn;
        turn.GetComponent<GridMovement>().TurnGoat.SetActive(RPC_TurnGoat);
        turn.GetComponent<GridMovement>().TurnTiger.SetActive(RPC_TurnTiger);
    }
    [PunRPC]
    void RPC_Data()
    {
        Totalgoat = Totalgoat - 1;
    }
}