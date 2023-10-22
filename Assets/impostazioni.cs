using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.UI;

public class impostazioni : MonoBehaviour
{
    public enum Voci { Generali, Comandi, Grafica, Audio, Info_e_Supporto }
    public enum Lingue { linguaDiSistema, inglese, italiano }
    public enum DifficoltàGenerali { normale, facile, ultraFacile }
    public enum Modalità { miraEVola, guidaIlVoloToccando }
    public enum Direzioni { oppostaAlDito, ugualeAlDito }
    public enum MiraRispetto { centroSchermo, centroBebe }
    public enum TipiSfondo { dinamico3D, immagineStatica2D, tintaUnita2D }
    public enum FrequenzaCinguettii { sempre, spesso, aVolte, mai}

    public RectTransform listaPannelli;
    public RectTransform listaVoci;
    public FlexibleColorPicker colorPicker;
    public GameObject finestraConf;
    public Image[] listaColori;
    public Image coloreSfondoIm, coloreMirinoIm;
    public Lofelt.NiceVibrations.HapticReceiver ricevitoreFeedbackAptico;
    public AudioListener audioListener;
    public Font fontDis, fontNormale;
    public List<ToggleController> listaToggle;
    public List<Dropdown> listaDropdown;
    public List<Slider> listaSlider;
    public RectTransform listaColoriMenù, listaColoriElementi;
    public GameObject coloreSfondoTintaUnita, listaColoriSemplifElem;


    public static bool feedbackApticoAttivo = true;
    public static bool fontDislessia = false;
    public static DifficoltàGenerali difficoltà;
    public static int fpsGame = 120;
    public static int fpsMenù = 30;
    public static Lingue lingua;

    public static bool livelloDiProva;
    public static Modalità modalità;
    public static Direzioni direzione;
    public static MiraRispetto miraRispetto;
    public static float sensibilità = 1;
    public static float tempoPremerePowerUp = .5f;

    public static List<Color> coloriMenù = new List<Color>()
    {
        new Color(0, 0.4392157f, 1, 1),
        Color.red,
        new Color(0.7019608f, 0.282353f, 1, 1),
        Color.green
    };
    public static TipiSfondo tipoSfondo;
    public static Color coloreSfondo = Color.black;
    public static Color coloreMirino = new Color(0, 0, .5f, 1);
    public static bool semplificaElementi = false;
    public static List<Color> coloriElementi = new List<Color>()
    {
        new Color(1, .5f, 0, 1),
        Color.blue,
        Color.green,
        Color.red,
        new Color(1, 0, 1, 1)
    };

    public static bool audioAttivo = true;
    public static FrequenzaCinguettii frequenzaCinguettii = FrequenzaCinguettii.aVolte;
    public static bool attivaCinguettiiSeSubisceDanno = true;
    public static float volumeMusica = 1;
    public static float volumeEffettiSonoriGame = 1;
    public static float volumeEffettiSonoriMenù = 1;


    string formattaStringa(string stringa)
    {
        string s = "";
        for (int i = 0; i < stringa.Length; i++)
            if (stringa[i] + "" != "_")
                s += stringa[i] + "";
            else
            {
                s += " ";
                if (stringa[i + 1] + "" == "e")
                {
                    s += "&";
                    i += 1;
                }
            }
        return s;
    }

    int indexDropdown(int[] n, int v)
    {
        for (int i = 0; i < n.Length; i++)
            if (n[i] == v)
                return i;
        return 0;
    }


