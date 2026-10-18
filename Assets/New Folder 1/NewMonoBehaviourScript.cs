using System.Collections;
using UnityEngine;

public class SequenciaFinal : MonoBehaviour
{
    [Header("Personagens")]
    public Transform robo;
    public Transform fofura;
    public Transform felpudo;

    [Header("Lancador e missil")]
    public Transform lancador;
    public GameObject missil;
    public ParticleSystem fogoMissil;

    [Header("Cena 2")]
    public Transform pontoCena2;
    public ParticleSystem coracoes;
    public GameObject textoFim;

    [Header("Ajustes")]
    public float velRobo = 3f;
    public float velFelpudo = 4f;
    public float velMissil = 6f;
    public float margemFora = 3f;
    public float tempoSacar = 0.4f;
    public float tempoMirando = 0.5f;
    public float tempoFade = 0.8f;
    public Vector3 posicaoNoColo = new Vector3(0.6f, 0.4f, 0f);
    public Vector3 posicaoLancador = new Vector3(0.8f, 0.3f, 0f);

    [Header("Posicoes na cena 2 (X em relacao ao PontoCena2)")]
    public float roboX = 6f;            // robo deitado, longe, na direita
    public float roboY = -0.3f;
    public float anguloRobo = 90f;
    public float fofuraX = 0f;          // Fofura no centro
    public float felpudoParaX = -3f;    // onde o Felpudo para

    float yRobo, yFofura, yFelpudo;
    float fade = 0f;
    Texture2D texPreta;

    void Awake()
    {
        texPreta = new Texture2D(1, 1);
        texPreta.SetPixel(0, 0, Color.black);
        texPreta.Apply();
    }

