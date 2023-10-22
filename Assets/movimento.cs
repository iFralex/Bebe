using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class movimento : MonoBehaviour
{
    public static float velocitàCamera = 1, posizioneInizialeCamera = 0;
    public float forza = 1;
    public partitaManager pm;
    public finePartita finePart;
    float _vita = 10;
    public float vita
    {
        get { return _vita; }
        set
        {
            if (value != _vita)
            {
                _vita = value;
                pm.Danno(value);
            }
        }
    }
    public AudioSource voloSource;
    public List<Animator> ali;
    Coroutine dannoContCor;
    public bool input = true;

    private void Awake() => Application.targetFrameRate = 200;
     
    void Start()
    {
        vita = 10;
        StartCoroutine(InizioVolo(.1f));
    }

    void Update()
    {
        Camera.main.transform.position = new Vector3(0, transform.position.y + 1.5f, -10);
        if (velocitàCamera != 1)
        {
            Vector3 pos = Camera.main.transform.GetChild(1).position;
            Camera.main.transform.GetChild(1).position = new Vector3(pos.x, Camera.main.transform.position.y * velocitàCamera + posizioneInizialeCamera, pos.z);
        }
        if (Input.touchCount > 0 && input)
        {
            if (impostazioniInGame.modalità == impostazioni.Modalità.guidaIlVoloToccando)
                return;
            Vector2 direz = (Vector2)(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) - (impostazioniInGame.miraRispetto == impostazioni.MiraRispetto.centroBebe ? transform.position : Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2)))) * (impostazioniInGame.direzioneMira == impostazioni.Direzioni.oppostaAlDito ? -1 : 1) * impostazioniInGame.sensibilità * 4;
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    GetComponentInChildren<LineRenderer>().positionCount = 10;
                    break;
                case TouchPhase.Moved:
                    List<Vector3> punti = new List<Vector3>();
                    for (int i = 0; i < 10; i++)
                        punti.Add(new Vector2(direz.x * i / 5, (direz.y * i / 5) + (-5.2f * Mathf.Pow((float)i / 5, 2) / 2)));
                    GetComponentInChildren<LineRenderer>().SetPositions(punti.ToArray());
                    float num = Mathf.Max(.5f, direz.magnitude / 10);
                    break;
                case TouchPhase.Ended:
                    GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    GetComponent<Rigidbody2D>().AddForce(direz, ForceMode2D.Impulse);
                    GetComponent<Rigidbody2D>().gravityScale = .5f;
                    GetComponentInChildren<LineRenderer>().positionCount = 0;
                    StartCoroutine(InizioVolo());
                    break;
            }
        }
        else if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse0)) && input)
        {
            if (impostazioniInGame.modalità == impostazioni.Modalità.guidaIlVoloToccando)
                return;
            Vector2 direz = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - (impostazioniInGame.miraRispetto == impostazioni.MiraRispetto.centroBebe ? transform.position : Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2)))) * (impostazioniInGame.direzioneMira == impostazioni.Direzioni.oppostaAlDito ? -1 : 1) * impostazioniInGame.sensibilità * 4;
            if (Input.GetKeyDown(KeyCode.Mouse0))
                GetComponentInChildren<LineRenderer>().positionCount = 10;
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                List<Vector3> punti = new List<Vector3>();
                for (int i = 0; i < 10; i++)
                    punti.Add(new Vector2(direz.x * i / 5, (direz.y * i / 5) + (-5.2f * Mathf.Pow((float)i / 5, 2) / 2)));
                GetComponentInChildren<LineRenderer>().SetPositions(punti.ToArray());
                float num = Mathf.Max(.5f, direz.magnitude / 10);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                GetComponent<Rigidbody2D>().AddForce(direz, ForceMode2D.Impulse);
                GetComponent<Rigidbody2D>().gravityScale = .5f;
                GetComponentInChildren<LineRenderer>().positionCount = 0;
                StartCoroutine(InizioVolo());
            }
        }
    }

    public void FixedUpdate()
    {
        if (Input.touchCount > 0 && input)
        {
            if (impostazioniInGame.modalità == impostazioni.Modalità.miraEVola)
                return;
            if (!voloSource.isPlaying)
                StartCoroutine(InizioVolo());
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                GetComponent<Rigidbody2D>().AddForce((Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) - (impostazioniInGame.miraRispetto == impostazioni.MiraRispetto.centroBebe ? transform.position : Vector3.zero)) * (impostazioniInGame.direzioneMira == impostazioni.Direzioni.oppostaAlDito ? -1 : 1) * impostazioniInGame.sensibilità * 2);
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (impostazioniInGame.modalità == impostazioni.Modalità.miraEVola)
                return;
            if (!voloSource.isPlaying)
                StartCoroutine(InizioVolo());
            GetComponent<Rigidbody2D>().AddForce((Vector2)((Camera.main.ScreenToWorldPoint(Input.mousePosition) - (impostazioniInGame.miraRispetto == impostazioni.MiraRispetto.centroBebe ? transform.position : Vector3.zero)) / forza) * (impostazioniInGame.direzioneMira == impostazioni.Direzioni.oppostaAlDito ? -1 : 1) * impostazioniInGame.sensibilità * 2);
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        GetComponent<Rigidbody2D>().gravityScale = .1f;
        if (col.gameObject.tag == "danno" && !col.collider.usedByEffector)
        {
            int n = 0;
            if (impostazioni.difficoltà == impostazioni.DifficoltàGenerali.normale)
                n = 5;
            else if (impostazioni.difficoltà == impostazioni.DifficoltàGenerali.facile)
                n = 2;
            pm.Danno(n);
            if (dannoContCor == null)
                dannoContCor = StartCoroutine(DannoContinuato());
        }
        else
        {
            float nit = 0;
            if (col.gameObject.tag == "mura")
                return;
            else if (col.gameObject.tag == "rimbalza")
                nit = 0.2f;
            else
                nit = 1;
            MoreMountains.NiceVibrations.MMVibrationManager.TransientHaptic(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.magnitude / 7f), nit);
        }
    }

    public void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "danno" && !col.collider.usedByEffector)
            if (dannoContCor != null)
            {
                StopCoroutine(dannoContCor);
                dannoContCor = null;
            }
    }

    IEnumerator InizioVolo(float secondi = 0)
    {
        yield return new WaitForSeconds(secondi);
        foreach (Animator ala in ali)
            ala.SetBool("sbatti", true);
        voloSource.Play();
        yield return new WaitWhile(() => GetComponent<Rigidbody2D>().velocity != Vector2.zero);
        voloSource.Pause();
        foreach (Animator ala in ali)
            ala.SetBool("sbatti", false);
    }

    IEnumerator DannoContinuato()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(1.5f);
            pm.Danno(3);
        }
    }
}