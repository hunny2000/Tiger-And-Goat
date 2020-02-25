using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Animator TigerRot;
    public Animator GoatRot;

    private void Awake()
    {
        TigerRot.Play("MainRotatinTiger");
        GoatRot.Play("MainRotation");
    }

}
