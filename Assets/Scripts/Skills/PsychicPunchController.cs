using UnityEngine;

public class PsychicPunchController : MonoBehaviour
{
    public GameObject esferaPrefab;
    public float initialSpeed;
    public float maxSpeed;
    public float initialScale;
    public float maxScale;
    public float initialDamage;
    public float maxDamage;
    public float initialManaCost;
    public float maxManaCost;
    public float cooldown;

    private GameObject currentSphere;
    public Transform spawnPoint;
    public float currentSpeed;
    private float currentScale;
    public float currentDamage;
    private float currentManaCost;
    private bool inCooldown;
    private float remainingTime;

    PlayerStats playerStats;
    public bool onHand = false;

    public GameObject chargeBar;

    private void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (playerStats.selectedMode == PlayerStats.GameMode.PyshicShot && playerStats.currentMana >= initialManaCost)
        {
            if (inCooldown)
            {
                remainingTime -= Time.deltaTime;
                Debug.Log("remaining time = " + remainingTime);
                if (remainingTime <= 0f)
                {
                    inCooldown = false;
                    remainingTime = 0f;
                }
            }
            if (!inCooldown && Input.GetMouseButtonDown(0))
            {
                chargeBar.SetActive(true);
                chargeBar.GetComponent<ChargeBar>().target = gameObject;
                chargeBar.GetComponent<ChargeBar>().ChangeObjetive(maxDamage);
                // Crear una nueva esfera
                currentSphere = Instantiate(esferaPrefab, spawnPoint.position, Quaternion.identity);
                currentSphere.name = "Psychic Ball";
                onHand = true;
                currentSpeed = initialSpeed;
                currentScale = initialScale;
                currentDamage = initialDamage;
                currentManaCost = initialManaCost;
            }
            else if (!inCooldown && Input.GetMouseButton(0) && currentSphere != null)
            {
                // Incrementar la escala y la velocidad de la esfera mientras se mantiene pulsado el botón izquierdo del ratón
                if (currentSphere != null)
                {
                    currentScale += Time.deltaTime;
                    currentScale = Mathf.Clamp(currentScale, 0f, maxScale);
                    currentSphere.transform.localScale = Vector3.one * currentScale;

                    currentSpeed = Mathf.Lerp(initialSpeed, maxSpeed, currentScale / maxScale);
                    currentDamage = Mathf.Lerp(initialDamage, maxDamage, currentScale / maxScale);
                    currentManaCost = Mathf.Lerp(initialManaCost, maxManaCost, currentScale / maxScale);
                }
            }
            else if (!inCooldown && Input.GetMouseButtonUp(0) && currentSphere != null)
            {
                if (playerStats.currentMana >= currentManaCost)
                {
                    playerStats.UseSkill(currentManaCost);
                    // Lanzar la esfera cuando se suelta el botón izquierdo del ratón
                    if (currentSphere != null)
                    {
                        onHand = false;
                        Rigidbody rigidbody = currentSphere.GetComponent<Rigidbody>();
                        rigidbody.velocity = transform.forward * currentSpeed;


                    }
                    // Asignar el daño a la esfera
                    PsychicBall ballScript = currentSphere.GetComponent<PsychicBall>();
                    if (ballScript != null)
                    {
                        int damageInt = Mathf.RoundToInt(currentDamage);
                        ballScript.damage = damageInt;
                        ballScript.velocity = currentSpeed;
                        ballScript.throwed = true;
                    }
                    currentSphere = null;
                    // Iniciar el cooldown
                    inCooldown = true;
                    remainingTime = cooldown;
                    chargeBar.GetComponent<ChargeBar>().chargeBar.fillAmount = 0;
                    chargeBar.SetActive(false);
                }
                else
                {
                    Debug.Log("No tienes mana suficiente");
                    onHand = false;
                    Destroy(currentSphere);
                }

            }
            else
            {
                Debug.Log("Skill on Coldown...");
            }
        }
    }
    private void LateUpdate()
    {
        if (onHand)
        {
            currentSphere.transform.position = spawnPoint.position;

        }
    }
}

