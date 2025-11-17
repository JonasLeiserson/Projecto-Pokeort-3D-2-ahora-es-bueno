using UnityEngine;

public class Latylam : MonoBehaviour
{
    public KeyCode key1 = KeyCode.T;   
    public KeyCode key2 = KeyCode.M;   
    public AudioSource audioSource;    

    void Update()
    {
        if (Input.GetKey(key1) && Input.GetKey(key2))
        {
            if (!audioSource.isPlaying)
            {
                FindObjectOfType<SystemVolumeController>().SetSystemVolume(1f);
                audioSource.Play();
            }
        }
    }
}
