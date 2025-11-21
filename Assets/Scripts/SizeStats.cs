using UnityEngine;

public class SizeStats : MonoBehaviour
{
    [SerializeField] int size = 1;       // start at size 1 instead of 0
    [SerializeField] float baseScale = 0.6f;
    [SerializeField] float scalePerSize = 0.15f;

    void Start()
    {
        UpdateScale();
    }

    public bool TryEatEnemy(int enemySize)
    {
        if (size >= enemySize)
        {
            if (size < 10)
            {
                size++;
                UpdateScale();     // <-- grow when size increases
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
        return playerSize <= size;
    }

    public int GetSize()
    {
        return size;
    }

    private void UpdateScale()
    {
        float newScale = baseScale + (size - 1) * scalePerSize;
        transform.localScale = Vector3.one * newScale;
    }
}