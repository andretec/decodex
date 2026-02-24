# ==========================
# PARAMETROS DE CONFIGURACAO
# ==========================

file_output = "grade_centralizada_150x150_XYS.txt"
escala_X = 450        # tamanho total em X
escala_Y = 450        # tamanho total em Y
dist_div = 150         # distancia entre linhas da grade
tam_div = 150         # mantido por compatibilidade
centralizada = True   # True = origem no centro | False = origem no canto

# ==========================
# DEFINICAO DOS LIMITES
# ==========================

if centralizada:
    x_min = -escala_X / 2
    x_max =  escala_X / 2
    y_min = -escala_Y / 2
    y_max =  escala_Y / 2
else:
    x_min = 0.0
    x_max = float(escala_X)
    y_min = 0.0
    y_max = float(escala_Y)

# ==========================
# GERACAO DA GRADE OTIMIZADA
# ==========================

lines = []

lines.append("LAYER-1")
lines.append("LAYER-2")
lines.append("LAYER-3")
lines.append("LAYER-4")


# ==========================
# LINHAS VERTICAIS (zig-zag)
# ==========================

y_start = y_min
y_end = y_max

x_values = [x_min + i * dist_div for i in range(int((x_max - x_min) / dist_div) + 1)]

for i, x in enumerate(x_values):
    # deslocamento lateral (laser desligado)
    lines.append(f"{x:.1f}\t{y_start:.1f}\t0")

    # escrita alternada
    lines.append(f"{x:.1f}\t{y_end:.1f}\t1")

    # inverte o sentido
    y_start, y_end = y_end, y_start

# ==========================
# LINHAS HORIZONTAIS (zig-zag)
# ==========================

x_start = x_min
x_end = x_max

y_values = [y_min + i * dist_div for i in range(int((y_max - y_min) / dist_div) + 1)]

for i, y in enumerate(y_values):
    # deslocamento vertical (laser desligado)
    lines.append(f"{x_start:.1f}\t{y:.1f}\t0")

    # escrita alternada
    lines.append(f"{x_end:.1f}\t{y:.1f}\t1")

    # inverte o sentido
    x_start, x_end = x_end, x_start

# ==========================
# VOLTA PARA A ORIGEM
# ==========================

lines.append("0.0\t0.0\t0")
lines.append("0.0\t0.0\t0")

# ==========================
# SALVA NO ARQUIVO
# ==========================

with open(file_output, "w") as f:
    for line in lines:
        f.write(line + "\n")

print(f"Arquivo '{file_output}' gerado com sucesso.")
