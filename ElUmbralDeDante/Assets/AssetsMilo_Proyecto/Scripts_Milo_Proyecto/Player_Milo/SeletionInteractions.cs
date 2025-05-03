using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeletionInteractions : MonoBehaviour
{
    public float distancia = 1.5f;
    private LayerMask mask;
    [SerializeField] private Camera cam;

    public GameObject textPressE;
    GameObject ultimoReconocido = null;
    //private Material ultimoMaterialOriginal = null;
    selectorVisual SelectorVisual;

    void Start()
    {
        mask = LayerMask.GetMask("Raycast Detect"); // Asegúrate de que los objetos tengan este layer
        textPressE.SetActive(false);
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (ultimoReconocido == null)
        {
            textPressE.SetActive(false);
        }
        // Debug del rayo (verde desde la cámara hacia adelante)
        Debug.DrawRay(ray.origin, ray.direction * distancia, Color.green);

        if (Physics.Raycast(ray, out hit, distancia, mask))
        {

            Deselect();
            SelectedObject(hit.transform);

            if (Input.GetKeyDown(KeyCode.E))
            {
                IInteractuable objeto = hit.collider.GetComponent<IInteractuable>();
                if (objeto != null)
                {
                    objeto.ActivarObjeto();
                }
            }
        }
        else
        {
            Deselect();
        }
    }

    void SelectedObject(Transform transform)
    {
        SelectorVisual = transform.GetComponent<selectorVisual>();
        if (SelectorVisual != null)
        {
            SelectorVisual.ActivarResaltado();
        }

        ultimoReconocido = transform.gameObject;
        textPressE.SetActive(true);
    }

    void Deselect()
    {
        if (ultimoReconocido)
        {
            SelectorVisual = ultimoReconocido.GetComponent<selectorVisual>();
            if (SelectorVisual != null)
            {
                SelectorVisual.DesactivarResaltado();
            }

            textPressE.SetActive(false);
        }

        ultimoReconocido = null;
        SelectorVisual = null;
    }
}

