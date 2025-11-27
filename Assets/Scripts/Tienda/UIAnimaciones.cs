using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIAnimaciones : MonoBehaviour
{
    public static IEnumerator EscalarSuave(Transform obj, Vector3 inicio, Vector3 fin, float duracion)
    {
        float t = 0;
        obj.localScale = inicio;
        while (t < duracion)
        {
            t += Time.deltaTime;
            obj.localScale = Vector3.Lerp(inicio, fin, Mathf.SmoothStep(0, 1, t / duracion));
            yield return null;
        }
        obj.localScale = fin;
    }

    public static IEnumerator CambiarAlpha(CanvasGroup cg, float alphaFinal, float duracion)
    {
        float alphaInicial = cg.alpha;
        float t = 0;
        while (t < duracion)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(alphaInicial, alphaFinal, Mathf.SmoothStep(0, 1, t / duracion));
            yield return null;
        }
        cg.alpha = alphaFinal;
    }
}
