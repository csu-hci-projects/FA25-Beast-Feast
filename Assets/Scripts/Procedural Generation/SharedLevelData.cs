using System;
using UnityEngine;
using Random = System.Random;

[ExecuteAlways]
[DisallowMultipleComponent]
public class SharedLevelData : MonoBehaviour
{
    public static SharedLevelData Instance { get; private set; }

    [SerializeField] int scale = 1;
    [SerializeField] int seed = Environment.TickCount;

    [ContextMenu("Generate New Seed")]
    public void GenerateSeed()
    {
        seed = Environment.TickCount;
        random = new Random(seed);
    }

    Random random;
    public int Scale => scale;
    public Random Rand => random;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            enabled = false;
            Debug.LogWarning("Duplicate SharedLevelData detected and disabled", this);
        }
        Debug.Log(Instance.GetInstanceID());
        random = new Random(seed);
    }

    public void ResetRandom()
    {
        random = new Random(seed);
    }
}
