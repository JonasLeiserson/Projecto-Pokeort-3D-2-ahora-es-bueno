using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TiendaUiGenerador : MonoBehaviour
{
    private class CarritoItem
    {
        public Item itemReferencia;
        public int cantidad = 1;
    }

    [Header("Referencias UI de la Tienda")]
    public Transform contenedorTienda; // El padre donde se generan los ítems de la tienda
    public TMP_FontAsset fuenteTMP;
    public Item[] itemsDisponibles;
    public TMP_Text textoDinero;
    public Button InteractuarVendedor;
    public static TiendaUiGenerador instance;

    [Header("Referencias UI del Carrito")]
    public GameObject panelCarrito; // El panel principal del carrito (ej: el área azul)
    public Button botonConfirmar;
    public Button botonCancelar;
    public Transform contenedorItemsCarrito; // El padre DENTRO del panelCarrito donde irá la lista
    public GameObject prefabItemCarrito; // Prefab de UI para cada ítem del carrito
    public TMP_Text textoTotalCompra; // Texto para mostrar el total

    private List<CarritoItem> itemsEnCarrito = new List<CarritoItem>();
    private int totalCompraTemporal = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // Asumimos que el Panel Principal del Canvas ya está desactivado al inicio
        // y que panelCarrito es el área azul donde aparece la lista
        if (panelCarrito != null)
        {
            panelCarrito.SetActive(false);
        }
    }

    void Start()
    {
        CrearBotones();
        ActualizarDinero();

        // Enlazar los métodos a los botones de Confirmar/Cancelar que ya existen en la escena
        if (botonConfirmar != null)
        {
            botonConfirmar.onClick.AddListener(ConfirmarCompra);
        }
        if (botonCancelar != null)
        {
            botonCancelar.onClick.AddListener(CancelarCompra);
        }
    }

    void CrearBotones()
    {
        foreach (Item item in itemsDisponibles)
        {
            // --- Panel del Ítem (Menos tosco) ---
            GameObject panel = new GameObject(item.itemName, typeof(RectTransform));
            panel.transform.SetParent(contenedorTienda, false);

            Image fondo = panel.AddComponent<Image>();
            fondo.color = new Color(0.9f, 0.9f, 0.95f, 1f);

            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(250, 100);

            // Efecto de borde/sombra
            Outline outline = panel.AddComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.3f);
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

            // Botón Añadir al Carrito (El botón 'Añadir' verde en tu UI)
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

            // Texto del botón
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

    void AnadirAlCarrito(Item item)
    {
        CarritoItem carritoItem = new CarritoItem { itemReferencia = item, cantidad = 1 };
        itemsEnCarrito.Add(carritoItem);
        totalCompraTemporal += item.valor;

        // Activa el panel azul central (panelCarrito) y el total.
        if (panelCarrito != null)
        {
            panelCarrito.SetActive(true);
        }

        ActualizarCarritoUI();
    }

    void ActualizarCarritoUI()
    {
        if (contenedorItemsCarrito == null || prefabItemCarrito == null) return;

        // Limpiar lista anterior
        foreach (Transform child in contenedorItemsCarrito)
        {
            Destroy(child.gameObject);
        }

        // Resumen de ítems (agrupar)
        Dictionary<Item, int> resumen = new Dictionary<Item, int>();
        foreach (var item in itemsEnCarrito)
        {
            if (resumen.ContainsKey(item.itemReferencia))
            {
                resumen[item.itemReferencia]++;
            }
            else
            {
                resumen.Add(item.itemReferencia, 1);
            }
        }

        // Generar UI para cada ítem agrupado
        foreach (var par in resumen)
        {
            GameObject itemUI = Instantiate(prefabItemCarrito, contenedorItemsCarrito);

            // Asumiendo que el prefab tiene Icono, TextoNombre y TextoCantidad
            TMP_Text nombreTxt = itemUI.transform.Find("TextoNombre")?.GetComponent<TMP_Text>();
            TMP_Text cantidadTxt = itemUI.transform.Find("TextoCantidad")?.GetComponent<TMP_Text>();

            if (nombreTxt != null) nombreTxt.text = par.Key.itemName;

            int precioTotalItem = par.Key.valor * par.Value;
            if (cantidadTxt != null) cantidadTxt.text = $"x{par.Value} (${precioTotalItem})";
        }

        // Actualizar Total
        if (textoTotalCompra != null)
        {
            textoTotalCompra.text = $"TOTAL: ${totalCompraTemporal}";
        }
    }

    public void ConfirmarCompra()
    {
        if (itemsEnCarrito.Count == 0) return;

        if (PlataManager.instance.PlataJugador >= totalCompraTemporal)
        {
            PlataManager.instance.QuitarDinero(totalCompraTemporal);

            foreach (var carritoItem in itemsEnCarrito)
            {
                if (Inventario.instance != null)
                {
                    Inventario.instance.AñadirItem(carritoItem.itemReferencia, 1);
                }
            }

            ActualizarDinero();

            VaciarCarrito();
        }
        else
        {
            Debug.Log("No tienes suficiente dinero para completar la compra de $" + totalCompraTemporal);
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
        {
            panelCarrito.SetActive(false);
        }
        ActualizarCarritoUI();
    }

    void ActualizarDinero()
    {
        if (textoDinero != null)
            textoDinero.text = "Dinero: $" + PlataManager.instance.PlataJugador;
    }

    public void MostrarCanvas()
    {
        contenedorTienda.gameObject.transform.parent.gameObject.SetActive(true); // Activa el Canvas completo
        InteractuarVendedor.gameObject.SetActive(false);
    }

    public void EsconderCanvas()
    {
        contenedorTienda.gameObject.transform.parent.gameObject.SetActive(false); // Desactiva el Canvas completo
        InteractuarVendedor.gameObject.SetActive(true);
        CancelarCompra();
    }
}