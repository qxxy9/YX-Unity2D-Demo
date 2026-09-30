using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffData 
{
    public string buffId;
    public float duration;
    public int mod;

    public BuffData(string _id,float _duration,int _mod)
    {
        this.buffId = _id;
        this.duration = _duration;
        this.mod = _mod;
    }

    public void timeLoss()
    {
        duration-=Time.deltaTime;
    }

    public bool CheckTime()
    {
        if (duration >0) 
            return true;
        return false;
    }


}
