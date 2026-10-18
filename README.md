# Felpudo e Fofura — Animação 2D ( https://play.unity.com/en/games/9d3a2d61-d20a-405a-919e-d6039cae39d3/felpudo-e-fofura-trablho )

Animação 2D feita na **Unity 6** com **sprites** e **sistema de partículas**. O Felpudo precisa resgatar a Fofura, que foi sequestrada por um robô vilão. Ele saca um lançador de mísseis, atira, e a história termina em uma floresta, com o robô caído e uma aura de **corações** espalhados pela tela.

## Objetivo

- Assistir à sequência completa da animação, do sequestro até o reencontro.
- Ver o Felpudo sacar o lançador, atirar o míssil e guardar a arma.
- Acompanhar a mudança para a segunda cena, na floresta.
- Chegar ao final, com a palavra **FIM** em fundo preto.

## Como assistir

1. Abra o link da animação no **Unity Play** (ou o projeto/executável).
2. A sequência começa sozinha, sem precisar apertar nenhuma tecla.
3. Acompanhe a história até aparecer o **FIM**.
4. Para ver de novo, atualize a página.

## Elementos da animação

| Elemento              | Descrição                                                   |
| --------------------- | ----------------------------------------------------------- |
| Felpudo               | Herói verde, que persegue o robô e atira o míssil           |
| Fofura                | Personagem sequestrada, que reencontra o Felpudo no final   |
| Robô vilão            | Sequestra a Fofura e termina caído na segunda cena          |
| Lançador e míssil     | O Felpudo saca, atira e guarda; o míssil sai com fogo animado |
| Corações              | Aura de partículas espalhadas pela cena do reencontro       |
| Cenários              | Savana na primeira parte e floresta na segunda              |
| Texto FIM             | Aparece em fundo preto no encerramento                      |
| Câmera                | Fixa na primeira cena e muda de posição para a floresta     |

## Sequência da animação

1. O robô caminha até a Fofura, pega e foge para fora da tela.
2. O Felpudo corre atrás dele.
3. Ele saca o lançador, atira o míssil e guarda a arma.
4. A tela escurece e a história muda para a floresta.
5. O robô aparece caído, e o Felpudo chega andando.
6. A Fofura pula de surpresa ao vê-lo, e uma aura de corações se espalha.
7. A tela escurece de novo e surge o **FIM**.

## Como foi feito

- **Engine:** Unity 6 (6000.0.82f1), template 2D
- **Linguagem:** C#, com o script `SequenciaFinal.cs`, que comanda a sequência por corrotinas
- **Efeitos:** Particle System (corações), animação de sprites (fogo do míssil), fade da tela
- **Interface:** TextMeshPro para o texto **FIM**
- **Publicação:** build WebGL no Unity Play

## Demonstração

O vídeo ( https://youtu.be/K0F2FjgVqf4 ) mostra a animação completa, do sequestro até o **FIM**.

## Regras resumidas

- Não há controle do jogador: a animação roda sozinha.
- Duração aproximada: cerca de 30 segundos.
- Ao terminar, a tela fica em preto com a palavra **FIM**.

## Créditos

Trabalho desenvolvido por **Sostenes**.

Sprites do Felpudo, da Fofura, do robô e dos cenários: material da prática Lab 06 (UNEB).