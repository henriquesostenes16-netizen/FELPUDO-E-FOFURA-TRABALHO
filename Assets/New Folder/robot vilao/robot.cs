using System.Collections;
using UnityEngine;

public class LevarFofura : MonoBehaviour
{
    public Transform fofura;
    public float velocidade = 2f;
    public float margemFora = 3f; // quanto além da borda direita ele vai antes de parar
    public Vector3 posicaoNoColo = new Vector3(0.6f, 0.4f, 0f);

    void Start() { StartCoroutine(Sequestro()); }

    IEnumerator Sequestro()
    {
        yield return new WaitForSeconds(1f);

        // 1) Vai até a Fofura
        while (Vector3.Distance(transform.position, fofura.position) > 1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(fofura.position.x, transform.position.y, 0),
                velocidade * Time.deltaTime);
            yield return null;
        }

        // 2) Pega ela
        fofura.SetParent(transform);
        fofura.localPosition = posicaoNoColo;

        // 3) Foge até sair completamente da tela
        Camera cam = Camera.main;
        float bordaDireita = cam.transform.position.x + cam.orthographicSize * cam.aspect;
        float xSaida = bordaDireita + margemFora;

        while (transform.position.x < xSaida)
        {
            transform.position += Vector3.right * velocidade * Time.deltaTime;
            yield return null;
        }
    }

    public void SoltarFofura() { fofura.SetParent(null); }
}