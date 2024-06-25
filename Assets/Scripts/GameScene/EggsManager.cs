using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EggsManager
{

    // Start is called before the first frame update   

    //public string[] _eggType;
    //public int[] _eggNumber;
    //public bool[] _hasEgg;    
    private string eggType;
    private int eggNumber;
    private bool hasEgg;
    private int totalEgg;
   

    public string EggType { get => eggType; set => eggType = value; }
    public int EggNumber { get => eggNumber; set => eggNumber = value; }
    public bool HasEgg { get => hasEgg; set => hasEgg = value; }
    public int TotalEgg { get => totalEgg; set => totalEgg = value; }

    public EggsManager(string eggType, int eggNumber, bool hasEgg, int totalEgg)
    {
        this.EggType = eggType;
        this.EggNumber = eggNumber;
        this.HasEgg = hasEgg;
        this.TotalEgg = totalEgg;
    }
    

}
