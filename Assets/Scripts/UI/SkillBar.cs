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

    // Referencia al contenedor de las habilidades
    public Transform contenedor;
    private Text [] coldowns;
    private Image[] coldownImages;
    private Image[] imageSkill;
    private PlayerStats player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
        coldownImages = new Image[habilidades.Length];
        coldowns = new Text[habilidades.Length];
        imageSkill = new Image[habilidades.Length];
        // Crear los cuadrados de habilidad dinámicamente
        for (int i = 0; i < habilidades.Length; i++)
        {
            // Crear una instancia del prefab del cuadrado
            GameObject cuadrado = Instantiate(cuadradoPrefab, contenedor);
            cuadrado.name = "Skill" + i;

            
            
            // Obtener la referencia a la imagen del cuadrado
            Image[] images = cuadrado.GetComponentsInChildren<Image>();
            Image imagenHabilidad = images[0];
            imagenHabilidad.sprite = habilidades[i].imagen;
            imageSkill[i] = images[2];
            Debug.Log("nombre del objetivo = " + imageSkill[i].gameObject.name);
            coldownImages[i] = cuadrado.GetComponentsInChildren<Image>()[1];
            cuadrado.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);

            // Obtener la referencia a la imagen del cuadrado usado para el coldown visual
            /*Image imagenColdown= images[1];
            imagenColdown.sprite = habilidades[i].imagen;*/

            Text[] textos = cuadrado.GetComponentsInChildren<Text>();
            // Obtener la referencia al texto de la tecla
            Text textoTecla = textos[0];
            if (habilidades[i].tecla.ToString() == "Alpha1")
            {
                textoTecla.text = "1";
            }
            else if (habilidades[i].tecla.ToString() == "Alpha2")
            {
                textoTecla.text = "2";
            }
            else if (habilidades[i].tecla.ToString() == "Alpha3")
            {
                textoTecla.text = "3";
            }
            //textoTecla.text = habilidades[i].tecla.ToString();

            // Obtener la referencia al texto del cooldown
            Text textoCooldown = textos[1];
            textoCooldown.text = habilidades[i].cooldown.ToString();
            coldowns[i] = textoCooldown;
        }
    }
    private void Update()
    {
        if (habilidades[0].container.GetComponent<BoomerangController>().onColdown && player.selectedMode == PlayerStats.GameMode.Boomerang)
        {
            //Image[] images = GameObject.Find("Skill0").GetComponentsInChildren<Image>();
            Debug.Log(coldownImages[0].gameObject.name);
            coldownImages[0].gameObject.SetActive(true);
            Debug.Log("BOMMERANG EN COLDOWN");
        }else if (habilidades[1].container.GetComponent<UseAttractThrowSkill>().onColdown)
        {
            Debug.Log(coldownImages[1].gameObject.name);
            coldownImages[1].gameObject.SetActive(true);
            Debug.Log("ATTRACT-TRHOW EN COLDOWN");
        }/*else if (habilidades[1].container.onColdown)
        {

        }*/else
        {
            coldownImages[0].gameObject.SetActive(false);
            coldownImages[1].gameObject.SetActive(false);
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
        }
        else if (player.selectedMode == PlayerStats.GameMode.AttractThrow)
        {
            Debug.Log("ATRAACT MODE");
            imageSkill[1].gameObject.SetActive(true);
            imageSkill[0].gameObject.SetActive(false);
            imageSkill[2].gameObject.SetActive(false);
        }
        else if (player.selectedMode == PlayerStats.GameMode.PyshicShot)
        {
            Debug.Log("PSYQUIC MODE");
            imageSkill[2].gameObject.SetActive(true);
            imageSkill[0].gameObject.SetActive(false);
            imageSkill[1].gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("nada de nada");
            imageSkill[0].gameObject.SetActive(false);
            imageSkill[1].gameObject.SetActive(false);
            imageSkill[2].gameObject.SetActive(false);
        }
    }
}