    void Awake()
    {
        bool cè(string s) => PlayerPrefs.HasKey(s);
        int OttieniInt(string s) => PlayerPrefs.GetInt(s);
        float OttieniFloat(string s) => PlayerPrefs.GetFloat(s);
        string OttieniString(string s) => PlayerPrefs.GetString(s);
        bool OttieniBool(string s) => System.Convert.ToBoolean(OttieniInt(s));
        Color OttieniColore(string s) => new Color(OttieniFloat("(r)" + s), OttieniFloat("(g)" + s), OttieniFloat("(b)" + s));
        void SalvaInt(string s, int v) => PlayerPrefs.SetInt(s, v);
        void SalvaFloat(string s, float v) => PlayerPrefs.SetFloat(s, v);
        void SalvaString(string s, string v) => PlayerPrefs.SetString(s, v);
        void SalvaColore(string s, Color c)
        {
            SalvaFloat("(r)" + s, c.r);
            SalvaFloat("(g)" + s, c.g);
            SalvaFloat("(b)" + s, c.b);
        }

        if (variabili.primoFrame)
        {
            string s = "feedback aptico";
            if (cè(s))
                feedbackApticoAttivo = OttieniBool(s);
            else
                SalvaInt(s, System.Convert.ToInt32(feedbackApticoAttivo));

            s = "difficoltà generale";
            if (cè(s))
                difficoltà = (DifficoltàGenerali)OttieniInt(s);
            else
                SalvaInt(s, (int)difficoltà);

            s = "font dislessia";
            if (cè(s))
                fontDislessia = OttieniBool(s);
            else
                SalvaInt(s, System.Convert.ToInt32(fontDislessia));

            s = "fps in game";
            if (cè(s))
                fpsGame = OttieniInt(s);
            else
                SalvaInt(s, fpsGame);

            s = "fps in menù";
            if (cè(s))
                fpsMenù = OttieniInt(s);
            else
                SalvaInt(s, fpsMenù);

            s = "lingua";
            if (cè(s))
                lingua = (Lingue)OttieniInt(s);
            else
                SalvaInt(s, (int)lingua);


            s = "modalità input";
            if (cè(s))
                modalità = (Modalità)OttieniInt(s);
            else
                SalvaInt(s, (int)modalità);

            s = "direzione input";
            if (cè(s))
                direzione = (Direzioni)OttieniInt(s);
            else
                SalvaInt(s, (int)direzione);

            s = "mira rispetto";
            if (cè(s))
                miraRispetto = (MiraRispetto)OttieniInt(s);
            else
                SalvaInt(s, (int)miraRispetto);

            s = "sensibilità";
            if (cè(s))
                sensibilità = OttieniFloat(s);
            else
                SalvaFloat(s, sensibilità);

            s = "tempo premere per power up";
            if (cè(s))
                tempoPremerePowerUp = OttieniFloat(s);
            else
                SalvaFloat(s, tempoPremerePowerUp);


            for (int i = 0; i < listaColoriMenù.childCount; i++)
            {
                s = "colore menù: " + i.ToString();
                if (cè("(b)" + s))
                    coloriMenù[i] = OttieniColore(s);
                else
                    SalvaColore(s, coloriMenù[i]);
            }

            s = "tipo sfondo";
            if (cè(s))
                tipoSfondo = (TipiSfondo)OttieniInt(s);
            else
                SalvaInt(s, (int)tipoSfondo);

            s = "colore sfondo gameplay";
            if (cè("(b)" + s))
                coloreSfondo = OttieniColore(s);
            else
                SalvaColore(s, coloreSfondo);

            s = "colore mirino";
            if (cè("(b)" + s))
                coloreMirino = OttieniColore(s);
            else
                SalvaColore(s, coloreMirino);

            s = "semplifica elementi";
            if (cè(s))
                semplificaElementi = OttieniBool(s);
            else
                SalvaInt(s, System.Convert.ToInt32(semplificaElementi));

            for (int i = 0; i < listaColoriElementi.childCount; i++)
            {
                s = "colore elemento: " + i.ToString();
                if (cè("(b)" + s))
                    coloriElementi[i] = OttieniColore(s);
                else
                    SalvaColore(s, coloriElementi[i]);
            }


            s = "audio attivo";
            if (cè(s))
                audioAttivo = OttieniBool(s);
            else
                SalvaInt(s, System.Convert.ToInt32(audioAttivo));

            s = "frequenza cinguettii";
            if (cè(s))
                frequenzaCinguettii = (FrequenzaCinguettii)OttieniInt(s);
            else
                SalvaInt(s, (int)frequenzaCinguettii);

            s = "attiva cinguettii se subisce danno";
            if (cè(s))
                attivaCinguettiiSeSubisceDanno = OttieniBool(s);
            else
                SalvaInt(s, System.Convert.ToInt32(attivaCinguettiiSeSubisceDanno));

            s = "volume musica";
            if (cè(s))
                volumeMusica = OttieniFloat(s);
            else
                SalvaFloat(s, volumeMusica);

            s = "volume effetti sonori in game";
            if (cè(s))
                volumeEffettiSonoriGame = OttieniFloat(s);
            else
                SalvaFloat(s, volumeEffettiSonoriGame);

            s = "volume effetti sonori in menù";
            if (cè(s))
                volumeEffettiSonoriMenù = OttieniFloat(s);
            else
                SalvaFloat(s, volumeEffettiSonoriMenù);
        }



        listaToggle[0].isOn = feedbackApticoAttivo;
        listaToggle[1].isOn = fontDislessia;
        listaToggle[2].isOn = semplificaElementi;
        listaToggle[3].isOn = audioAttivo;
        listaToggle[4].isOn = attivaCinguettiiSeSubisceDanno;
        foreach (ToggleController t in listaToggle)
            t.eventi.Invoke(t.isOn);

        listaDropdown[0].value = (int)difficoltà;
        listaDropdown[1].value = indexDropdown(new int[] { 120, 60, 30 }, fpsGame);
        listaDropdown[2].value = indexDropdown(new int[] { 60, 30, 24, 15 }, fpsMenù);
        listaDropdown[3].value = (int)lingua;
        listaDropdown[4].value = (int)modalità;
        listaDropdown[5].value = (int)direzione;
        listaDropdown[6].value = (int)miraRispetto;
        listaDropdown[7].value = (int)tipoSfondo;
        listaDropdown[8].value = (int)frequenzaCinguettii;
        foreach (Dropdown d in listaDropdown)
            d.onValueChanged.Invoke(d.value);

        listaSlider[0].value = sensibilità;
        listaSlider[1].value = tempoPremerePowerUp;
        listaSlider[2].value = volumeMusica;
        listaSlider[3].value = volumeEffettiSonoriGame;
        listaSlider[4].value = volumeEffettiSonoriMenù;
        foreach (Slider d in listaSlider)
            d.onValueChanged.Invoke(d.value);

        for (int i = 0; i < listaColoriMenù.childCount; i++)
            listaColoriMenù.GetChild(i).GetChild(2).GetComponent<Image>().color = coloriMenù[i];
        coloreSfondoIm.color = coloreSfondo;
        coloreMirinoIm.color = coloreMirino;
        for (int i = 0; i < listaColoriElementi.childCount; i++)
            listaColoriElementi.GetChild(i).GetChild(2).GetComponent<Image>().color = coloriElementi[i];
        for (int i = 0; i < listaColori.Length; i++)
        {
            colorPicker.index = i;
            listaColori[i].GetComponent<eventoColore>().evento.Invoke(listaColori[i].GetComponent<Image>().color);
        }
    }

