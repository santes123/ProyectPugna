using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public GameObject[] buffPrefabs;  // Array de prefabs de los efectos de los buffos
    private GameObject currentBuffEffect;  // Referencia al efecto actual del buffo

    // Método para aplicar un buffo al jugador
    public void ApplyBuff(PowerupType buffType, float buffDuration)
    {
        // Si ya hay un efecto de buffo en juego, lo destruimos
        if (currentBuffEffect != null)
        {
            Destroy(currentBuffEffect);
        }

        // Instanciamos el nuevo efecto de buffo
        int positionOnArray = AssignPrefab(buffType);
        currentBuffEffect = Instantiate(buffPrefabs[positionOnArray], transform.position, buffPrefabs[positionOnArray].transform.rotation, transform);
        //currentBuffEffect.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        // Iniciamos una corrutina para destruir el efecto cuando finalice el buffo
        StartCoroutine(DestroyBuffEffect(buffDuration));
    }

    // Corrutina para destruir el efecto de buffo después de un tiempo determinado
    private System.Collections.IEnumerator DestroyBuffEffect(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Destruimos el efecto de buffo
        Destroy(currentBuffEffect);
    }
    //metodo para asginar un elemento u otro dependiendo del PowerupType
    int AssignPrefab(PowerupType powerupType)
    {
        switch (powerupType)
        {
            case PowerupType.Damage:
                return 0;
                //break;
            case PowerupType.Speed:
                return 1;
                //break;
            case PowerupType.Invincibility:
                return 2;
                //break;
            default:
                return -1;
                //break;
        }
    }
}
