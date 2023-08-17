import json
import matplotlib.pyplot as plt
import numpy as np

from mpl_toolkits.mplot3d import Axes3D

def parse_json(json_data):
    data = json.loads(json_data)
    fig = plt.figure()
    ax = fig.add_subplot(111, projection='3d')
    plot_element(ax, data)
    ax.set_xlabel('X')
    ax.set_ylabel('Y')
    ax.set_zlabel('Z')
    ax.set_xlim(-10, 10)
    ax.set_ylim(0, 1)
    ax.set_zlim(-5, 5)
    plt.show()

def plot_element(ax, element):
    if "name" and "components" in element and element["importance"]>0.2 and element["hierarchy_level"]<3:
            # and "x" in element and "y" in element and "z" in element:
        name = element["name"]
        x = element["components"][0]["position"]["x"]
        y = element["components"][0]["position"]["y"]
        z = element["components"][0]["position"]["z"]
        importance = element["importance"]
        ax.scatter(x, y, z)
        ax.text(x, y, z, name)
        print(f"Name: {name}")
        print(f"Coordinates: x={x}, y={y}, z={z}")
        print(f"Importance: {importance}")
        print()

    if "children" in element:
        for child in element["children"]:
            plot_element(ax, child)

# Read the JSON file
with open("Assets/scene_graph_importance.json") as file:
    json_data = file.read()

# Parse the JSON data
parse_json(json_data)
