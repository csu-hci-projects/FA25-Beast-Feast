using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class DirectedAgent : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    [SerializeField] TextMeshProUGUI sizeText;
    [SerializeField] private List<Image> hearts;
    [SerializeField] private Sprite fullHeart; 
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] float attackRadius = 5f;

    private SizeStats playerStats;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updateRotation = false; // We’ll handle rotation manually
        playerStats = GetComponent<SizeStats>();
    }

    void Start()
    {
        SetSizeText();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        // Move relative to camera direction
        if (Camera.main)
        {
            Vector3 camForward = Camera.main.transform.forward;
            camForward.y = 0;
            Vector3 camRight = Camera.main.transform.right;
            camRight.y = 0;
            inputDir = (camForward.normalized * v + camRight.normalized * h).normalized;
        }

        if (inputDir.magnitude >= 0.1f)
        {
            agent.Move(inputDir * agent.speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputDir), 0.15f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProcessAttack();
            SetSizeText();
        }

        // Animation blend (same parameter as before)
        float speed = inputDir.magnitude * agent.speed;
        animator.SetFloat("forwardSpeed", speed);
        UpdateHealth();
    }

    private void ProcessAttack()
    {
        GameObject[] enemies = GetEnemiesInAttackRange();
            foreach (GameObject enemy in enemies)
            {
                bool eatEnemy = playerStats.TryEatEnemy(enemy.GetComponent<SizeStats>().GetSize());
                if (eatEnemy)
                {
                if (enemy.GetComponent<SizeStats>().GetIsBoss())
                {
                    GetComponent<GameOverScreen>().Win();
                }
                    Destroy(enemy);
                }
            }
    }

    private GameObject[] GetEnemiesInAttackRange()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> closeEnemies = new List<GameObject>();
        foreach (GameObject obj in taggedObjects)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            if (distance <= attackRadius)
            {
                closeEnemies.Add(obj);
            }
        }
        return closeEnemies.ToArray();
    }
    
    private void SetSizeText()
    {
        sizeText.text = "Current Size: " + playerStats.GetSize().ToString();
    }

    private void UpdateHealth()
    {
        int health = playerStats.GetHealth();
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < health)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }
}
