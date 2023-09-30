using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Player playerData;

    [SerializeField]
    private ActionObject push;

    private void Awake()
    {
        playerData = new Player(push, push, push, push);
    }
}
