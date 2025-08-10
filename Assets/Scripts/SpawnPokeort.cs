using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPokeort : MonoBehaviour
{
    public GameObject[] PokeortsSpawneables;
    public Camera CamaraJugador;
    public float IntervaloDeSpawn = 5f;
    public BoxCollider areaDeSpawn;
    private int indiceAleatorio;
    public bool Activado;
    private Coroutine spawnCoroutine;
    public float DistanciaMaxima = 5f;

    void Start()
    {
        CamaraJugador = Camera.main;
        if (Activado)
        {
            spawnCoroutine = StartCoroutine(SpawnearPokeorts());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnearPokeorts()
    {
        while (Activado)
        {
            yield return new WaitForSeconds(IntervaloDeSpawn);

            if (VerificarDistancia() && !VerificarCamara())
            {
                indiceAleatorio = Random.Range(0, PokeortsSpawneables.Length);
                Vector3 spawnPosition = GetRandomPointInCollider();
                GameObject enemigoElegido = PokeortsSpawneables[indiceAleatorio];
                Instantiate(enemigoElegido, spawnPosition, Quaternion.identity);
            }
        }
    }
    Vector3 GetRandomPointInCollider()
    {
        Bounds bordes = areaDeSpawn.bounds;
        float randomX = Random.Range(bordes.min.x, bordes.max.x);
        float randomZ = Random.Range(bordes.min.z, bordes.max.z);
        return new Vector3(randomX, bordes.center.y, randomZ);
    }
    private bool VerificarDistancia()
    {
        float distancia = Vector3.Distance(CamaraJugador.transform.position, areaDeSpawn.transform.position);
        Debug.Log(distancia);
        if (distancia < DistanciaMaxima)
        {
            Debug.Log("Dentro De Area De Spawn");
            return true; 
        }
        Debug.Log("Afuera De Area De Spawn");
        return false; 
    }
    private bool VerificarCamara()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(CamaraJugador);
        Debug.Log(GeometryUtility.TestPlanesAABB(planes, areaDeSpawn.bounds));
        return GeometryUtility.TestPlanesAABB(planes, areaDeSpawn.bounds);
    }
}