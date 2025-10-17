using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TiendaUiGenerador : MonoBehaviour
{
    [Header("Referencias")]
    public Transform contenedorTienda;
    public TMP_FontAsset fuenteTMP;
    public Item[] itemsDisponibles;
    public int dineroJugador = 500;
    public TMP_Text textoDinero;
    public static TiendaUiGenerador instance;
    public Button InteractuarVendedor;
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
    }
    void Start()
    {
        CrearBotones();
        ActualizarDinero();
    }

    void CrearBotones()
    {
        foreach (Item item in itemsDisponibles)
        {
            GameObject panel = new GameObject(item.itemName, typeof(RectTransform));
            panel.transform.SetParent(contenedorTienda, false);
            Image fondo = panel.AddComponent<Image>();
            fondo.color = new Color(0.95f, 0.95f, 0.95f, 1f);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(250, 100);

            GameObject imagenObj = new GameObject("Icono", typeof(Image));
            imagenObj.transform.SetParent(panel.transform, false);
            Image img = imagenObj.GetComponent<Image>();
            img.sprite = item.icon;
            RectTransform rectImg = img.GetComponent<RectTransform>();
            rectImg.anchorMin = new Vector2(0, 0);
            rectImg.anchorMax = new Vector2(0, 1);
            rectImg.offsetMin = new Vector2(10, 10);
            rectImg.offsetMax = new Vector2(90, -10);

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

            // Botón Comprar
            GameObject botonObj = new GameObject("BotonComprar", typeof(Button), typeof(Image));
            botonObj.transform.SetParent(panel.transform, false);
            Image botonImg = botonObj.GetComponent<Image>();
            botonImg.color = new Color(0.7f, 0.9f, 0.7f, 1f);
            Button boton = botonObj.GetComponent<Button>();
            RectTransform rectBoton = botonObj.GetComponent<RectTransform>();
            rectBoton.anchorMin = new Vector2(0.65f, 0.1f);
            rectBoton.anchorMax = new Vector2(0.95f, 0.4f);
            rectBoton.offsetMin = Vector2.zero;
            rectBoton.offsetMax = Vector2.zero;

            GameObject textoBoton = new GameObject("Texto", typeof(TextMeshProUGUI));
            textoBoton.transform.SetParent(botonObj.transform, false);
            TMP_Text txtBoton = textoBoton.GetComponent<TMP_Text>();
            txtBoton.text = "Comprar";
            txtBoton.font = fuenteTMP;
            txtBoton.fontSize = 18;
            txtBoton.alignment = TextAlignmentOptions.Center;
            txtBoton.color = Color.black;
            RectTransform rectTxtBoton = textoBoton.GetComponent<RectTransform>();
            rectTxtBoton.anchorMin = Vector2.zero;
            rectTxtBoton.anchorMax = Vector2.one;
            rectTxtBoton.offsetMin = Vector2.zero;
            rectTxtBoton.offsetMax = Vector2.zero;

            // Evento de compra
            boton.onClick.AddListener(() => ComprarItem(item));
        }
    }

    void ComprarItem(Item item)
    {
        if (PlataManager.instance.PlataJugador >= item.valor)
        {

            PlataManager.instance.QuitarDinero(item.valor);

            ActualizarDinero();

            Debug.Log("🛒 Compraste " + item.itemName);
        }
        else
        {
            Debug.Log("❌ No tienes suficiente dinero para comprar " + item.itemName);
        }
    }

    void ActualizarDinero()
    {
        if (textoDinero != null)
            textoDinero.text = "Dinero: $" + PlataManager.instance.PlataJugador;
    }
    public void MostrarCanvas()
    {
        contenedorTienda.gameObject.SetActive(true);
        InteractuarVendedor.gameObject.SetActive(false);
    }
    public void EsconderCanvas()
    {
        contenedorTienda.gameObject.SetActive(false);
        InteractuarVendedor.gameObject.SetActive(true);
    }
}
