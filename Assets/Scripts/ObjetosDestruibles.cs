using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetosDestruibles : HitHerramientas
{
    [SerializeField] GameObject objetoRecolectable;
    [SerializeField] int numGolpes = 10;
    [SerializeField] int cantidadRecolectables = 5;
    [SerializeField] float distanciaMaxima = 5f;
    [SerializeField] int energiaMinimaGastada = 2, energiaMaximaGastada = 3;
    int energiaGastada;

    void Update()
    {
        energiaGastada = Random.Range(energiaMinimaGastada, (energiaMaximaGastada + 1));
    }

    public override void Hit()
    {
        while (numGolpes > 0)
        {
            numGolpes -= 1;
            GameManager.Instance.Player.energia -= energiaGastada;
            break;
        }

        if (numGolpes <= 0)
        {
            DestruirObjeto();
            GenerarRecolectables();
        }
    }

    private void DestruirObjeto()
    {
        Destroy(gameObject);
    }

    private void GenerarRecolectables()
    {
        for (int i = 0; i < cantidadRecolectables; i++)
        {
            Vector3 posicionAleatoria = Random.insideUnitSphere * distanciaMaxima;
            posicionAleatoria.z = 0f; // Para asegurarnos de que los recolectables se generen en el plano horizontal

            Instantiate(objetoRecolectable, transform.position + posicionAleatoria, Quaternion.identity);
        }
    }
}
