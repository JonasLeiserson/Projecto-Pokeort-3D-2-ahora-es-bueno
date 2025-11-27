using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdminDebugger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("EscenaCasa");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("EscenaTienda");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("EscenaCentro");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadScene("GameScene");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SceneManager.LoadScene("Gimnasio");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SceneManager.LoadScene("Laboratorio");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.transform.position = new Vector3(177.0843f, 3f, 18.66978f);

            if (cc != null) cc.enabled = true;
        }
    }
}
