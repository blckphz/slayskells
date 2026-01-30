using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float lifeTime = 1f;
    private TextMeshPro textMesh;
    private Color textColor;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        textColor = textMesh.color;
    }

    public void Setup(float damageAmount)
    {
        textMesh.text = damageAmount.ToString();
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Move upward
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // Fade out alpha over time
        textColor.a -= (1f / lifeTime) * Time.deltaTime;
        textMesh.color = textColor;
    }
}