    void Start()
    {
        /*float alt = 0;
        for (int i = 0; i < listaVoci.childCount; i++)
            alt += listaVoci.GetChild(i).GetComponent<RectTransform>().sizeDelta.y + 15;
        listaVoci.sizeDelta = new Vector2(listaPannelli.sizeDelta.x, alt);*/
        for (int i = 0; i < listaVoci.transform.childCount; i++)
            listaVoci.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(listaVoci.rect.width, listaVoci.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y);
        gameObject.SetActive(false);
    }

    public void SelezionaPannello(int n)
    {
        colorPicker.gameObject.SetActive(false);
        finestraConf.gameObject.SetActive(false);
        if (listaPannelli.GetChild(n).gameObject.activeInHierarchy)
        {
            listaPannelli.parent.parent.gameObject.SetActive(false);
            listaVoci.GetChild(n).GetComponent<Image>().color = new Color(0, 0, .5f, 1);
            return;
        }
        MMVibrationManager.Selezione();
        listaPannelli.parent.parent.gameObject.SetActive(true);

        for (int i = 0; i < listaPannelli.childCount; i++)
            if (n != i)
                listaPannelli.GetChild(i).gameObject.SetActive(false);
            else
                listaPannelli.GetChild(n).gameObject.SetActive(true);

        for (int i = 0; i < listaVoci.childCount; i++)
            if (i == n)
                listaVoci.GetChild(n).GetComponent<Image>().color = Color.green;
            else
                listaVoci.GetChild(i).GetComponent<Image>().color = new Color(0, 0, .5f, 1);

        float alt = 0;
        for (int i = 0; i < listaPannelli.GetChild(n).childCount; i++)
            alt += listaPannelli.GetChild(n).GetChild(i).GetComponent<RectTransform>().sizeDelta.y + 15;
        listaPannelli.sizeDelta = new Vector2(listaPannelli.sizeDelta.x, alt);
        listaPannelli.parent.parent.GetChild(0).GetComponent<Text>().text = formattaStringa(((Voci)n).ToString());
        if ((Voci)n == Voci.Grafica)
            for (int i = 0; i < listaColori.Length; i++)
                listaColori[i].transform.parent.GetChild(0).GetComponent<Image>().color = Color.blue;
    }

