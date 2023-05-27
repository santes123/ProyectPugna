using UnityEngine;
using UnityEngine.UI;

public class FloatingDamageText : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float fadeSpeed = 2f;

    private Text damageText;
    private float lifeTime = 0.5f;
    private float timer = 0f;

    void Start()
    {
        damageText = GetComponent<Text>();
    }

    void Update()
    {
        if (damageText.isActiveAndEnabled)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            damageText.color = Color.Lerp(damageText.color, Color.clear, fadeSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            if (timer >= lifeTime)
            {
                damageText.enabled = false;
            }
        }
    }

    public void SetDamageText(string text, Color color)
    {
        timer = 0f;
        if (damageText != null)
        {
            damageText.text = text;
            damageText.color = color;
        }
        else
        {
            Debug.LogError("Text component not found on the FloatingDamageText object.");
        }
    }
}
