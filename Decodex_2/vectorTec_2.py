import re

file = "teste_Altura.gcode"
output_file = "teste_Altura_XYSV.txt"

# Expressões regulares
regex_x = re.compile(r"X([-+]?[0-9]*\.?[0-9]+)")
regex_y = re.compile(r"Y([-+]?[0-9]*\.?[0-9]+)")
regex_z = re.compile(r"Z([-+]?[0-9]*\.?[0-9]+)")
regex_e = re.compile(r"E([-+]?[0-9]*\.?[0-9]+)")
regex_f = re.compile(r"F([-+]?[0-9]*\.?[0-9]+)")

x_atual = None
y_atual = None
z_atual = None
z_anterior = None
e_anterior = None
f_atual = None   # feedrate em mm/min

camada = 0
dados = {}

with open(file, "r") as f:
    for linha in f:
        linha = linha.strip()

        # Ignora comentários
        if not linha or linha.startswith(";"):
            continue

        if linha.startswith(("G0", "G1")):

            x_match = regex_x.search(linha)
            y_match = regex_y.search(linha)
            z_match = regex_z.search(linha)
            e_match = regex_e.search(linha)
            f_match = regex_f.search(linha)

            if x_match:
                x_atual = float(x_match.group(1))
            if y_match:
                y_atual = float(y_match.group(1))

            # Atualiza velocidade se existir F
            if f_match:
                f_atual = float(f_match.group(1))

            # Detecta mudança de camada
            if z_match:
                z_atual = float(z_match.group(1))
                if z_anterior is None or z_atual != z_anterior:
                    camada += 1
                    dados[camada] = []
                    z_anterior = z_atual

            # Define shutter (extrusão)
            shutter = 0
            if e_match:
                e_atual = float(e_match.group(1))
                if e_anterior is not None and e_atual > e_anterior:
                    shutter = 1
                e_anterior = e_atual

            # Calcula velocidade em mm/s
            if f_atual is not None:
                velocidade = round(f_atual / 60.0,1)
            else:
                velocidade = 0.0

            # Armazena dados
            if camada > 0 and x_atual is not None and y_atual is not None:
                dados[camada].append((x_atual, y_atual, shutter, velocidade))

# Escrita do arquivo de saída
with open(output_file, "w") as f:
    for layer in sorted(dados.keys()):
        f.write(f"LAYER-{layer}\n")
        for x, y, s, v in dados[layer]:
            f.write(f"{x}\t{y}\t{s}\t{v}\n")
        f.write("\n")

print(f"Arquivo gerado com sucesso: {output_file}")
