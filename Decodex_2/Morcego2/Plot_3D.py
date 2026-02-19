import numpy as np
import trimesh

def parse_gcode(gcode_lines):
    vertices = []
    current_position = np.array([0.0, 0.0, 0.0])  # Posição inicial (X, Y, Z)
    
    for line in gcode_lines:
        if line.startswith('G1'):  # Verifica se é um comando de movimento linear
            parts = line.split()
            x = current_position[0]  # Mantém o valor atual de X se não for especificado
            y = current_position[1]  # Mantém o valor atual de Y se não for especificado
            z = current_position[2]  # Mantém o valor atual de Z se não for especificado
            
            for part in parts:
                if part.startswith('X'):
                    x = float(part[1:])  # Extrai o valor de X
                elif part.startswith('Y'):
                    y = float(part[1:])  # Extrai o valor de Y
                elif part.startswith('Z'):
                    z = float(part[1:])  # Extrai o valor de Z
            
            # Atualiza a posição atual
            current_position = np.array([x, y, z])
            vertices.append(current_position.copy())
    
    return np.array(vertices)

def create_mesh_from_gcode(vertices):
    # Cria uma malha a partir dos vértices
    lines = [[i, i+1] for i in range(len(vertices) - 1)]
    mesh = trimesh.creation.line_segments(vertices, lines)
    return mesh

def main():
    # Carrega o G-code de um arquivo
    with open('morcego-1.gcode', 'r') as file:
        gcode_lines = file.readlines()
    
    # Parseia o G-code para obter os vértices
    vertices = parse_gcode(gcode_lines)
    
    # Cria a malha 3D
    mesh = create_mesh_from_gcode(vertices)
    
    # Exporta a malha para um arquivo STL
    mesh.export('output.stl')
    print("Arquivo 3D gerado com sucesso: output.stl")

if __name__ == "__main__":
    main()