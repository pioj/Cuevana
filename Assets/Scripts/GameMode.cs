using System;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [Header("Game")]
    public static bool AllRescued;
    public static TimeSpan TimePlayed;
    protected DateTime _startTime;

    [Header("Nave")]
    public static bool PlayerLanded;
    public static int Lives;
    public static Vector2 CurrentCheckPoint;

    [Header("Prisioneros")]
    public static int TotalPrisoners;
    public static int PrisonersNeeded;
    public static int PrisonersRescued;
    public static int PrisonersAboard;
    public static int PrisonersLeft;
    public static int PrisonersKilled;

    [Header("Info")] 
    [SerializeField] protected bool _AllRescued;
    [SerializeField] protected bool _PlayerLanded;
    [SerializeField] protected int _Lives;
    [SerializeField] protected int _TotalPrisoners;
    [SerializeField] protected int _PrisonersNeeded;
    [SerializeField] protected int _PrisonersRescued;
    [SerializeField] protected int _PrisonersAboard;
    [SerializeField] protected int _PrisonersLeft;
    [SerializeField] protected int _PrisonersKilled;

    protected void Start()
    {
        _startTime = new DateTime();
        
        //ESPACIO PARA PERSONALIZAR EL GAMEMODE
        //
        
        _startTime = DateTime.Now;
        Lives = 3;
        //
        CurrentCheckPoint = Vector2.zero;
    }

    protected void Update()
    {
        AllRescued = PrisonersRescued >= PrisonersNeeded;
        PrisonersLeft = TotalPrisoners - (PrisonersRescued + PrisonersKilled);

        if (Lives < 1)
        {
            GameOver(0);
        }
        else if (AllRescued)
        {
            GameOver(1);
        }
        else if (PrisonersNeeded - PrisonersRescued > PrisonersLeft)
        {
            GameOver(2);
        }
        
        //relleno info privada
        _AllRescued = AllRescued;
        _PlayerLanded = PlayerLanded;
        _Lives = Lives;
        _TotalPrisoners = TotalPrisoners;
        _PrisonersNeeded = PrisonersNeeded;
        _PrisonersRescued = PrisonersRescued;
        _PrisonersAboard = PrisonersAboard;
        _PrisonersLeft = PrisonersLeft;
        _PrisonersKilled = PrisonersKilled;
    }

    protected void GameOver(int result)
    {
        TimePlayed = DateTime.Now - _startTime;
        Debug.Log("playtime: " +TimePlayed.Seconds);
        
        switch (result)
        {
            case 0:     // 0 - pierde todas las vidas
                Debug.Log("GAME OVER! No te quedan más vidas!");
                Debug.Break();
                break;
            case 1:     // 1 - gana la partida
                Debug.Log("YOU WIN! Level Complete!");
                Debug.Break();
                break;
            case 2:     // 2 - mueren más minions de los necesarios para completar el nivel
                Debug.Log("GAME OVER! Murieron más prisioneros de los necesarios!");
                Debug.Break();
                break;
            case 3:     // 3 - pierdes, otros motivos
                Debug.Log("GAME OVER! Otros motivos...");
                Debug.Break();
                break;
        }
    }
}
