using UnityEngine;

public class GameMode_SaveAll : GameMode
{
    private new void Start()
    {
        base.Start();
        
        //PROPIO DE ESTE GAMEMODE, REQUIERE SALVAR TODOS
        TotalPrisoners = GetLevelPrisoners();
        if (TotalPrisoners < 1)
        {
            Debug.LogError("NO HAY PRISIONEROS EN ESTE NIVEL!!!");
            Debug.DebugBreak();
        }
        else
        {
            //En este GameMode necesito salvar a TODOS
            PrisonersNeeded = TotalPrisoners ;
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
