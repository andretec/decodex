import numpy as np
from stl.mesh import Mesh
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d.art3d import Poly3DCollection

def parse_gcode(file_path):
    vertices = []
    current_position = np.array([0.0, 0.0, 0.0])
    
    with open(file_path, 'r') as file:
        for line in file:
            line = line.strip()
            if line.startswith("G0") or line.startswith("G1"):  # Movimentação
                parts = line.split()
                new_position = current_position.copy()
                for part in parts[1:]:
                    if part.startswith("X") and part[1:].strip():
                        new_position[0] = float(part[1:])
                    elif part.startswith("Y") and part[1:].strip():
                        new_position[1] = float(part[1:])
                    elif part.startswith("Z") and part[1:].strip():
                        new_position[2] = float(part[1:])
                
                vertices.append(new_position.copy())
                current_position = new_position
    
    return np.array(vertices[:-1])  # Remove a última camada

def create_stl(vertices, output_file):
    if len(vertices) < 3:
        print("Não há vértices suficientes para criar um STL.")
        return
    
    faces = []
    for i in range(len(vertices) - 2):
        faces.append([vertices[0], vertices[i + 1], vertices[i + 2]])
    
    faces = np.array(faces)
    model = Mesh(np.zeros(faces.shape[0], dtype=Mesh.dtype))
    for i, f in enumerate(faces):
        for j in range(3):
            model.vectors[i][j] = f[j]
    
    model.save(output_file)
    print(f"Arquivo STL salvo como {output_file}")
    return model

def plot_stl(model):
    fig = plt.figure()
    ax = fig.add_subplot(111, projection='3d')
    
    for vector in model.vectors:
        ax.add_collection3d(Poly3DCollection([vector], alpha=0.5, edgecolor='k'))
    
    ax.auto_scale_xyz(model.vectors[:,:,0], model.vectors[:,:,1], model.vectors[:,:,2])
    plt.show()

# Caminho do arquivo G-code
file_path = "morcego-1.gcode"
vertices = parse_gcode(file_path)
model = create_stl(vertices, "output.stl")

if model:
    plot_stl(model)
