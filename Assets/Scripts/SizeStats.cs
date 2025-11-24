using System.Drawing;
using UnityEngine;

public class SizeStats : MonoBehaviour
{
    [SerializeField] int size = 0;
    [SerializeField] bool isBoss = false;
    private int health = 3;

    void Start()
    {
        
    }

    public bool TryEatEnemy(int enemySize)
    {
        
        if (size >= enemySize)
        {
            if (size < 10) size++;
            return true;
        }
        else
        {
            Debug.Log("Enemy too big to eat!");
            return false;
        }

    }

    public bool TryEatPlayer(int playerSize)
    {
        if (playerSize > size)
        {
            Debug.Log("Player too big to eat!");
            return false;
        }
        else
        {
            return true;
        }

    }
    
    public int GetSize()
    {
        return size;
    }

    public int GetHealth()
    {
        return health;
    }
    
    public bool GetIsBoss()
    {
        return isBoss;
    }

    public void DecreaseHealth()
    {
        health--;
    }
}
