using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class impostazioniInGame : MonoBehaviour
{
    public RectTransform elementi;
    public List<Dropdown> listaDropdown;
    public List<ToggleController> listaToggle;
    public List<Slider> listaSlider;
    public List<Image> listaColori;
    [Space]
    public LineRenderer mirino;
    public FlexibleColorPicker colorPicker;
    public AudioListener audioListener;
    public AudioSource musicaSfondo;
    public List<AudioSource> musicaEffettiSonori;
    public Image sfondoIm;

    public static impostazioni.Modalità modalità;
    public static impostazioni.Direzioni direzioneMira;
    public static impostazioni.MiraRispetto miraRispetto;
    public static float sensibilità;
    public static float tempoPremutoPowerUp;

    public static Color coloreMirino;
    public static Color coloreSfondo;
    public static Sprite sfondo;

    public static bool audioAttivo;
    public static impostazioni.FrequenzaCinguettii frequenzaCinguettii;
    public static bool cinguettaSeSubisceDanno;
    public static float volumeMusica;
    public static float volumeEffettiSonori;


    void Awake()
    {
        modalità = impostazioni.modalità;
        direzioneMira = impostazioni.direzione;
        miraRispetto = impostazioni.miraRispetto;
        sensibilità = impostazioni.sensibilità;
        tempoPremutoPowerUp = impostazioni.tempoPremerePowerUp;

        coloreMirino = impostazioni.coloreMirino;
        coloreSfondo = impostazioni.coloreSfondo;

        audioAttivo = impostazioni.audioAttivo;
        frequenzaCinguettii = impostazioni.frequenzaCinguettii;
        cinguettaSeSubisceDanno = impostazioni.attivaCinguettiiSeSubisceDanno;
        volumeMusica = impostazioni.volumeMusica;
        volumeEffettiSonori = impostazioni.volumeEffettiSonoriGame;

        listaDropdown[0].value = (int)modalità;
        listaDropdown[1].value = (int)direzioneMira;
        listaDropdown[2].value = (int)miraRispetto;
        listaDropdown[3].value = (int)frequenzaCinguettii;

        listaToggle[0].isOn = audioAttivo;
        listaToggle[1].isOn = cinguettaSeSubisceDanno;

        listaSlider[0].value = sensibilità;
        listaSlider[1].value = tempoPremutoPowerUp;
        listaSlider[2].value = volumeMusica;
        listaSlider[3].value = volumeEffettiSonori;

        listaColori[0].color = coloreMirino;
        listaColori[1].color = coloreSfondo;
        /*
        float alt = 0;
        for (int i = 0; i < elementi.childCount; i++)
            if (elementi.GetChild(i).gameObject.activeInHierarchy)
                alt += elementi.GetChild(i).GetComponent<RectTransform>().sizeDelta.y + 15;
        elementi.sizeDelta = new Vector2(elementi.sizeDelta.x, alt);
        */
        ImpostaColoreMirino(coloreMirino);
        ImpostaAudioAttivo(audioAttivo);
        ImpostaVolumeMusica(volumeMusica);
        ImpostaVolumeEffettiSonori(volumeEffettiSonori);
        ImpostaColoreSfondo(impostazioni.coloreSfondo);
        Application.targetFrameRate = impostazioni.fpsGame;
        gameObject.SetActive(false);
    }

    public void ApriImpostazioni(bool b)
    {
        gameObject.SetActive(b);
        Time.timeScale = b ? 0 : 1;
    }

    public void Esci()
    {
        AssetBundle.UnloadAllAssetBundles(true);
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void SelezionaColore(int n)
    {
        MoreMountains.NiceVibrations.MMVibrationManager.Selezione();
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
        for (int i = 0; i < listaColori.Count; i++)
            listaColori[i].transform.parent.GetChild(0).GetComponent<Image>().color = Color.blue;
        listaColori[n].transform.parent.GetChild(0).GetComponent<Image>().color = Color.green;
        /*
        if (listaColori[n].transform.parent.GetChild(0).GetComponent<Image>().color != Color.green)
        {
            colorPicker[n].startingColor = coloreMirino;
            colorPicker[n].gameObject.SetActive(true);
            listaColori[n].transform.parent.GetChild(0).GetComponent<Image>().color = Color.green;
        }
        else
        {
            colorPicker[n].gameObject.SetActive(false);
            listaColori[n].transform.parent.GetChild(0).GetComponent<Image>().color = Color.blue;
        }*/
    }

    public void ImpostaModalità(int n) => modalità = (impostazioni.Modalità)n;
    public void ImpostaDirezioneMira(int n) => direzioneMira = (impostazioni.Direzioni)n;
    public void ImpostaMiraRispetto(int n) => miraRispetto = (impostazioni.MiraRispetto)n;
    public void ImpostaSensibilità(float n) => sensibilità = n;
    public void ImpostaTempoPremutoPowerUp(float n) => tempoPremutoPowerUp = n;
    public void ImpostaColoreMirino(Color c)
    {
        UnityEngine.Gradient gradiente = new UnityEngine.Gradient();
        gradiente.SetKeys(new GradientColorKey[] { new GradientColorKey(c, 0), new GradientColorKey(Color.white, 1) }, new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(0, 1) });
        mirino.colorGradient = gradiente;
        coloreMirino = c;
    }
    public void ImpostaAudioAttivo(bool b)
    {
        audioAttivo = b;
        AudioListener.volume = b ? 1 : 0;
    }
    public void ImpostaFrequenzaCinguettii(int n) => frequenzaCinguettii = (impostazioni.FrequenzaCinguettii)n;
    public void ImpostaCinguettaSeSubisceDanno(bool b) => cinguettaSeSubisceDanno = b;
    public void ImpostaVolumeMusica(float v)
    {
        volumeMusica = v;
        musicaSfondo.volume = v;
    }
    public void ImpostaVolumeEffettiSonori(float v)
    {
        volumeEffettiSonori = v;
        for (int i = 0; i < musicaEffettiSonori.Count; i++)
            musicaEffettiSonori[i].volume = v;
    }

    public void ImpostaColoreSfondo(Color c)
    {
        if (!impostazioni.livelloDiProva)
        {
            if (impostazioni.tipoSfondo == impostazioni.TipiSfondo.immagineStatica2D)
            {
                sfondoIm.sprite = sfondo;
                listaColori[1].transform.parent.parent.gameObject.SetActive(false);
            }
            else if (impostazioni.tipoSfondo == impostazioni.TipiSfondo.tintaUnita2D)
            {
                sfondoIm.color = c;
                coloreSfondo = c;
                listaColori[1].color = c;
            }
            else
            {
                sfondoIm.gameObject.SetActive(false);
                listaColori[1].transform.parent.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            sfondoIm.color = c;
            coloreSfondo = c;
            listaColori[1].color = c;
        }
    }
}