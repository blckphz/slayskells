using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    private IAstarAI ai;
    private Animator anim;
    private Transform playerTransform;

    [Header("Combat Settings")]
    public float attackRange = 1.2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    void Start()
    {
        ai = GetComponent<IAstarAI>();
        anim = GetComponent<Animator>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    void Update()
    {
        if (playerTransform == null || ai == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange)
        {
            ai.isStopped = true;
            TryAttack();
        }
        else
        {
            ai.isStopped = false;
            ai.destination = playerTransform.position;
        }

        UpdateAnimator();
    }

    void TryAttack()
    {
        // Only trigger the animation if enough time has passed
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            anim.SetTrigger("isAttacking"); // Now using a Trigger
            lastAttackTime = Time.time;
        }
    }

    void UpdateAnimator()
    {
        Vector3 velocity = ai.velocity;

        // Use X and Y for 2D movement
        if (velocity.magnitude > 0.1f)
        {
            Vector2 movementVector = new Vector2(velocity.x, velocity.y).normalized;
            anim.SetFloat("x", movementVector.x);
            anim.SetFloat("y", movementVector.y);
        }

        anim.SetFloat("Speed", ai.isStopped ? 0f : velocity.magnitude);
    }
}