using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int pots = 7;
    void Awake() {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool hasPotsLeft() {
        return pots > 0;
    }

    internal void retrievePot() {
        pots++;
    }

    internal void throwPot() {
        if (pots > 0)
            pots--;
    }
}
