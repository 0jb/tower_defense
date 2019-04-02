using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Wave", menuName = "Data/Wave/WaveObj", order = 1)]
public class WaveScriptableObj : ScriptableObject
{
    public string waveName = "Wave";
    public List<Spawn> spawns = new List<Spawn>();

    [System.Serializable]
    public class Spawn
    {
        public float duration = 1.0f;

        public Spawn(float Duration = 1.0f)
        {
            duration = Duration;
        }
    }
}