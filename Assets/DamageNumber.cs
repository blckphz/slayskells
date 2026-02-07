using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float lifeTime = 1f;

    // Using TMP_Text covers BOTH TextMeshPro and TextMeshProUGUI
    private TMP_Text textMesh;
    private Color textColor;

    void Awake()
    {
        // This looks at the parent AND all children for the component
        textMesh = GetComponentInChildren<TMP_Text>();

        if (textMesh == null)
        {
            Debug.LogError($"No TMP component found on {gameObject.name} or its children!");
            return;
        }

        textColor = textMesh.color;
    }

    public void Setup(float damageAmount)
    {
        if (textMesh != null)
        {
            textMesh.text = damageAmount.ToString();
        }
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Move upward
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        if (textMesh != null)
        {
            // Update the alpha and apply it back to the mesh
            textColor.a -= (1f / lifeTime) * Time.deltaTime;
            textMesh.color = textColor;
        }
    }
}