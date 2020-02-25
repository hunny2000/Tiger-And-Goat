using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridMovement : MonoBehaviour
{
    public List<GameObject> GridPos;
    public List<GameObject> Tiger;
    public GameObject GoatSpawn;
    public Material Selection;
    public Material TigerMat;
    public GameObject TurnGoat;
    public GameObject TurnTiger;


    public int Kill;
    int FutureKill = 1;
    Vector3 DeadPos = new Vector3(55f, 0f, -11f);

    public bool TigerTurn = false;

    FiillCheckAndDiagonal Fill;
    GameObject LastSpawnPoint;

    bool selected = false;
    GameObject LastObj;
    public GameObject SpawnHit = null;
    public  RaycastHit hit1;
    public RaycastHit HalfHit;



    //UI
    public GameObject TigerWonPanel;
    public Animator TigerWon;
    public float delay = 2f;




    //NETWORKING
    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void Update()
    {
        
        var ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
 
        if (Physics.Raycast(ray1 , out hit1) && TigerTurn == true && PlayerPrefs.GetString("Choice") == "Tiger" && PlayerPrefs.GetString("Mode") == "Online" ||
            Physics.Raycast(ray1, out hit1) && TigerTurn == true && PlayerPrefs.GetString("Mode") == "Offline")  
        { 
            foreach (GameObject tiger in Tiger)
            {
                if (Input.GetMouseButton(0) && hit1.collider.tag == "Tiger" && selected == false )  
                {
                    

                    selected = true;
                    hit1.collider.gameObject.GetComponent<Renderer>().material = Selection;

                    LastObj = hit1.collider.gameObject;
                }
                 

                if (selected == true && Input.GetMouseButton(0) )
                { 
                    if(hit1.collider.tag == "SpawnPoint" && hit1.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Fill == false) 
                    {
                        Vector3 Delta = hit1.collider.gameObject.transform.position - LastObj.transform.position;

                        //Debug.Log(Delta);

                        if (Delta == new Vector3(11f, 0f, 0f)  ||
                            Delta == new Vector3(0f, 0f, 11f)  ||
                            Delta == new Vector3(11f, 0f, 11f) &&  hit1.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Diagonal == true||

                            Delta == new Vector3(-11f, 0f, 0f) ||
                            Delta == new Vector3(0f, 0f, -11f)  ||
                            Delta == new Vector3(-11f, 0f, -11f) && hit1.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Diagonal == true ||

                            Delta == new Vector3(11f, 0f, -11f)  && hit1.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Diagonal == true ||
                            Delta == new Vector3(-11f, 0, 11f) && hit1.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Diagonal == true)
                        {
                            
                           
                            Debug.Log("spawn");
                            SpawnHit = hit1.collider.gameObject;
                            LastObj.transform.position = SpawnHit.transform.position;

                            
                            selected = false;
                            LastObj.GetComponent<Renderer>().material = TigerMat;
                            if (PlayerPrefs.GetString("Mode") == "Offline")
                            {
                                TigerTurn = false;
                                TurnTiger.SetActive(false);
                                TurnGoat.SetActive(true);
                            }
                            if (PlayerPrefs.GetString("Mode") == "Online")
                            {
                                PV.RPC("RPC_Turn", RpcTarget.AllBuffered, false, false, true);
                            }
                        }

                        #region CUT THE GOAT
                        if (Delta == new Vector3(22f, 0f, 0f) ||
                            Delta == new Vector3(0f, 0f, 22f) ||
                            Delta == new Vector3(22f, 0f, 22f) && hit1.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Diagonal == true ||

                            Delta == new Vector3(-22f, 0f, 0f) ||
                            Delta == new Vector3(0f, 0f, -22f) ||
                            Delta == new Vector3(-22f, 0f, -22f) && hit1.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Diagonal == true ||

                            Delta == new Vector3(22f, 0f, -22f) && hit1.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Diagonal == true ||
                            Delta == new Vector3(-22f, 0, 22f) && hit1.collider.gameObject.GetComponent<FiillCheckAndDiagonal>().Diagonal == true)
                        {

                            Debug.Log("CUT");
                            Vector3 HalfDelta = Delta / 2;
                            
                            foreach (GameObject SpawnPoint in GridPos)
                            {
                                if (LastObj.transform.position + HalfDelta == SpawnPoint.transform.position)
                                {
                                    SpawnPoint.GetComponent<FiillCheckAndDiagonal>().Fill = false;
                                }
                            }
                            foreach (GameObject SpawnPoint in GoatSpawn.GetComponent<GoatSpawn>().GoatPos)
                            {
                                if (LastObj.transform.position + HalfDelta == SpawnPoint.transform.position)
                                {
                                    Debug.Log(SpawnPoint);
                                    
                                    Kill++;
                                    if (Kill == FutureKill)
                                    {
                                        DeadPos = DeadPos + new Vector3(0f, 0f, 11f);
                                        SpawnPoint.transform.position = DeadPos;
                                        FutureKill++;
                                        
                                    }
                                    if (Kill == 5)
                                    {
                                        TigerWonPanel.SetActive(true);
                                        TigerWon.Play("TigerWon");

                                        Invoke("Restart", delay);

                                    }


                                    SpawnHit = hit1.collider.gameObject;
                                    LastObj.transform.position = SpawnHit.transform.position;


                                    //PV.RPC("RPC_Turn", RpcTarget.AllBuffered, false, false, true);

                                    if (PlayerPrefs.GetString("Mode") == "Offline")
                                    {
                                        TigerTurn = false;
                                        TurnTiger.SetActive(false);
                                        TurnGoat.SetActive(true);
                                    }
                                    if (PlayerPrefs.GetString("Mode") == "Online")
                                    {
                                        PV.RPC("RPC_Turn", RpcTarget.AllBuffered, false, false, true);
                                    }

                                    selected = false;
                                    LastObj.GetComponent<Renderer>().material = TigerMat;
                                    
                                }
                            }
                            
                        }

                    }
                    #endregion


                    if (hit1.collider.name != LastObj.name) 
                    {
                        selected = false;
                        LastObj.GetComponent<Renderer>().material = TigerMat;
                        
                    }
                }
            }         

        }
    }
    [PunRPC]
    void RPC_Turn(bool RPC_turn , bool RPC_TurnTiger,bool RPC_TurnGoat) 
    {
        TigerTurn = RPC_turn;
        
        TurnTiger.SetActive(RPC_TurnTiger);

        TurnGoat.SetActive(RPC_TurnGoat);
    }
    
   void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
