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
    private Material ultimoMaterialOriginal = null;

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
        MeshRenderer mr = transform.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            ultimoMaterialOriginal = mr.material; // Guarda el material original completo
            mr.material.color = Color.red;
        }

        ultimoReconocido = transform.gameObject;
        textPressE.SetActive(true);
    }

    void Deselect()
    {
        if (ultimoReconocido)
        {
            MeshRenderer mr = ultimoReconocido.GetComponent<MeshRenderer>();
            if (mr != null && ultimoMaterialOriginal != null)
            {
                mr.material = ultimoMaterialOriginal;
            }

            textPressE.SetActive(false);
        }

        ultimoReconocido = null;
        ultimoMaterialOriginal = null;
    }
}

