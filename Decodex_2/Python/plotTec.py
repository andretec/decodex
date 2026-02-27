import matplotlib.pyplot as plt
from matplotlib.widgets import Slider

arquivo = "base_USP_dodecaedro_LH_15um_LW_04_lines_04um_H.txt"

# =========================
# Leitura do arquivo
# =========================
camadas = {}
camada_atual = None

with open(arquivo, "r") as f:
    for linha in f:
        linha = linha.strip()

        # Identificação da camada
        if linha.startswith("LAYER-"):
            camada_atual = int(linha.split("-")[1])
            camadas[camada_atual] = []
            continue

        # Ignorar cabeçalho ou linhas vazias
        if linha.startswith("X") or not linha:
            continue

        partes = linha.split()

        # Garantir que existem pelo menos 3 colunas
        if len(partes) >= 3:
            x = float(partes[0])
            y = float(partes[1])
            shutter = int(partes[2])

            camadas[camada_atual].append((x, y, shutter))

total_camadas = max(camadas.keys())

# =========================
# Função de plot
# =========================
def plot_layer(layer):
    ax.clear()
    dados = camadas[layer]

    for i in range(1, len(dados)):
        x0, y0, _ = dados[i - 1]
        x1, y1, shutter = dados[i]

        if shutter == 1:
            cor = "blue"
        else:
            cor = "green"

        ax.plot([x0, x1], [y0, y1],
                marker='o',
                linestyle='solid',
                markersize=5,
                color=cor)

    ax.set_title(f"Camada {layer}")
    ax.set_aspect("equal", adjustable="box")
    ax.set_xlabel("X")
    ax.set_ylabel("Y")
    ax.grid(True)

    fig.canvas.draw_idle()

# =========================
# Interface gráfica
# =========================
fig, ax = plt.subplots()
plt.subplots_adjust(left=0.1, right=0.85)

ax_slider = plt.axes([0.88, 0.2, 0.03, 0.6])
slider = Slider(
    ax=ax_slider,
    label="Layer",
    valmin=1,
    valmax=total_camadas,
    valinit=1,
    valstep=1,
    orientation="vertical"
)

slider.on_changed(lambda val: plot_layer(int(val)))

# Plot inicial
plot_layer(1)

plt.show()
