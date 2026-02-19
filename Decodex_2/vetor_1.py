import re

def interpret_gcode(file):
    """
    Interpreta arquivo G-code do Creality Slicer e extrai coordenadas X, Y por camadas
    """
    layers_data = []
    current_layer = []
    layer_number = 0
    in_layer = False
    
    # Regex para encontrar comandos de movimento (G0/G1) com coordenadas X e Y
    coord_pattern = re.compile(r'G[01]\s+X([0-9\.-]+)\s+Y([0-9\.-]+)')
    # Regex para encontrar indicação de camada (comentários do Creality Slicer)
    layer_pattern = re.compile(r';\s*LAYER\s*:?\s*(\d+)', re.IGNORECASE)
    
    try:
        with open(file, 'r', encoding='utf-8') as f:
            for line_num, line in enumerate(f, 1):
                line = line.strip()
                
                # Verifica se é uma nova camada
                layer_match = layer_pattern.search(line)
                if layer_match:
                    # Salva a camada anterior se existir
                    if current_layer and in_layer:
                        layers_data.append({
                            'layer_num': layer_number,
                            'coordinates': current_layer
                        })
                    
                    # Inicia nova camada
                    layer_number = int(layer_match.group(1))
                    current_layer = []
                    in_layer = True
                    continue
                
                # Se estamos dentro de uma camada, procura por coordenadas X, Y
                if in_layer and line.startswith(('G0', 'G1')):
                    coord_match = coord_pattern.search(line)
                    if coord_match:
                        x = float(coord_match.group(1))
                        y = float(coord_match.group(2))
                        current_layer.append((x, y))
                        
                # Verifica se é o início da impressão (primeira camada)
                elif ';TYPE:' in line.upper() and not in_layer:
                    in_layer = True
                    layer_number = 0
            
            # Adiciona a última camada
            if current_layer and in_layer:
                layers_data.append({
                    'layer_num': layer_number,
                    'coordinates': current_layer
                })
    
    except FileNotFoundError:
        print(f"Erro: Arquivo '{file}' não encontrado.")
        return None
    except Exception as e:
        print(f"Erro ao processar arquivo: {e}")
        return None
    
    return layers_data

def save_to_output(layers_data, output_file='output_coordinates.txt'):
    """
    Salva as coordenadas em um arquivo de saída formatado
    """
    try:
        with open(output_file, 'w', encoding='utf-8') as f:
            for layer in layers_data:
                # Escreve o cabeçalho da camada
                f.write(f"Layer: {layer['layer_num']}\n")
                f.write("X\tY\n")
                
                # Escreve as coordenadas
                for x, y in layer['coordinates']:
                    f.write(f"{x:.3f}\t{y:.3f}\n")
                
                f.write("\n")  # Linha em branco entre camadas
        
        print(f"Arquivo salvo com sucesso: {output_file}")
        print(f"Total de camadas processadas: {len(layers_data)}")
        
    except Exception as e:
        print(f"Erro ao salvar arquivo: {e}")

def main():
    # Defina o nome do arquivo de entrada aqui
    file = "morcego-1.gcode"  # Substitua pelo nome do seu arquivo
    
    # Interpreta o G-code
    layers_data = interpret_gcode(file)
    
    if layers_data:
        # Salva o resultado
        save_to_output(layers_data)
        
        # Mostra estatísticas
        total_points = sum(len(layer['coordinates']) for layer in layers_data)
        print(f"Total de pontos extraídos: {total_points}")
        
        # Exemplo de acesso aos dados
        print("\nExemplo dos dados extraídos:")
        for i, layer in enumerate(layers_data[:3]):  # Mostra as 3 primeiras camadas
            print(f"\nCamada {layer['layer_num']}: {len(layer['coordinates'])} pontos")
            if layer['coordinates']:
                print(f"  Primeiro ponto: X={layer['coordinates'][0][0]:.2f}, Y={layer['coordinates'][0][1]:.2f}")
                print(f"  Último ponto: X={layer['coordinates'][-1][0]:.2f}, Y={layer['coordinates'][-1][1]:.2f}")

# Versão alternativa para processamento mais detalhado
def advanced_gcode_parser(file):
    """
    Parser avançado que lida com diferentes formatos de G-code
    """
    layers = []
    current_layer = []
    layer_num = 0
    extrusion_active = False
    
    try:
        with open(file, 'r', encoding='utf-8') as f:
            for line in f:
                line = line.strip()
                
                # Ignora comentários e linhas vazias
                if not line or line.startswith(';'):
                    # Verifica se é comentário de camada
                    if 'layer' in line.lower() and any(word in line.lower() for word in ['layer', 'height']):
                        if current_layer:
                            layers.append((layer_num, current_layer))
                            current_layer = []
                        # Tenta extrair número da camada
                        for word in line.split():
                            if word.isdigit():
                                layer_num = int(word)
                                break
                        layer_num += 1
                    continue
                
                # Processa comandos de movimento
                parts = line.split()
                if parts[0] in ['G0', 'G1']:
                    x = y = None
                    # Procura por coordenadas X e Y
                    for part in parts[1:]:
                        if part.startswith('X'):
                            x = float(part[1:])
                        elif part.startswith('Y'):
                            y = float(part[1:])
                    
                    if x is not None and y is not None:
                        current_layer.append((x, y))
        
        # Adiciona a última camada
        if current_layer:
            layers.append((layer_num, current_layer))
    
    except Exception as e:
        print(f"Erro: {e}")
        return []
    
    return layers

if __name__ == "__main__":
    # Exemplo de uso
    main()
    
    # Ou use o parser avançado
    # file = "seu_arquivo.gcode"
    # layers = advanced_gcode_parser(file)
    # save_to_output([{'layer_num': num, 'coordinates': coords} for num, coords in layers])