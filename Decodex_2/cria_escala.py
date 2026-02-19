# ==========================
# PARAMETROS DE CONFIGURACAO
# ==========================

file_output = "escala_XYS.txt"
escala_X = 100        # tamanho da escala em X
escala_Y = 100        # tamanho da escala em Y
dist_div = 10         # distancia entre subdivisoes
tam_div = 5           # tamanho das subdivisoes (risquinhos)

# ==========================
# GERACAO DA TABELA
# ==========================

lines = []
lines.append("LAYER-4")

# --- Origem ---
lines.append("0.0\t0.0\t0")

# ==========================
# ESCALA HORIZONTAL (X)
# ==========================
for x in range(dist_div, escala_X + 1, dist_div):
    # Linha principal
    lines.append(f"{float(x)}\t0.0\t1")

    # Sub-divisao vertical (risquinho)
    if x < escala_X:
        lines.append(f"{float(x)}\t{float(tam_div)}\t1")
        lines.append(f"{float(x)}\t0.0\t0")

# Volta para a origem antes da escala Y
lines.append("0.0\t0.0\t0")

# ==========================
# ESCALA VERTICAL (Y)
# ==========================
for y in range(dist_div, escala_Y + 1, dist_div):
    # Linha principal
    lines.append(f"0.0\t{float(y)}\t1")

    # Sub-divisao horizontal (risquinho)
    if y < escala_Y:
        lines.append(f"{float(tam_div)}\t{float(y)}\t1")
        lines.append(f"0.0\t{float(y)}\t0")

# ==========================
# SALVA NO ARQUIVO
# ==========================

with open(file_output, "w") as f:
    for line in lines:
        f.write(line + "\n")

print(f"Arquivo '{file_output}' gerado com sucesso.")
