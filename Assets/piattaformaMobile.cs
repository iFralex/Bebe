using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class piattaformaMobile : MonoBehaviour
{
    public Vector2 posizioneIniziale, posizioneFinale;
    public float velocità;
    public float tempo;

    void Start()
    {
        if (impostazioni.difficoltà == impostazioni.DifficoltàGenerali.facile)
            velocità = velocità / 2.5f;
        else if (impostazioni.difficoltà == impostazioni.DifficoltàGenerali.ultraFacile)
            velocità = velocità / 5f;
        transform.position = posizioneIniziale;
    }

    void Update()
    {
        //if (tempo < 1)
        float somma = tempo + Time.deltaTime * velocità;
        tempo = Mathf.Min(1, somma);
        tempo = Mathf.Max(tempo, 0);
        //tempo += Time.deltaTime * velocità;
        /*else
            tempo -= Time.deltaTime * velocità;*/
        transform.position = Vector2.Lerp(posizioneIniziale, posizioneFinale, tempo);
        if (tempo >= 1 || tempo <= 0)
            velocità *= -1;
        if (tempo > 1 || tempo < 0)
            print(tempo);
        }
}
