using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable<TSaveData> where TSaveData : SaveDataBase
{
    public TSaveData GetSaveData();
    public void LoadSaveData(TSaveData saveData);
}
