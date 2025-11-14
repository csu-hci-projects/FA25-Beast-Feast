using System.Drawing;
using NUnit.Framework.Internal;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SizeStats : MonoBehaviour
{
    [SerializeField] int size = 0;
    [SerializeField] TextMeshProUGUI sizeText;

    void Start()
    {
        if(gameObject.tag == "Player")
        {
            updateSize();
        }
    }

    public bool TryEatEnemy(int enemySize)
    {
        
        if (size >= enemySize)
        {
            if (size < 10)
            {
                size++;
                if(gameObject.tag == "Player")
                {
                    updateSize();
                }
  
            } 
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
        return this.size;
    }

    public void updateSize()
    {
        sizeText.text = "Current Size: " + size;
    }
}
