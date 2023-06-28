using UnityEngine;
using UnityEngine.UI;

public class SkillBar : MonoBehaviour
{
    // Estructura para almacenar los datos de cada habilidad
    [System.Serializable]
    public class Habilidad
    {
        public Sprite imagen;
        public KeyCode tecla;
        public float cooldown;
        public GameObject container;
    }

    public Habilidad[] habilidades; // Arreglo de habilidades

    // Prefab del cuadrado de habilidad
    public GameObject cuadradoPrefab;
    public GameObject barPrefab;

    // Referencia al contenedor de las habilidades
    public Transform contenedor;
    private Text [] coldowns;
    private Image[] coldownImages;
    private Image[] imageSkill;
    private PlayerStats player;

    //booleanos para los coldowns
    bool coldown1 = false;
    bool coldown2 = false;
    bool coldown3 = false;
    //bool coldown4 = false;

    float remainingColdownTime = 0f;
    float expectedBoomerangColdownTime;
    //public GameObject skillBar;

    private void Start()
    {
        //player = GameObject.Find("Player").GetComponent<PlayerStats>();
        player = FindObjectOfType<PlayerStats>();
        habilidades[0].container = FindObjectOfType<BoomerangController>().gameObject;
        habilidades[1].container = FindObjectOfType<UseAttractThrowSkill>().gameObject;
        habilidades[2].container = FindObjectOfType<PsychicPunchController>().gameObject;
        habilidades[3].container = FindObjectOfType<DashController>().gameObject;
        contenedor = GameObject.FindGameObjectWithTag("SkillBar").transform;

        coldownImages = new Image[habilidades.Length];
        coldowns = new Text[habilidades.Length];
        imageSkill = new Image[habilidades.Length];
        // Crear los cuadrados de habilidad dinámicamente
        for (int i = 0; i < habilidades.Length; i++)
        {
            // Crear una instancia del prefab del cuadrado
            GameObject cuadrado = Instantiate(cuadradoPrefab, contenedor);
            /*cuadrado.GetComponent<RectTransform>().sizeDelta = 
                new Vector2(skillBar.GetComponent<RectTransform>().sizeDelta.x, skillBar.GetComponent<RectTransform>().sizeDelta.y);*/
            GameObject vertical_bar = Instantiate(barPrefab, contenedor);
            cuadrado.name = "Skill" + i;

            // Obtener la referencia a la imagen del cuadrado
            Image[] images = cuadrado.GetComponentsInChildren<Image>();
            Image imagenHabilidad = images[0];
            imagenHabilidad.sprite = habilidades[i].imagen;
            imageSkill[i] = images[2];
            Debug.Log("nombre del objetivo = " + imageSkill[i].gameObject.name);
            coldownImages[i] = cuadrado.GetComponentsInChildren<Image>()[1];
            //imagen del coldown
            images[1].sprite = habilidades[i].imagen;
            cuadrado.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);

            // Obtener la referencia a la imagen del cuadrado usado para el coldown visual
            /*Image imagenColdown= images[1];
            imagenColdown.sprite = habilidades[i].imagen;*/

            Text[] textos = cuadrado.GetComponentsInChildren<Text>();
            // Obtener la referencia al texto de la tecla
            Text textoTecla = textos[0];
            AssignValuesOfKey(habilidades[i], textoTecla);
            //ANTES AQUI ESTABA LA SIGNACION DEL TEXTO DE TECLA

            //textoTecla.text = habilidades[i].tecla.ToString();

            // Obtener la referencia al texto del cooldown
            Text textoCooldown = textos[1];
            textoCooldown.text = habilidades[i].cooldown.ToString();
            coldowns[i] = textoCooldown;
        }
    }
    private void Update()
    {
        if (habilidades[0].container.GetComponent<BoomerangController>().onColdown &&
            !coldown1)
        {
            //Image[] images = GameObject.Find("Skill0").GetComponentsInChildren<Image>();
            //Debug.Log(coldownImages[0].gameObject.name);
            coldownImages[0].gameObject.SetActive(true);
            //expectedBoomerangColdownTime = GameObject.Find("Player").GetComponent<UseBoomerang>().expectedColdownTime;
            expectedBoomerangColdownTime = FindObjectOfType<UseBoomerang>().expectedColdownTime;
            remainingColdownTime = expectedBoomerangColdownTime;
            //Debug.Log("remaining coldown = " + remainingColdownTime);
            coldown1 = true;
            Debug.Log("BOMMERANG EN COLDOWN");
        }else if (habilidades[1].container.GetComponent<UseAttractThrowSkill>().onColdown)
        {
            Debug.Log(coldownImages[1].gameObject.name);
            coldownImages[1].gameObject.SetActive(true);
            Debug.Log("ATTRACT-TRHOW EN COLDOWN");
            coldown2 = true;
        }else if (habilidades[2].container.GetComponent<PsychicPunchController>().onColdown && !coldown3)
        {
            Debug.Log(coldownImages[2].gameObject.name);
            coldownImages[2].gameObject.SetActive(true);
            Debug.Log("PSYCHIC BALL EN COLDOWN");
            coldown3 = true;
        }
        else if (habilidades[3].container.GetComponent<DashController>().remainingTime > 0)
        {
            //ARREGLAR COLDOWN MOSTRADO DEL DASH (APLICAR ARRAY PARA VERIFICAR EL COLDOWN DE LA PRIMERA CARGA USADA, LUEGO LA SIGUIENTE...
            if (habilidades[3].container.GetComponent<DashController>().remainingTime > 0)
            {
                coldownImages[3].gameObject.SetActive(true);
                Debug.Log("DASH EN COLDOWN");
                float fillAmount = habilidades[3].container.GetComponent<DashController>().remainingTime /
                habilidades[3].container.GetComponent<DashController>().chargeRegenTime;
                //Debug.Log("fill amount  = " + fillAmount);
                coldownImages[3].fillAmount = fillAmount;
                coldowns[3].text = habilidades[3].container.GetComponent<DashController>().remainingTime.ToString("F1");
            }
            else
            {
                coldowns[3].text = habilidades[3].container.GetComponent<DashController>().chargeRegenTime.ToString("F0");
            }

        }
        else
        {

        }
        if (coldown1)
        {
            if (remainingColdownTime > 0 && habilidades[0].container.GetComponent<BoomerangController>().onColdown)
            {
                remainingColdownTime -= Time.deltaTime;
                float fillAmount = remainingColdownTime / expectedBoomerangColdownTime;
                //Debug.Log("fill amount = " + fillAmount);
                coldownImages[0].fillAmount = fillAmount;
                coldowns[0].text = remainingColdownTime.ToString("F1");
            }
            else
            {
                remainingColdownTime = 0f;
                //coldowns[0].text = habilidades[0].container.GetComponent<BoomerangController>().expectedColdownTime.ToString("F1");
                //coldowns[0].text = GameObject.Find("Player").GetComponent<UseBoomerang>().expectedColdownTime.ToString("F1");
                coldowns[0].text = FindObjectOfType<UseBoomerang>().expectedColdownTime.ToString("F1");
                coldownImages[0].fillAmount = 0f;
                coldown1 = false;
            }

        }
        if (coldown2)
        {
            float coldown = habilidades[1].container.GetComponent<UseAttractThrowSkill>().coldownTime;
            float remainingTime = habilidades[1].container.GetComponent<UseAttractThrowSkill>().remainingTime;
            if (remainingTime > 0)
            {
                float fillAmount = remainingTime / coldown;
                //Debug.Log("fill amount  = " + fillAmount);
                coldownImages[1].fillAmount = fillAmount;
                coldowns[1].text = remainingTime.ToString("F1");
            }
            else
            {
                coldownImages[1].fillAmount = 0f;
                coldowns[1].text = habilidades[1].container.GetComponent<UseAttractThrowSkill>().coldownTime.ToString("F0");
                coldown2 = false;
            }
        }
        if (coldown3)
        {
            float coldown = habilidades[2].container.GetComponent<PsychicPunchController>().coldown;
            float remainingTime = habilidades[2].container.GetComponent<PsychicPunchController>().remainingTime;
            if (remainingTime > 0)
            {
                float fillAmount = remainingTime / coldown;
                //Debug.Log("fill amount  = " + fillAmount);
                coldownImages[2].fillAmount = fillAmount;
                coldowns[2].text = remainingTime.ToString("F1");
            }
            else
            {
                coldownImages[2].fillAmount = 0f;
                coldowns[2].text = habilidades[1].container.GetComponent<PsychicPunchController>().coldown.ToString("F0");
                coldown3 = false;
            }

        }
    }
    private void FixedUpdate()
    {
        if (player.selectedMode == PlayerStats.GameMode.Boomerang)
        {
            Debug.Log("boomerang mode");
            imageSkill[0].gameObject.SetActive(true);
            imageSkill[1].gameObject.SetActive(false);
            imageSkill[2].gameObject.SetActive(false);

            imageSkill[3].gameObject.SetActive(false);
            //mostramos como activas las skills del boomerang escondiendo el hide
            //habilidades.
        }
        else if (player.selectedMode == PlayerStats.GameMode.AttractThrow)
        {
            Debug.Log("ATRAACT MODE");
            imageSkill[1].gameObject.SetActive(true);
            imageSkill[0].gameObject.SetActive(false);
            imageSkill[2].gameObject.SetActive(false);

            imageSkill[3].gameObject.SetActive(false);
        }
        else if (player.selectedMode == PlayerStats.GameMode.PyshicShot)
        {
            Debug.Log("PSYQUIC MODE");
            imageSkill[2].gameObject.SetActive(true);
            imageSkill[0].gameObject.SetActive(false);
            imageSkill[1].gameObject.SetActive(false);

            imageSkill[3].gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("nada de nada");
            imageSkill[0].gameObject.SetActive(false);
            imageSkill[1].gameObject.SetActive(false);
            imageSkill[2].gameObject.SetActive(false);

            imageSkill[3].gameObject.SetActive(false);
        }
    }
    private void AssignValuesOfKey(Habilidad habilidad, Text textoTecla)
    {
        if (habilidad.tecla.ToString() == "Alpha1")
        {
            textoTecla.text = "1";
        }
        else if (habilidad.tecla.ToString() == "Alpha2")
        {
            textoTecla.text = "2";
        }
        else if (habilidad.tecla.ToString() == "Alpha3")
        {
            textoTecla.text = "3";
        }
        else if (habilidad.tecla.ToString() == "E")
        {
            textoTecla.text = "E";
        }
        else if (habilidad.tecla.ToString() == "Q")
        {
            textoTecla.text = "Q";
        }
        else if (habilidad.tecla.ToString() == "Z")
        {
            textoTecla.text = "Z";
        }
        else if (habilidad.tecla.ToString() == "R")
        {
            textoTecla.text = "R";
        }
    }
}
