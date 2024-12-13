using UnityEngine;

public class GameMode_SaveMost : GameMode
{
    private new void Start()
    {
        base.Start();
        
        //PROPIO DE ESTE GAMEMODE
        TotalPrisoners = GetLevelPrisoners();
        if (TotalPrisoners < 1)
        {
            Debug.LogError("NO HAY PRISIONEROS EN ESTE NIVEL!!!");
            Debug.DebugBreak();
        }
        else
        {
            //por defecto aplico porcentaje de la mayoría, para completar el nivel
            PrisonersNeeded = Mathf.CeilToInt((float)TotalPrisoners * 0.8f);
        }
    }

    private int GetLevelPrisoners()
    {
        var tempPrisoners = 0;
        var tempo = FindObjectsOfType<minion_spawnpoint>();
        foreach (var comp in tempo)
        {
            tempPrisoners += comp.amount;
        }
        return tempPrisoners;
    }
}