    void OnGUI()
    {
        if (fade <= 0.001f) return;
        GUI.depth = -100;
        GUI.color = new Color(1, 1, 1, fade);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texPreta);
    }

    float BordaDireita()
    {
        Camera c = Camera.main;
        return c.transform.position.x + c.orthographicSize * c.aspect;
    }

    void Start()
    {
        yRobo = robo.position.y;
        yFofura = fofura.position.y;
        yFelpudo = felpudo.position.y;

        if (lancador) lancador.gameObject.SetActive(false);
        if (missil) missil.SetActive(false);
        if (textoFim) textoFim.SetActive(false);
        if (coracoes) coracoes.Stop();
        StartCoroutine(Sequencia());
    }

    IEnumerator Sequencia()
    {
        yield return new WaitForSeconds(1f);

        // 1) Robo vai ate a Fofura
        yield return MoverAte(robo, new Vector3(fofura.position.x, robo.position.y, 0), velRobo, 1f);

        // 2) Pega a Fofura
        fofura.SetParent(robo);
        fofura.localPosition = posicaoNoColo;

        // 3) Robo foge para fora da tela
        float xSaida = BordaDireita() + margemFora;
        yield return MoverAte(robo, new Vector3(xSaida, robo.position.y, 0), velRobo, 0.05f);

        // 4) Felpudo corre ate o meio da tela
        yield return MoverAte(felpudo, new Vector3(BordaDireita() - 6f, felpudo.position.y, 0), velFelpudo, 0.05f);

        // 5) Felpudo saca o lancador
        Vector3 escalaFinal = Vector3.one;
        if (lancador)
        {
            lancador.SetParent(felpudo);
            lancador.localPosition = posicaoLancador;
            escalaFinal = lancador.localScale;
            lancador.localScale = Vector3.zero;
            lancador.gameObject.SetActive(true);
            yield return Escalar(lancador, Vector3.zero, escalaFinal, tempoSacar);
        }

        yield return new WaitForSeconds(tempoMirando);

        // 6) Atira o missil
        Coroutine voo = StartCoroutine(Disparar(xSaida));
        yield return new WaitForSeconds(0.5f);

        // 7) Guarda o lancador
        if (lancador)
        {
            yield return Escalar(lancador, escalaFinal, Vector3.zero, tempoSacar);
            lancador.SetParent(null);
            lancador.gameObject.SetActive(false);
        }
        yield return voo;
        yield return new WaitForSeconds(0.5f);

        // 8) Escurece e vai para a cena 2
        yield return Fade(0f, 1f, tempoFade);

        // para qualquer outro script do robo (ex.: LevarFofura)
        foreach (MonoBehaviour mb in robo.GetComponents<MonoBehaviour>())
        {
            if (mb == this) continue;
            mb.StopAllCoroutines();
            mb.enabled = false;
        }

        Camera cam = Camera.main;
        Vector3 cp = cam.transform.position;
        cam.transform.position = new Vector3(pontoCena2.position.x, cp.y, cp.z);
        float larg = cam.orthographicSize * cam.aspect;
        float bordaEsq = pontoCena2.position.x - larg;
        float px = pontoCena2.position.x;

        // solta a Fofura de vez
        fofura.SetParent(null);
        fofura.rotation = Quaternion.identity;
        fofura.position = new Vector3(px + fofuraX, yFofura, 0);

        // robo deitado, longe da Fofura
        robo.position = new Vector3(px + roboX, yRobo + roboY, 0);
        robo.rotation = Quaternion.Euler(0, 0, anguloRobo);

        // Felpudo comeca fora da tela, a esquerda
        felpudo.SetParent(null);
        felpudo.position = new Vector3(bordaEsq - 2f, yFelpudo, 0);

        yield return null;
        if (fofura.parent != null) fofura.SetParent(null);
        fofura.position = new Vector3(px + fofuraX, yFofura, 0);

        yield return Fade(1f, 0f, tempoFade);
        yield return new WaitForSeconds(1.5f);

        // 9) Felpudo entra andando
        yield return MoverAte(felpudo, new Vector3(px + felpudoParaX, yFelpudo, 0), velFelpudo * 0.6f, 0.05f);
        yield return new WaitForSeconds(0.4f);

        // 10) Fofura ve o Felpudo: pulinho de surpresa
        yield return Pular(fofura, 0.6f, 0.4f);

        // 11) Aura de coracoes
        if (coracoes)
        {
            coracoes.transform.position = (fofura.position + felpudo.position) / 2f + new Vector3(0, 0.5f, 0);
            coracoes.Play();
        }
        yield return new WaitForSeconds(3f);

        // 12) Escurece, esconde o mundo e mostra FIM em fundo preto
        yield return Fade(0f, 1f, 1f);
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        cam.cullingMask = 0;
        if (textoFim) textoFim.SetActive(true);
        yield return Fade(1f, 0f, 1f);
    }

    IEnumerator Disparar(float xSaida)
    {
        if (!missil) yield break;
        Vector3 origem = lancador ? lancador.position : felpudo.position;
        missil.transform.position = origem;
        missil.SetActive(true);
        if (fogoMissil) fogoMissil.Play();
        yield return MoverAte(missil.transform, new Vector3(xSaida, origem.y + 1f, 0), velMissil, 0.05f);
        missil.SetActive(false);
    }

    IEnumerator Fade(float de, float para, float duracao)
    {
        float t = 0f;
        while (t < duracao)
        {
            t += Time.deltaTime;
            fade = Mathf.Lerp(de, para, t / duracao);
            yield return null;
        }
        fade = para;
    }

    IEnumerator Pular(Transform t, float altura, float duracao)
    {
        Vector3 inicio = t.position;
        float tempo = 0f;
        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float y = Mathf.Sin(tempo / duracao * Mathf.PI) * altura;
            t.position = inicio + new Vector3(0, y, 0);
            yield return null;
        }
        t.position = inicio;
    }

    IEnumerator Escalar(Transform t, Vector3 de, Vector3 para, float duracao)
    {
        float tempo = 0f;
        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            t.localScale = Vector3.Lerp(de, para, tempo / duracao);
            yield return null;
        }
        t.localScale = para;
    }

    IEnumerator MoverAte(Transform t, Vector3 alvo, float vel, float tolerancia)
    {
        while (Vector3.Distance(t.position, alvo) > tolerancia)
        {
            t.position = Vector3.MoveTowards(t.position, alvo, vel * Time.deltaTime);
            yield return null;
        }
    }
}