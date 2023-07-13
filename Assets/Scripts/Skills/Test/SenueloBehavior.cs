using UnityEngine;
using UnityEngine.AI;

public class SenueloBehavior : MonoBehaviour
{
    private NavMeshAgent agent;
    private float vida;
    private float duracion;
    public float distanciaMinima = 2f;
    public float distanciaMaxima = 5f;
    public float tiempoEspera = 3f;
    private bool llegoAPunto;
    private Vector3 puntoDestino;
    private Vector3 lastPosition;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        llegoAPunto = true;
    }

    void Update()
    {
        duracion -= Time.deltaTime;

        if (duracion <= 0f)
        {
            Destroy(gameObject);
        }

        if (llegoAPunto)
        {
            LlegoAPunto();
        }
        if (verifyIfArrivedToPoint())
        {
            llegoAPunto = true;
            //LlegoAPunto();
        }
    }

    public void AsignarVida(float vida)
    {
        this.vida = vida;
    }

    public void AsignarDuracion(float duracion)
    {
        this.duracion = duracion;
    }

    void LlegoAPunto()
    {
        float espera = Random.Range(tiempoEspera, tiempoEspera * 2f);
        Invoke("MoverAPunto", espera);
        llegoAPunto = false;
    }

    void MoverAPunto()
    {
        puntoDestino = GenerarPuntoDestino();
        agent.SetDestination(puntoDestino);
    }

    Vector3 GenerarPuntoDestino()
    {
        Vector3 randomDirection = Random.insideUnitSphere * distanciaMaxima;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, distanciaMaxima, NavMesh.AllAreas);

        lastPosition = hit.position;
        return hit.position;
    }
    //verificar y acabar la implementacion
    bool verifyIfArrivedToPoint()
    {
        if (agent.transform.position == lastPosition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // Implementa lógica adicional según tus necesidades, como recibir daño de los enemigos, etc.
}
