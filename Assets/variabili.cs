using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class variabili : MonoBehaviour
{
    [SerializeField] private Sprite[] baccheSingoleL;
    [SerializeField] private Sprite grappoloBaccaL;
    [SerializeField] public Sprite[] baccheSingoleBiancoL;
    [SerializeField] private Sprite grappoloBaccaBiancoL;
    [SerializeField] public List<DettagliPotenziamento> _listaDettagliPotenziamenti;
    [SerializeField] public List<Sprite> listaCuoriSprite;
    [SerializeField] Font _fontDislessia;
    [SerializeField] Font _fontNormale;

    public static bool primoFrame = true;
    public static Sprite[] baccheSingole;
    public static Sprite grappoloBacca;
    public static Sprite[] baccheSingoleBianco;
    public static Sprite grappoloBaccaBianco;
    public static List<DettagliPotenziamento> listaDettagliPotenziamenti;
    public static List<int> quantitàPowerUps;
    public static List<int> tempiPowerUps;
    public static List<int> potenzePowerUps;
    public static Font fontDislessia;
    public static Font fontNormale;

    void Awake()
    {
        if (primoFrame)
        {
            baccheSingole = baccheSingoleL;
            baccheSingoleBianco = baccheSingoleBiancoL;
            grappoloBacca = grappoloBaccaL;
            grappoloBaccaBianco = grappoloBaccaBiancoL;
            partitaManager.listaCuoriIm = listaCuoriSprite;
            DettagliPotenziamento.listaElementi = _listaDettagliPotenziamenti;
            fontDislessia = _fontDislessia;
            fontNormale = _fontNormale;
        }
    }

    void Start() => primoFrame = false;
}