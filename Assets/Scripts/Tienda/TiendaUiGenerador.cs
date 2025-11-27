using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class TiendaUiGenerador : MonoBehaviour
{
    private class CarritoItem
    {
        public Item itemReferencia;
        public int cantidad = 1;
    }

    [Header("Referencias UI de la Tienda")]
    public Transform contenedorTienda;
    public TMP_FontAsset fuenteTMP;
    public Item[] itemsDisponibles;
    public TMP_Text textoDinero;
    public Button InteractuarVendedor;
    public static TiendaUiGenerador instance;

    [Header("Referencias UI del Carrito")]
    public GameObject panelCarrito;
    public Button botonConfirmar;
    public Button botonCancelar;
    public Transform contenedorItemsCarrito;
    public GameObject prefabItemCarrito;
    public TMP_Text textoTotalCompra;
    public Image fondoOscuro; 

    private List<CarritoItem> itemsEnCarrito = new List<CarritoItem>();
    private int totalCompraTemporal = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        if (panelCarrito != null)
            panelCarrito.SetActive(false);

        if (fondoOscuro != null)
        {
            fondoOscuro.gameObject.SetActive(false);
            CanvasGroup cgFondo = fondoOscuro.GetComponent<CanvasGroup>();
            if (cgFondo == null) cgFondo = fondoOscuro.gameObject.AddComponent<CanvasGroup>();
            cgFondo.alpha = 0f;
        }
    }

    void Start()
    {
        CrearBotones();
        ActualizarDinero();

        if (botonConfirmar != null)
            botonConfirmar.onClick.AddListener(ConfirmarCompra);

        if (botonCancelar != null)
            botonCancelar.onClick.AddListener(CancelarCompra);
    }

    // ========================
    // CREACIÓN DE LOS BOTONES
    // ========================
    void CrearBotones()
    {
        foreach (Item item in itemsDisponibles)
        {
            GameObject panel = new GameObject(item.itemName, typeof(RectTransform));
            panel.transform.SetParent(contenedorTienda, false);

            Image fondo = panel.AddComponent<Image>();
            fondo.color = new Color(0.95f, 0.95f, 1f, 1f);

            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(250, 100);

            Outline outline = panel.AddComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.25f);
            outline.effectDistance = new Vector2(1, -1);

            // Icono
            GameObject imagenObj = new GameObject("Icono", typeof(Image));
            imagenObj.transform.SetParent(panel.transform, false);
            Image img = imagenObj.GetComponent<Image>();
            img.sprite = item.icon;
            RectTransform rectImg = img.GetComponent<RectTransform>();
            rectImg.anchorMin = new Vector2(0, 0);
            rectImg.anchorMax = new Vector2(0, 1);
            rectImg.offsetMin = new Vector2(10, 10);
            rectImg.offsetMax = new Vector2(90, -10);

            // Nombre
            GameObject nombreObj = new GameObject("Nombre", typeof(TextMeshProUGUI));
            nombreObj.transform.SetParent(panel.transform, false);
            TMP_Text txtNombre = nombreObj.GetComponent<TMP_Text>();
            txtNombre.text = item.itemName;
            txtNombre.font = fuenteTMP;
            txtNombre.fontSize = 20;
            txtNombre.alignment = TextAlignmentOptions.TopLeft;
            txtNombre.color = Color.black;
            RectTransform rectNombre = nombreObj.GetComponent<RectTransform>();
            rectNombre.anchorMin = new Vector2(0.4f, 0.5f);
            rectNombre.anchorMax = new Vector2(1, 1);
            rectNombre.offsetMin = new Vector2(0, -20);
            rectNombre.offsetMax = new Vector2(-10, -5);

            // Precio
            GameObject precioObj = new GameObject("Precio", typeof(TextMeshProUGUI));
            precioObj.transform.SetParent(panel.transform, false);
            TMP_Text txtPrecio = precioObj.GetComponent<TMP_Text>();
            txtPrecio.text = "$" + item.valor;
            txtPrecio.font = fuenteTMP;
            txtPrecio.fontSize = 18;
            txtPrecio.alignment = TextAlignmentOptions.BottomLeft;
            txtPrecio.color = Color.black;
            RectTransform rectPrecio = precioObj.GetComponent<RectTransform>();
            rectPrecio.anchorMin = new Vector2(0.4f, 0);
            rectPrecio.anchorMax = new Vector2(1, 0.5f);
            rectPrecio.offsetMin = new Vector2(0, 10);
            rectPrecio.offsetMax = new Vector2(-10, 25);

            // Botón Añadir
            GameObject botonObj = new GameObject("BotonAnadir", typeof(Button), typeof(Image));
            botonObj.transform.SetParent(panel.transform, false);
            Image botonImg = botonObj.GetComponent<Image>();
            botonImg.color = new Color(0.3f, 0.7f, 0.3f, 1f);
            Button boton = botonObj.GetComponent<Button>();
            RectTransform rectBoton = botonObj.GetComponent<RectTransform>();
            rectBoton.anchorMin = new Vector2(0.65f, 0.1f);
            rectBoton.anchorMax = new Vector2(0.95f, 0.4f);
            rectBoton.offsetMin = Vector2.zero;
            rectBoton.offsetMax = Vector2.zero;

            // Texto botón
            GameObject textoBoton = new GameObject("Texto", typeof(TextMeshProUGUI));
            textoBoton.transform.SetParent(botonObj.transform, false);
            TMP_Text txtBoton = textoBoton.GetComponent<TMP_Text>();
            txtBoton.text = "Añadir";
            txtBoton.font = fuenteTMP;
            txtBoton.fontSize = 18;
            txtBoton.alignment = TextAlignmentOptions.Center;
            txtBoton.color = Color.black;
            RectTransform rectTxtBoton = textoBoton.GetComponent<RectTransform>();
            rectTxtBoton.anchorMin = Vector2.zero;
            rectTxtBoton.anchorMax = Vector2.one;
            rectTxtBoton.offsetMin = Vector2.zero;
            rectTxtBoton.offsetMax = Vector2.zero;

            boton.onClick.AddListener(() => AnadirAlCarrito(item));
        }
    }

    // ========================
    // FUNCIONALIDAD DE CARRITO
    // ========================
    void AnadirAlCarrito(Item item)
    {
        CarritoItem carritoItem = new CarritoItem { itemReferencia = item, cantidad = 1 };
        itemsEnCarrito.Add(carritoItem);
        totalCompraTemporal += item.valor;

        // Feedback visual: rebote leve
        Transform panel = contenedorTienda.Find(item.itemName);
        if (panel != null)
        {
            StartCoroutine(UIAnimaciones.EscalarSuave(panel, Vector3.one * 1.1f, Vector3.one, 0.2f));
        }

        if (panelCarrito != null)
            panelCarrito.SetActive(true);

        ActualizarCarritoUI();
    }

    void ActualizarCarritoUI()
    {
        if (contenedorItemsCarrito == null || prefabItemCarrito == null) return;

        foreach (Transform child in contenedorItemsCarrito)
            Destroy(child.gameObject);

        Dictionary<Item, int> resumen = new Dictionary<Item, int>();
        foreach (var item in itemsEnCarrito)
        {
            if (resumen.ContainsKey(item.itemReferencia))
                resumen[item.itemReferencia]++;
            else
                resumen.Add(item.itemReferencia, 1);
        }

        foreach (var par in resumen)
        {
            GameObject itemUI = Instantiate(prefabItemCarrito, contenedorItemsCarrito);
            TMP_Text nombreTxt = itemUI.transform.Find("TextoNombre")?.GetComponent<TMP_Text>();
            TMP_Text cantidadTxt = itemUI.transform.Find("TextoCantidad")?.GetComponent<TMP_Text>();

            if (nombreTxt != null) nombreTxt.text = par.Key.itemName;
            int precioTotalItem = par.Key.valor * par.Value;
            if (cantidadTxt != null) cantidadTxt.text = $"x{par.Value} (${precioTotalItem})";

            // Pequeña animación de aparición
            itemUI.transform.localScale = new Vector3(0f, 0f, 0f);
            StartCoroutine(UIAnimaciones.EscalarSuave(
                itemUI.transform,
                new Vector3(0f, 0f, 0f),
                new Vector3(1f, 0.5f, 1f),
                0.25f
            ));

        }

        if (textoTotalCompra != null)
            textoTotalCompra.text = $"TOTAL: ${totalCompraTemporal}";
    }

    // ========================
    // BOTONES DE CONFIRMAR Y CANCELAR
    // ========================
    public void ConfirmarCompra()
    {
        if (itemsEnCarrito.Count == 0) return;

        if (PlataManager.instance.PlataJugador >= totalCompraTemporal)
        {
            PlataManager.instance.QuitarDinero(totalCompraTemporal);

            foreach (var carritoItem in itemsEnCarrito)
                if (Inventario.instance != null)
                    Inventario.instance.AñadirItem(carritoItem.itemReferencia, 1);

            ActualizarDinero();
            VaciarCarrito();
        }
        else
        {
            Debug.Log("No tienes suficiente dinero para completar la compra.");
        }
    }

    public void CancelarCompra()
    {
        VaciarCarrito();
    }

    void VaciarCarrito()
    {
        itemsEnCarrito.Clear();
        totalCompraTemporal = 0;
        if (panelCarrito != null)
            panelCarrito.SetActive(false);
        ActualizarCarritoUI();
    }

    // ========================
    // ACTUALIZAR DINERO
    // ========================
    void ActualizarDinero()
    {
        if (textoDinero != null)
        {
            textoDinero.text = "Dinero: $" + PlataManager.instance.PlataJugador;
            StartCoroutine(UIAnimaciones.EscalarSuave(textoDinero.transform, Vector3.one * 1.2f, Vector3.one, 0.3f));
        }
    }

    // ========================
    // MOSTRAR / OCULTAR TIENDA
    // ========================
    public void MostrarCanvas()
    {
        GameObject canvasObj = contenedorTienda.gameObject.transform.parent.gameObject;
        canvasObj.SetActive(true);
        ControlCursor.instance.MostrarCursor();
        InteractuarVendedor.gameObject.SetActive(false);

        CanvasGroup cg = canvasObj.GetComponent<CanvasGroup>();
        if (cg == null) cg = canvasObj.AddComponent<CanvasGroup>();
        cg.alpha = 0f;

        contenedorTienda.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        StartCoroutine(UIAnimaciones.CambiarAlpha(cg, 1f, 0.4f));
        StartCoroutine(UIAnimaciones.EscalarSuave(contenedorTienda, contenedorTienda.localScale, new Vector3(1f, 1.3f, 1f), 0.4f));

        if (fondoOscuro != null)
        {
            fondoOscuro.gameObject.SetActive(true);
            CanvasGroup cgFondo = fondoOscuro.GetComponent<CanvasGroup>();
            StartCoroutine(UIAnimaciones.CambiarAlpha(cgFondo, 0.6f, 0.3f));
        }
    }

    public void EsconderCanvas()
    {
        GameObject canvasObj = contenedorTienda.gameObject.transform.parent.gameObject;
        CanvasGroup cg = canvasObj.GetComponent<CanvasGroup>();
        if (cg == null) cg = canvasObj.AddComponent<CanvasGroup>();
        ControlCursor.instance.BloquearCursor();
        StartCoroutine(CerrarTiendaSuave(canvasObj, cg));
        
    }

    private IEnumerator CerrarTiendaSuave(GameObject canvasObj, CanvasGroup cg)
    {
        yield return StartCoroutine(UIAnimaciones.CambiarAlpha(cg, 0f, 0.25f));
        canvasObj.SetActive(false);
        InteractuarVendedor.gameObject.SetActive(true);
        CancelarCompra();

        if (fondoOscuro != null)
        {
            CanvasGroup cgFondo = fondoOscuro.GetComponent<CanvasGroup>();
            StartCoroutine(UIAnimaciones.CambiarAlpha(cgFondo, 0f, 0.25f));
            yield return new WaitForSeconds(0.25f);
            fondoOscuro.gameObject.SetActive(false);
        }

        GameObject.FindGameObjectWithTag("Player").GetComponent<MovimientoJugador>().enabled = true;
    }
}
