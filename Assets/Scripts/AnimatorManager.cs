using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator anim;
    public float velocity;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = transform.parent.gameObject.GetComponent<CharacterController>().velocity.magnitude;
        anim.SetFloat("Velocidad", velocity);
    }
}
