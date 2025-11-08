using UnityEngine;

public class SizeIndicator : MonoBehaviour
{
    public Transform player;
    public GameObject indicatorPrefab;
    private GameObject indicatorInstance;
    private Renderer indicatorRenderer;
    public float sizeValue = 5f; // manually set this per enemy
    public float playerSize = 10f; // manually set or assign from player
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        indicatorInstance = Instantiate(indicatorPrefab, transform);
        indicatorInstance.transform.localPosition = new Vector3(0, 2f, 0);
        
        indicatorRenderer = indicatorInstance.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || indicatorRenderer == null) return;

        // Show/hide the indicator depending on M key
        bool showIndicator = Input.GetKey(KeyCode.M);

        indicatorRenderer.gameObject.SetActive(showIndicator);

        // Only change color if indicator is visible
        if (showIndicator)
        {
            if (sizeValue < playerSize)
                indicatorRenderer.material.color = Color.green;
            else if (sizeValue > playerSize)
                indicatorRenderer.material.color = Color.red;
            else
                indicatorRenderer.material.color = Color.yellow;
        }
    }
}
