using UnityEngine;

public class PsychicPunchController : SkillParent
{
    public GameObject esferaPrefab;
    public float initialSpeed;
    public float initialImpulseForce;
    public float maxImpulseForce;
    public float maxSpeed;
    public float initialScale;
    public float maxScale;
    public float initialDamage;
    public float maxDamage;
    public float initialManaCost;
    public float maxManaCost;

    private GameObject currentSphere;
    public Transform spawnPoint;
    public float currentSpeed;
    private float currentScale;
    public float currentDamage;
    public float currentImpulseForce;
    private float currentManaCost;
    public bool onColdown;

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
            current_coldown -= Time.deltaTime;
            //Debug.Log("remaining time = " + remainingTime);
            if (current_coldown <= 0f)
            {
                onColdown = false;
                current_coldown = 0f;
            }
        }
        if (playerStats.selectedMode == GameMode.PyshicShot/* && playerStats.currentMana >= initialManaCost*/)
        {

            if (!onColdown && Input.GetMouseButtonDown(0)/* && playerStats.currentMana >= initialManaCost*/)
            {
                if (playerStats.currentMana < initialManaCost)
                {
                    FindObjectOfType<GameManager>().ShowNoManaText();
                    Debug.Log("No tienes mana suficiente");
                    return;
                }
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
                currentImpulseForce = initialImpulseForce;
            }
            else if (!onColdown && Input.GetMouseButton(0) && currentSphere != null)
            {
                // Incrementar la escala y la velocidad de la esfera mientras se mantiene pulsado el bot�n izquierdo del rat�n
                if (currentSphere != null)
                {
                    currentScale += Time.deltaTime;
                    currentScale = Mathf.Clamp(currentScale, 0f, maxScale);
                    currentSphere.transform.localScale = Vector3.one * currentScale;

                    currentSpeed = Mathf.Lerp(initialSpeed, maxSpeed, currentScale / maxScale);
                    currentDamage = Mathf.Lerp(initialDamage, maxDamage, currentScale / maxScale);
                    currentManaCost = Mathf.Lerp(initialManaCost, maxManaCost, currentScale / maxScale);
                    currentImpulseForce = Mathf.Lerp(initialImpulseForce, maxImpulseForce, currentScale / maxScale);
                }
            }
            else if (!onColdown && Input.GetMouseButtonUp(0) && currentSphere != null)
            {
                if (playerStats.currentMana >= currentManaCost)
                {
                    playerStats.UseSkill(currentManaCost);
                    //Debug.Log("currentmana psychicShot = " + playerStats.currentMana);
                    // Lanzar la esfera cuando se suelta el bot�n izquierdo del rat�n
                    if (currentSphere != null)
                    {
                        onHand = false;
                        Rigidbody rigidbody = currentSphere.GetComponent<Rigidbody>();
                        rigidbody.velocity = transform.forward * currentSpeed;


                    }
                    // Asignar el da�o a la esfera
                    PsychicBall ballScript = currentSphere.GetComponent<PsychicBall>();
                    if (ballScript != null)
                    {
                        int damageInt = Mathf.RoundToInt(currentDamage);
                        ballScript.damage = damageInt;
                        ballScript.velocity = currentSpeed;
                        ballScript.impulseForce = currentImpulseForce;
                        ballScript.throwed = true;
                    }
                    currentSphere = null;
                    // Iniciar el cooldown
                    onColdown = true;
                    current_coldown = coldown;
                    chargeBar.GetComponent<ChargeBar>().chargeBar.fillAmount = 0;
                    chargeBar.SetActive(false);
                }
                else
                {
                    //mostramos el texto "no mana"
                    FindObjectOfType<GameManager>().ShowNoManaText();
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
        else
        {
        }
    }
    private void LateUpdate()
    {
        if (onHand)
        {
            currentSphere.transform.position = spawnPoint.position;

        }
    }

    public override void Activate()
    {
    }

    public override void Disable()
    {
        Destroy(currentSphere);
        currentSphere = null;
        onHand = false;
        
    }
}

