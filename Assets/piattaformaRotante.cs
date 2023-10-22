using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class piattaformaRotante : MonoBehaviour
{
    public float velocitàAngolare;

    void Start()
    {
        if (impostazioni.difficoltà == impostazioni.DifficoltàGenerali.facile)
            velocitàAngolare = velocitàAngolare / 2.5f;
        else if (impostazioni.difficoltà == impostazioni.DifficoltàGenerali.ultraFacile)
            velocitàAngolare = velocitàAngolare / 5f;
        GetComponent<Rigidbody2D>().angularVelocity = velocitàAngolare;
    }

    void Update()
    {
        
    }
}
