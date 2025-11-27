using UnityEngine;
using System.Collections;

public class CamaraOrbiter : MonoBehaviour
{
    public float tiempoDeVuelta = 5.0f;
    public Transform target;
    public Transform target2;

    // Distancia de la cámara al objetivo
    public float distance = 5.0f;

    // Almacena la rotación de la órbita en ángulos de Euler
    public float x = 40.0f;
    public float y = 10.0f;

    private Camera CameraJugador;
    private bool inicializado = false;

    void Update()
    {
        // ⭐ DEBUG: Verificar constantemente si aparece el NPC
        if (!inicializado)
        {
            GameObject npc = GameObject.FindGameObjectWithTag(PlayerPrefs.GetString("EncounteredPokemon"));
            Debug.Log($"🔍 UPDATE - NPC encontrado: {npc != null} (Frame: {Time.frameCount})");
            if (npc != null)
            {
                Debug.Log($"🔍 UPDATE - NPC: '{npc.name}' en posición: {npc.transform.position}");
            }
        }
    }

    void Start()
    {
        Debug.Log($"🔍 === START CAMARA ORBITER (Frame: {Time.frameCount}) ===");

        // ⭐ NUEVO: Usar corrutina con delay de 1 frame
        StartCoroutine(InicializarConDelay());
    }

