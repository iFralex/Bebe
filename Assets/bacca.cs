using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bacca : MonoBehaviour
{
    public float[] secondi;
    public int[] quantità;
    int index;

    void Start()
    {
        secondi[1] -= secondi[0];
        GetComponent<SpriteRenderer>().sprite = impostazioni.semplificaElementi ? variabili.grappoloBaccaBianco : variabili.grappoloBacca;
        StartCoroutine(Distruggi());
    }

    IEnumerator Distruggi()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(secondi[index]);
            if (index == 0)
            {
                index++;
                GetComponent<SpriteRenderer>().sprite = impostazioni.semplificaElementi ? variabili.baccheSingoleBianco[Random.Range(0, variabili.baccheSingole.Length)] : variabili.baccheSingole[Random.Range(0, variabili.baccheSingole.Length)];
            }
            else
                Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<movimento>())
        {
            col.gameObject.GetComponent<movimento>().pm.AggiungiBacche(quantità[index]);
            Destroy(gameObject);
        }
    }
}
