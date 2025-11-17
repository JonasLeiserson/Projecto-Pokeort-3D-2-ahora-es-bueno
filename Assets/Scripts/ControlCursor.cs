using UnityEngine;

public class ControlCursor: MonoBehaviour
{
    public static ControlCursor instance;
    public bool cursorActivo = false;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        BloquearCursor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (cursorActivo)
                BloquearCursor();
            else
                MostrarCursor();
        }

        if (CombateNPCManager.instance != null || CombateSalvajeManager.instance != null)
        {
            MostrarCursor();
        }
    }

    public void MostrarCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        cursorActivo = true;
    }

    public void BloquearCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cursorActivo = false;
    }
}
