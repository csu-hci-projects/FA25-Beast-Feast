using System;
using UnityEngine;

public class SizeIndicator : MonoBehaviour
{
    [SerializeField] GameObject indicatorPrefab;
    [SerializeField] bool isDungeonEnemy;
    private GameObject indicatorInstance;
    private Renderer indicatorRenderer;
    private GameObject playerObject;
    private Transform player;
    private SizeStats playerStats;
    private SizeStats enemyStats;


    void OnEnable()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerStats = playerObject.GetComponent<SizeStats>();
        }

        indicatorInstance = Instantiate(indicatorPrefab, transform);
        if (isDungeonEnemy)
        {
            indicatorInstance.transform.localPosition = new Vector3(0, 0.05f, 0);
            indicatorInstance.transform.localScale = Vector3.one * 0.01f;
        } else
        {
            indicatorInstance.transform.localPosition = new Vector3(0, 2f, 0);
            indicatorInstance.transform.localScale = Vector3.one * 0.8f;            
        }

        
        indicatorRenderer = indicatorInstance.GetComponent<Renderer>();

        enemyStats = GetComponent<SizeStats>();
    }

    void Update()
    {
        if (player == null || indicatorRenderer == null) return;

        // Show/hide the indicator depending on left shift key
        bool showIndicator = Input.GetKey(KeyCode.LeftShift);

        indicatorRenderer.gameObject.SetActive(showIndicator);

        // Only change color if indicator is visible
        if (showIndicator)
        {
            int sizeValue = enemyStats.GetSize();
            int playerSize = playerStats.GetSize();
            if (sizeValue < playerSize)
                indicatorRenderer.material.color = Color.green;
            else if (sizeValue > playerSize)
                indicatorRenderer.material.color = Color.red;
            else
                indicatorRenderer.material.color = Color.yellow;

            // Debug.Log("Showing Indicator!");
        }

    }
}
