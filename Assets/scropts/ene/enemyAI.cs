using UnityEngine;
using Pathfinding; // Critical: Allows access to AIPath and Seeker

public class EnemyAI : MonoBehaviour
{
    private IAstarAI ai; // Interface that works for AIPath or RichAI
    private Transform playerTransform;

    void Start()
    {
        // Cache the AI component
        ai = GetComponent<IAstarAI>();

        // Find the player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // Update destination every frame (or use a timer for better performance)
        if (playerTransform != null && ai != null)
        {
            ai.destination = playerTransform.position;
        }
    }
}