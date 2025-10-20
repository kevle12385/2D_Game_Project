using UnityEngine;
using TMPro;

public class EnemyHealthText : MonoBehaviour
{
    public Health2D target;       // Enemy health reference
    public TMP_Text hpText;       // The TextMeshPro text component
    public Vector3 offset = new Vector3(0, 1.3f, 0);
    public float followLerp = 10f;

    void Awake()
    {
        if (!hpText)
        {
            hpText = GetComponentInChildren<TMP_Text>();
        }
    }

    void LateUpdate()
    {
        if (!target)
        {
            Destroy(gameObject);
            return;
        }

        // Follow and face camera
        Vector3 goal = target.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, goal, followLerp * Time.deltaTime);
        if (Camera.main) transform.forward = Camera.main.transform.forward;

        // Update text
        hpText.text = $"{target.currentHP} / {target.maxHP}";
    }
}
