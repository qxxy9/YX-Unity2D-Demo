using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IsSaveManager 
{
    void LoadData(GameData _data);

    void SaveData(ref GameData _data);



}