    public void SelezionaColore(int n)
    {
        finestraConf.gameObject.SetActive(false);
        MMVibrationManager.Selezione();
        if (listaColori[n].transform.parent.GetChild(0).GetComponent<Image>().color == Color.green)
        {
            colorPicker.gameObject.SetActive(false);
            listaColori[n].transform.parent.GetChild(0).GetComponent<Image>().color = Color.blue;
            return;
        }
        colorPicker.coloreIm = listaColori[n];
        colorPicker.bufferedColor = null;
        colorPicker.startingColor = listaColori[n].color;
        colorPicker.evento = null;
        colorPicker.evento = listaColori[n].GetComponent<eventoColore>().evento;
        colorPicker.index = n;
        colorPicker.gameObject.SetActive(true);
        colorPicker.OnEnable();
        for (int i = 0; i < listaColori.Length; i++)
            listaColori[i].transform.parent.GetChild(0).GetComponent<Image>().color = Color.blue;
        listaColori[n].transform.parent.GetChild(0).GetComponent<Image>().color = Color.green;
    }

    public void DisattivaSelezioneColore()
    {
        colorPicker.gameObject.SetActive(false);
        for (int i = 0; i < listaColori.Length; i++)
            listaColori[i].transform.parent.GetChild(0).GetComponent<Image>().color = Color.blue;
    }

    public void AttivaFeedbackAptico(bool b)
    {
        if (!variabili.primoFrame)
        {
            feedbackApticoAttivo = b;
            PlayerPrefs.SetInt("feedback aptico", System.Convert.ToInt32(b));
        }
        MMVibrationManager.SetHapticsActive(b);
        ricevitoreFeedbackAptico.hapticsEnabled = b;
    }

    public void FontDislessia(bool b)
    {
        if (!variabili.primoFrame)
        {
            fontDislessia = b;
            PlayerPrefs.SetInt("font dislessia", System.Convert.ToInt32(b));
        }
        CambiaTuttiTesti(b);
    }

    public static void CambiaTuttiTesti(bool b)
    {
        Font f = b ? variabili.fontDislessia : variabili.fontNormale;
        if (FindObjectOfType<Text>().font == f && !variabili.primoFrame)
            return;
        Text[] testi = Resources.FindObjectsOfTypeAll<Text>();
        foreach (Text t in testi)
            t.font = f;
    }

    public void FPSGame(int n)
    {
        if (!variabili.primoFrame)
        { 
            switch (n)
        {
            case 0:
                fpsGame = 120;
                break;
            case 1:
                fpsGame = 60;
                break;
            case 2:
                fpsGame = 30;
                break;
        }
        PlayerPrefs.SetInt("fps in game", fpsGame);
    }
    }

    public void FPSMenù(int n)
    {
        if (!variabili.primoFrame)
        {
            switch (n)
            {
                case 0:
                    fpsMenù = 60;
                    break;
                case 1:
                    fpsMenù = 30;
                    break;
                case 2:
                    fpsMenù = 24;
                    break;
                case 3:
                    fpsMenù = 15;
                    break;
            }
            PlayerPrefs.SetInt("fps in menù", fpsMenù);
        }
        Application.targetFrameRate = fpsMenù;
    }

