﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ice_fall_scpt : MonoBehaviour
{
    // Start is called before the first frame update


    public float fall_time;

    void Start()
    {
        Physics.gravity = new Vector3(0, -200.0F, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - Character.time_leak - 45) > fall_time) {
            Debug.Log(Character.time_leak);
            Rigidbody rb = this.transform.GetComponent<Rigidbody>();
            rb.useGravity = true;
        }
    }
}
