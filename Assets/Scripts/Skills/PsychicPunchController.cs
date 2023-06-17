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
    public float coldown;

    private GameObject currentSphere;
    public Transform spawnPoint;
    public float currentSpeed;
    private float currentScale;
    public float currentDamage;
    private float currentManaCost;
    public bool onColdown;
    public float remainingTime;

    PlayerStats playerStats;
    public bool onHand = false;

    public GameObject chargeBar;

    private void Start()
    {
        //playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        playerStats = FindObjectOfType<PlayerStats>();
        chargeBar = FindObjectOfType<ChargeBar>().gameObject;
    }

    void Update()
    {
        //EL COLDOWN LO CALCULAMOS FUERA, PARA QUE CUANDO CAMBIE DE ARMA SE SIGA CALCULANDO
        if (onColdown)
        {
            remainingTime -= Time.deltaTime;
            //Debug.Log("remaining time = " + remainingTime);
            if (remainingTime <= 0f)
            {
                onColdown = false;
                remainingTime = 0f;
            }
        }
        if (playerStats.selectedMode == PlayerStats.GameMode.PyshicShot && playerStats.currentMana >= initialManaCost)
        {

            if (!onColdown && Input.GetMouseButtonDown(0))
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
            else if (!onColdown && Input.GetMouseButton(0) && currentSphere != null)
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
            else if (!onColdown && Input.GetMouseButtonUp(0) && currentSphere != null)
            {
                if (playerStats.currentMana >= currentManaCost)
                {
                    playerStats.UseSkill(currentManaCost);
                    //Debug.Log("currentmana psychicShot = " + playerStats.currentMana);
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
                    onColdown = true;
                    remainingTime = coldown;
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