    public void ImpostaLingua(int n)
    {
        if (!variabili.primoFrame)
        {
            lingua = (Lingue)n;
            PlayerPrefs.SetInt("lingua", (int)lingua);
        }
    }

    public void ImpostaDifficoltà(int n)
    {
        if (!variabili.primoFrame)
        {
            difficoltà = (DifficoltàGenerali)n;
            PlayerPrefs.SetInt("difficoltà generale", (int)difficoltà);
        }
    }

    float tempo;
    public void RipristinaTuttiIDatiDown()
    {
        tempo = Time.time;
        print(tempo);
    }

    public void RipristinaTuttiIDatiUp()
    {
        print(Time.time);
        if (tempo + 10 < Time.time)
            RipristinaTuttiIDati();
    }

    public void RipristinaTuttiIDati()
    {
        print("aa");
        //PlayerPrefs.DeleteAll();
        //Application.Quit();
    }

    public void ApriLivelloDiProva()
    {
        livelloDiProva = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1);
    }

    public void ImpostaModalità(int n)
    {
        if (!variabili.primoFrame)
        {
            modalità = (Modalità)n;
            PlayerPrefs.SetInt("modalità input", (int)modalità);
        }
    }

    public void ImpostaDirezione(int n)
    {
        if (!variabili.primoFrame)
        {
            direzione = (Direzioni)n;
            PlayerPrefs.SetInt("direzione", (int)direzione);
        }
    }

    public void ImpostaMiraRispetto(int n)
    {
        if (!variabili.primoFrame)
        {
            miraRispetto = (MiraRispetto)n;
            PlayerPrefs.SetInt("mira rispetto", (int)miraRispetto);
        }
    }

    public void ImpostaSensibilità(float n)
    {
        if (!variabili.primoFrame)
        {
            sensibilità = n;
            PlayerPrefs.SetFloat("sensibilità", n);
        }
    }

    public void ImpostaTempoPremerePowerUp(float n)
    {
        if (!variabili.primoFrame)
        {
            tempoPremerePowerUp = n;
            PlayerPrefs.SetFloat("tempo premere per power up", n);
        }
    }

    public void ImpostaColoriMenù(Color c)
    {
        int n = colorPicker.index;
        if (!variabili.primoFrame)
        {
            coloriMenù[n] = c;
            PlayerPrefs.SetFloat("(r)colore menù: " + n, c.r);
            PlayerPrefs.SetFloat("(g)colore menù: " + n, c.g);
            PlayerPrefs.SetFloat("(b)colore menù: " + n, c.b);
        }
        menuManager.coloriPannelli = coloriMenù;
    }

    public void ImpostaTipoSfondo(int n)
    {
        if (!variabili.primoFrame)
        {
            tipoSfondo = (TipiSfondo)n;
            PlayerPrefs.SetInt("tipo sfondo", n);
        }
        if (tipoSfondo == TipiSfondo.tintaUnita2D)
        {
            coloreSfondoTintaUnita.SetActive(true);
            coloreSfondoTintaUnita.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(coloreSfondoTintaUnita.transform.parent.GetComponent<RectTransform>().sizeDelta.x, 603);//coloreSfondoTintaUnita.transform.parent.GetComponent<RectTransform>().sizeDelta.y + coloreSfondoTintaUnita.GetComponent<RectTransform>().sizeDelta.y + 20);
        }
        else
        {
            coloreSfondoTintaUnita.SetActive(false);
            coloreSfondoTintaUnita.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(coloreSfondoTintaUnita.transform.parent.GetComponent<RectTransform>().sizeDelta.x, 510);// coloreSfondoTintaUnita.transform.parent.GetComponent<RectTransform>().sizeDelta.y - coloreSfondoTintaUnita.GetComponent<RectTransform>().sizeDelta.y - 20);
        }
        StartCoroutine(RidimensionaCanva());
    }

    public void ImpostaTipoSfondo(Color c)
    {
        if (!variabili.primoFrame)
        {
            coloreSfondo = c;
            PlayerPrefs.SetFloat("(r)colore sfondo gameplay", c.r);
            PlayerPrefs.SetFloat("(g)colore sfondo gameplay", c.g);
            PlayerPrefs.SetFloat("(b)colore sfondo gameplay", c.b);
        }
    }

    public void ImpostaColoreMirino(Color c)
    {
        if (!variabili.primoFrame)
        {
            coloreMirino = c;
            PlayerPrefs.SetFloat("(r)colore mirino", c.r);
            PlayerPrefs.SetFloat("(g)colore mirino", c.g);
            PlayerPrefs.SetFloat("(b)colore mirino", c.b);
        }
    }

    public void ImpostaSemplificaEleemnti(bool b)
    {
        if (!variabili.primoFrame)
        {
            semplificaElementi = b;
            PlayerPrefs.SetInt("semplifica elementi", System.Convert.ToInt32(b));
        }
        if (semplificaElementi)
        {
            listaColoriSemplifElem.SetActive(true);
            listaColoriSemplifElem.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(listaColoriSemplifElem.transform.parent.GetComponent<RectTransform>().sizeDelta.x, 820);
        }
        else
        {
            listaColoriSemplifElem.SetActive(false);
            listaColoriSemplifElem.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(listaColoriSemplifElem.transform.parent.GetComponent<RectTransform>().sizeDelta.x, 390);//listaColoriSemplifElem.transform.parent.GetComponent<RectTransform>().sizeDelta.y - listaColoriSemplifElem.GetComponent<RectTransform>().sizeDelta.y - 20);
        }
        StartCoroutine(RidimensionaCanva());
    }

    IEnumerator RidimensionaCanva()
    {
        yield return null;
        listaColoriSemplifElem.transform.parent.parent.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(listaColoriSemplifElem.transform.parent.parent.parent.GetComponent<RectTransform>().sizeDelta.x, listaColoriSemplifElem.transform.parent.parent.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void ImpostaColoriElementi(Color c)
    {
        int n = colorPicker.index - 6;
        if (!variabili.primoFrame)
        {
            coloriElementi[n] = c;
            PlayerPrefs.SetFloat("(r)colore elemento: " + n, c.r);
            PlayerPrefs.SetFloat("(g)colore elemento: " + n, c.g);
            PlayerPrefs.SetFloat("(b)colore elemento: " + n, c.b);
        }
    }

    public void ImpostaAudioAttivo(bool b)
    {
        if (!variabili.primoFrame)
        {
            audioAttivo = b;
            PlayerPrefs.SetInt("audio attivo", System.Convert.ToInt32(b));
        }
        AudioListener.volume = b ? 1 : 0;
    }

    public void ImpostaFrequenzaCinguetii(int n)
    {
        if (!variabili.primoFrame)
        {
            frequenzaCinguettii = (FrequenzaCinguettii)n;
            PlayerPrefs.SetInt("frequenza cinguettii", n);
        }
    }

    public void ImpostaCinguettiiSeSubisceDanno(bool b)
    {
        if (!variabili.primoFrame)
        {
            attivaCinguettiiSeSubisceDanno = b;
            PlayerPrefs.SetInt("attiva cinguettii se subisce danno", System.Convert.ToInt32(b));
        }
    }

    public void ImpostaVolumeMusica(float n)
    {
        if (!variabili.primoFrame)
        {
            volumeMusica = n;
            PlayerPrefs.SetFloat("volume musica", n);
        }
    }

    public void ImpostaVolumeEffettiSonoriGame(float n)
    {
        if (!variabili.primoFrame)
        {
            volumeEffettiSonoriGame = n;
            PlayerPrefs.SetFloat("volume effetti sonori in game", n);
        }
    }

    public void ImpostaVolumeEffettiSonoriMenù(float n)
    {
        if (!variabili.primoFrame)
        {
            volumeEffettiSonoriMenù = n;
            PlayerPrefs.SetFloat("volume effetti sonori in menù", n);
        }
    }

    public void InviamiUnaMail()
    {
        Application.OpenURL("mailto:ifralex.developer@gmail.com?subject=About%20Bebe");
    }
}