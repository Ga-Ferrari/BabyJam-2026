# 📱 Mobile Controls

Asset para adaptação simples de jogos Unity para plataformas mobile, utilizando o novo **Input System** da Unity.

---

## Como funciona

O sistema detecta automaticamente a plataforma em que o jogo está rodando. Caso seja uma plataforma mobile, um canvas dedicado é ativado, exibindo os controles virtuais na tela.

Esse canvas contém botões com os componentes `OnScreenButton` ou `OnScreenStick`, prontos para uso em dispositivos móveis.

## Como usar

1. Adicione o asset à **primeira cena do jogo** (ou à cena principal onde o jogo ocorre).
2. O objeto possui `DontDestroyOnLoad` e persistirá durante toda a sessão.
3. **Remova-o de cenas onde os controles não devem aparecer**, como menus iniciais e telas de fim de jogo, que já possuem sua própria UI.

## Pré-requisitos

- **Input System** instalado no projeto
- **TMPro** instalado no projeto.
- Inputs do jogador mapeados de acordo com o Input System.

> Os botões já incluídos no template (`Move`, `Jump` e `Pause`) utilizam o mesmo caminho de mapeamento do gamepad.

## Configuração

### `usesStick` (booleana)

No script principal do sistema, `ControlsEnabler`, há uma variável chamada `usesStick` que controla o modo de movimentação exibido no mobile:

| Valor | Comportamento |
|-------|---------------|
| `false` (padrão) | Exibe dois botões: **Left** e **Right** |
| `true` | Exibe um **stick analógico** de movimentação |

**Ative `usesStick`** quando o jogo utilizar movimentação top-down ou qualquer outro esquema que permita movimento livre no plano cartesiano. Caso contrário, para jogos side-scroller por exemplo, mantenha desativado.

### `PauseHandler`

Script responsável por ocultar os botões mobile quando o menu de pause estiver aberto, evitando sobreposição com a UI de pause. Os botões a serem ocultados são definidos pelo inspetor. 

Para ativá-lo, conecte-o através da função `TogglePause()` a um sistema do jogo que sinalize quando o evento de pause ocorre e se deve ser ativado ou desativado.