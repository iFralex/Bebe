using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nemico : MonoBehaviour
{
    Coroutine dannoCor;
    Vector2 direzPrec;

    void Start()
    {
        direzPrec.x = Random.Range(-1, 1);
        if (direzPrec.x == 0)
            direzPrec.x = 1;
        //StartCoroutine(Muovi());
    }

    private void OnBecameVisible()
    {
        StartCoroutine(Muovi());
    }
    
    IEnumerator Muovi()
    {
        GetComponent<Rigidbody2D>().velocity = direzPrec;
        for (; GetComponent<SpriteRenderer>().isVisible; )
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + transform.lossyScale.x / 3, transform.position.y), Vector2.down, 1);
            if (hit.collider == null)
            {
                direzPrec = Vector2.left;
                GetComponent<Rigidbody2D>().velocity = direzPrec;
                yield return new WaitForSeconds(.5f);
                continue;
            }
            hit = Physics2D.Raycast(new Vector2(transform.position.x - transform.lossyScale.x / 3, transform.position.y), Vector2.down, 1);
            if (hit.collider == null)
            {
                direzPrec = Vector2.right;
                GetComponent<Rigidbody2D>().velocity = direzPrec;
                yield return new WaitForSeconds(.5f);
                continue;
            }
            hit = Physics2D.Raycast(transform.position, Vector2.left, .3f);
            if (hit.collider != null && (hit.collider.gameObject.tag == "mura" || hit.collider.gameObject.tag == "nemico"))
            {
                direzPrec = Vector2.right;
                GetComponent<Rigidbody2D>().velocity = direzPrec;
                yield return new WaitForSeconds(.5f);
                continue;
            }
            hit = Physics2D.Raycast(transform.position, Vector2.right, .3f);
            if (hit.collider != null && (hit.collider.gameObject.tag == "mura" || hit.collider.gameObject.tag == "nemico"))
            {
                direzPrec = Vector2.left;
                GetComponent<Rigidbody2D>().velocity = direzPrec;
                yield return new WaitForSeconds(.5f);
                continue;
            }
            yield return null;
        }
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<movimento>())
        {
            dannoCor = StartCoroutine(Danno(col.gameObject.GetComponent<movimento>()));
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<movimento>())
        {
            if (dannoCor != null)
                StopCoroutine(dannoCor);
            GetComponent<Rigidbody2D>().velocity = direzPrec;
        }
    }

    IEnumerator Danno(movimento player)
    {
        for (; ; )
        {
            float n = 0;
            if (impostazioni.difficoltà == impostazioni.DifficoltàGenerali.normale)
                n = 1;
            else if (impostazioni.difficoltà == impostazioni.DifficoltàGenerali.normale)
                n = .3f;
            player.pm.Danno(n);
            yield return new WaitForSeconds(.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "terreno")
        {
            StopCoroutine(Muovi());
            if (dannoCor != null)
                StopCoroutine(dannoCor);
            StartCoroutine(DisattivaCollider());
        }
        else if (col.tag == "distruggi nemico")
        {
            FindObjectOfType<partitaManager>().AggiungiBacche(1);
            Destroy(gameObject);
        }
    }

    IEnumerator DisattivaCollider()
    {
        GetComponent<Collider2D>().isTrigger = true;
        yield return new WaitForSeconds(.5f);
        GetComponent<Collider2D>().isTrigger = false;
    }
}