    IEnumerator InicializarConDelay()
    {
        // ⭐ Esperar 1 frame para que todo se instancie
        yield return null;

        Debug.Log($"🔍 === INICIALIZAR CON DELAY (Frame: {Time.frameCount}) ===");

        string encounterValue = PlayerPrefs.GetString("EncounteredPokemon");
        Debug.Log($"🔍 Buscando NPC: '{encounterValue}'");
        Debug.Log($"🔍 CombateNPCManager existe: {CombateNPCManager.instance != null}");

        // ⭐ DEBUG EXHAUSTIVO: Listar TODOS los GameObjects
        GameObject[] todosLosObjetos = FindObjectsOfType<GameObject>();
        Debug.Log($"🔍 Total GameObjects en escena: {todosLosObjetos.Length}");

        // Contar y mostrar objetos relacionados con líder
        int contadorLideres = 0;
        int contadorTodosLosNPCs = 0;

        foreach (GameObject obj in todosLosObjetos)
        {
            // Contar todos los NPCs
            if (obj.name.ToLower().Contains("npc") || obj.tag.ToLower().Contains("npc"))
            {
                contadorTodosLosNPCs++;
                Debug.Log($"🔍 NPC #{contadorTodosLosNPCs}: '{obj.name}' (tag: '{obj.tag}') - Activo: {obj.activeInHierarchy}");
            }

            // Objetos relacionados con líder específicamente
            if (obj.name.ToLower().Contains("lider") ||
                obj.tag.ToLower().Contains("lider") ||
                obj.name == encounterValue ||
                obj.tag == encounterValue)
            {
                contadorLideres++;
                Debug.Log($"🔍 === LÍDER #{contadorLideres} ===");
                Debug.Log($"   - Nombre: '{obj.name}'");
                Debug.Log($"   - Tag: '{obj.tag}'");
                Debug.Log($"   - Activo en jerarquía: {obj.activeInHierarchy}");
                Debug.Log($"   - Activo en sí mismo: {obj.activeSelf}");
                Debug.Log($"   - Posición: {obj.transform.position}");
                Debug.Log($"   - Padre: {(obj.transform.parent != null ? obj.transform.parent.name : "null")}");
                Debug.Log($"   - Nombre == encounterValue: {obj.name == encounterValue}");
                Debug.Log($"   - Tag == encounterValue: {obj.tag == encounterValue}");

                // Verificar componentes específicos
                if (obj.GetComponent<PokedexManagerNPC>() != null)
                    Debug.Log($"   - ✅ Tiene PokedexManagerNPC");
                if (obj.GetComponent<LiderScript>() != null)
                    Debug.Log($"   - ✅ Tiene LiderScript");
                if (obj.GetComponent<CombatNPCInteraction>() != null)
                    Debug.Log($"   - ✅ Tiene CombatNPCInteraction");
            }
        }

        Debug.Log($"🔍 Total NPCs en escena: {contadorTodosLosNPCs}");
        Debug.Log($"🔍 Objetos relacionados con líder: {contadorLideres}");

        // ⭐ BÚSQUEDA MÚLTIPLE CON LOGS DETALLADOS
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject npc = null;

        Debug.Log($"🔍 Player encontrado: {player != null}");
        if (player != null)
        {
            Debug.Log($"🔍 Player: '{player.name}' en posición: {player.transform.position}");
        }

        // 1. Búsqueda por tag
        Debug.Log($"🔍 1. Buscando por tag: '{encounterValue}'");
        npc = GameObject.FindGameObjectWithTag(encounterValue);
        if (npc != null)
        {
            Debug.Log($"✅ 1. ENCONTRADO por tag: '{npc.name}' en {npc.transform.position}");
        }
        else
        {
            Debug.Log($"❌ 1. NO encontrado por tag");

            // 2. Búsqueda por nombre
            Debug.Log($"🔍 2. Buscando por nombre: '{encounterValue}'");
            npc = GameObject.Find(encounterValue);
            if (npc != null)
            {
                Debug.Log($"✅ 2. ENCONTRADO por nombre: '{npc.name}' en {npc.transform.position}");
            }
            else
            {
                Debug.Log($"❌ 2. NO encontrado por nombre");

                // 3. Búsqueda por nombre + "(Clone)"
                string cloneName = encounterValue + "(Clone)";
                Debug.Log($"🔍 3. Buscando por nombre con Clone: '{cloneName}'");
                npc = GameObject.Find(cloneName);
                if (npc != null)
                {
                    Debug.Log($"✅ 3. ENCONTRADO por nombre + Clone: '{npc.name}' en {npc.transform.position}");
                }
                else
                {
                    Debug.Log($"❌ 3. NO encontrado por nombre + Clone");

                    // 4. Búsqueda manual en lista
                    Debug.Log($"🔍 4. Búsqueda manual en objetos encontrados");
                    foreach (GameObject obj in todosLosObjetos)
                    {
                        string objClean = obj.name.Replace("(Clone)", "").Trim();
                        string encounterClean = encounterValue.Trim();

                        if (string.Equals(objClean, encounterClean, System.StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(obj.tag, encounterValue, System.StringComparison.OrdinalIgnoreCase))
                        {
                            npc = obj;
                            Debug.Log($"✅ 4. ENCONTRADO por comparación: '{npc.name}' (tag: '{npc.tag}')");
                            break;
                        }
                    }

                    if (npc == null)
                    {
                        Debug.Log($"❌ 4. NO encontrado en búsqueda manual");

                        // 5. FALLBACK: Usar el primer líder si existe
                        if (contadorLideres > 0)
                        {
                            Debug.Log($"🔍 5. FALLBACK: Buscando cualquier líder");
                            foreach (GameObject obj in todosLosObjetos)
                            {
                                if (obj.name.ToLower().Contains("lider") || obj.tag.ToLower().Contains("lider"))
                                {
                                    npc = obj;
                                    Debug.Log($"⚠️ 5. USANDO FALLBACK: '{npc.name}' (tag: '{npc.tag}')");
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError($"❌ 5. NO hay líderes disponibles para fallback");
                        }
                    }
                }
            }
        }

        // ⭐ ASIGNACIÓN DE TARGETS
        if (player != null)
        {
            target = player.transform;
            Debug.Log($"✅ Target (player) asignado: {target.name}");
        }

        if (npc != null)
        {
            target2 = npc.transform;
            Debug.Log($"✅ Target2 (npc) asignado: {target2.name}");
        }

        CameraJugador = player?.GetComponentInChildren<Camera>();

        if (target == null || target2 == null)
        {
            Debug.LogError($"❌ CamaraOrbiter: Faltan referencias.");
            Debug.LogError($"   Target (player): {target != null}");
            Debug.LogError($"   Target2 (npc): {target2 != null}");

            // ⭐ DEBUG FINAL: Estado de CombateNPCManager
            if (CombateNPCManager.instance != null)
            {
                Debug.LogError($"🔍 CombateNPCManager existe, pero no se pudo encontrar el NPC");
            }
            else
            {
                Debug.LogError($"🔍 CombateNPCManager NO existe");
            }

            inicializado = false;
        }

        // ⭐ CONFIGURACIÓN FINAL
        if (player != null)
        {
            if (npc.tag != "Lider" && npc.tag != "NPCGym")
            {
                float playerRotY = PlayerPrefs.GetFloat("RotY", 0f);
                x = x + playerRotY;
                Debug.Log($"🔍 Rotación inicial del jugador: {playerRotY}°, ángulo X cámara: {x}°");
            }
        }

        inicializado = true;
        Debug.Log($"✅ === CÁMARA CONFIGURADA EXITOSAMENTE ===");

        LateUpdate();
        DarVuelta();
    }

    void LateUpdate()
    {
        if (target == null || target2 == null) return;

        // Calcular la rotación y posición de la órbita
        Vector3 centerPoint = (target.position + target2.position) / 2.0f;
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        Vector3 position = rotation * new Vector3(0.0f, 2.0f, -(distance + 4f)) + centerPoint;

        // Actualizar la transformación de la cámara
        transform.rotation = rotation;
        transform.position = position;
    }

    public void SetOrbitAngles(float newX, float newY)
    {
        x = newX;
        y = newY;
    }

    public void AdjustOrbitAngles(float deltaX, float deltaY)
    {
        x += deltaX;
        y += deltaY;
    }

    private void DarVuelta()
    {
        StartCoroutine(DarVueltaCoroutine(tiempoDeVuelta));
    }

    private IEnumerator DarVueltaCoroutine(float duration)
    {
        float initialX = x;
        Debug.Log($"DarVuelta - Ángulo inicial: {initialX}°");
        float finalX = initialX + 180.0f;
        float timer = 0.0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);
            x = Mathf.Lerp(initialX, finalX, progress);

            yield return null;
        }

        x = finalX;
        Debug.Log($"DarVuelta - Ángulo final: {x}°");
    }
}