using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fucile : MonoBehaviour
{
    public int frequenza;
    public int segnalePrima;
    public Transform proiettile, obbiettivo;
    public GameObject lineaAvvertimento;
    public AudioClip ricaricaA, sparoA;
    public GameObject fiamma;

    void Start()
    {
        StartCoroutine(Spara());
    }

    private void OnBecameVisible()
    {
        StartCoroutine(Spara());
    }

    IEnumerator Spara()
    {
        for (; GetComponent<SpriteRenderer>().isVisible; )
        {
            yield return new WaitForSeconds(frequenza - segnalePrima);
            fiamma.SetActive(false);
            GetComponent<AudioSource>().volume = impostazioniInGame.volumeEffettiSonori;
            GetComponent<AudioSource>().clip = ricaricaA;
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(.1f);
            lineaAvvertimento.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            lineaAvvertimento.SetActive(false);
            yield return new WaitForSeconds(segnalePrima - .5f);
            GetComponent<AudioSource>().volume = impostazioniInGame.volumeEffettiSonori;
            GetComponent<AudioSource>().clip = sparoA;
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(.1f);
            fiamma.SetActive(true);
            proiettile.gameObject.SetActive(true);
            RaycastHit2D[] hit = new RaycastHit2D[5];
            //print(transform.eulerAngles);
            Physics2D.Raycast(transform.position, proiettile.position - transform.position, new ContactFilter2D().NoFilter(), hit);
            for (int i = 0; i < hit.Length; i++)
                if (hit[i].collider != null)
                    if (hit[i].collider.tag == "Player")
                    hit[i].collider.GetComponent<movimento>().pm.Danno(50);
            for (float i = 0; i <= 1; i += 0.3f)
            {
                proiettile.position = Vector2.Lerp(transform.position, obbiettivo.position, i);
                yield return new WaitForSeconds(.02f);
            }
            proiettile.gameObject.SetActive(false);
        }
    }
}
