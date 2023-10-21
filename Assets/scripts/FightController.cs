using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FightController : MonoBehaviour
{
    public GameObject Mario;
    public GameObject Topper;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TopperHit()
    {
        BroadcastMessage("topperOnHit");
    }

    void TopperDown()
    {
        BroadcastMessage("MarioResetHits");
    }

    void TopperHide()
    {

    }


}
