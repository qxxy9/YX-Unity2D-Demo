using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class TargetSaveData 
{

    public Vector3 savePosition;
    public int facingDir;
    public quaternion rotate;
    public CharacterStatSaveData characterStatSaveData;
    
    public TargetSaveData(Vector3 _savePosition, int _facingDir,quaternion _rotate, CharacterStatSaveData _characterStatSaveData)
    {
        this.savePosition = _savePosition;
        this.facingDir = _facingDir;
        this.rotate = _rotate;
        this.characterStatSaveData = _characterStatSaveData;
        
    }

}
