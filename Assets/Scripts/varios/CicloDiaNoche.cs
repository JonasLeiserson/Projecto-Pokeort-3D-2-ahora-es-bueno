using UnityEngine;

public class CicloDiaNoche : MonoBehaviour
{
    public float velocidadDeTiempo = 1f;
    public float intensidadMediodia = 1f;
    public float intensidadMedianoche = 0.1f;
    private Light luzDireccional;
    private float tiempo = 5f;

    void Start()
    {
        luzDireccional = GetComponent<Light>();
    }

    void Update()
    {
        tiempo += Time.deltaTime * velocidadDeTiempo;
        float anguloDeRotacion = tiempo * 15f;
        transform.rotation = Quaternion.Euler(anguloDeRotacion, 0, 0);
        float factorDeIntensidad = Mathf.Sin(anguloDeRotacion * Mathf.Deg2Rad);
        luzDireccional.intensity = Mathf.Lerp(intensidadMedianoche, intensidadMediodia, factorDeIntensidad);
    }
}