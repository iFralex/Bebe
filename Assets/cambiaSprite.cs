using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cambiaSprite : MonoBehaviour
{
    public Sprite spriteBianco;
    public int index;

    void Awake()
    {
        if (impostazioni.semplificaElementi || impostazioni.livelloDiProva)
        {
            if (spriteBianco != null)
                GetComponent<SpriteRenderer>().sprite = spriteBianco;
            GetComponent<SpriteRenderer>().color = impostazioni.coloriElementi[index];
        }
    }
}