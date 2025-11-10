using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TittleManager : MonoBehaviour
{

    public GameObject Entry;
    public GameObject Title;
    public string Scene;
    private GameObject fondo;

    public Animator animator;
    bool animacionTerminada = false;

    void Start()
    {
        fondo = GameObject.Find("Background");
        fondo.SetActive(false);
        animator = Entry.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!animacionTerminada)
        {
            AnimatorStateInfo estado = animator.GetCurrentAnimatorStateInfo(0);

            // Verifica si la animación está en el estado deseado y ha terminado
            if (estado.IsName("Entry") && estado.normalizedTime >= 1f)
            {
                animacionTerminada = true;
                fondo.SetActive(true);
                Entry.SetActive(false);


            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void ChangeScene() {
        SceneManager.LoadScene(Scene);
    }
}
