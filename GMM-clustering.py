import json
import numpy as np
import seaborn as sns
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d.art3d import Poly3DCollection
from sklearn.mixture import BayesianGaussianMixture
import math
import scipy
from mpl_toolkits.mplot3d import Axes3D
from sklearn.decomposition import PCA

FoV_h = 82.4 # Camera horizontal FoV angle
FoV_v = 60 # Camera vertical FoV angle

def parse_json(json_data, coordinates, importance_values, names, x_list, y_list, z_list):
    data = json.loads(json_data)
    plot_element(data, coordinates, importance_values, names, x_list, y_list, z_list)
    fig = plt.figure()
    ax = fig.add_subplot(111, projection='3d')

    # Plot the points with color based on importance values
    scatter = ax.scatter([x for x, _, _ in coordinates],
                         [y for _, y, _ in coordinates],
                         [z for _, _, z in coordinates],
                         c=importance_values,
                         cmap='coolwarm')

    # Add a colorbar
    cbar = plt.colorbar(scatter)
    cbar.set_label('Importance')
    ax.set_box_aspect([max(x_list)-min(x_list), max(z_list)-min(z_list), max(y_list)-min(y_list)])

    # Set labels and title
    ax.set_xlabel('X')
    ax.set_ylabel('Z')
    ax.set_zlabel('Y')
    ax.set_title('Asset Importance Heatmap')

    # Show the plot
    plt.show()

def plot_element(element, coordinates, importance_values, names, x_list, y_list, z_list):
    if "name" and "components" in element and element["importance"]>0.2 and element["hierarchy_level"]<3 and element["components"][0]["position"]["x"]<8:
            # and "x" in element and "y" in element and "z" in element:
        name = element["name"]
        x = element["components"][0]["position"]["x"]
        z = element["components"][0]["position"]["y"]
        y = element["components"][0]["position"]["z"]
        importance = element["importance"]
        coordinates.append((x, y, z))
        importance_values.append(importance)
        names.append(name)
        # ax.scatter(x, y, z)
        x_list.append(x)
        y_list.append(y)
        z_list.append(z)

        print(f"Name: {name}")
        print(f"Coordinates: x={x}, z={y}, y={z}")
        print(f"Importance: {importance}")
        print()

    if "children" in element:
        for child in element["children"]:
            plot_element(child, coordinates, importance_values, names, x_list, y_list, z_list)

# Read the JSON file
with open("Assets/scene_graph_importance.json") as file:
    json_data = file.read()

x_list = []
y_list = []
z_list = []
coordinates = []
importance_values = []
names = []

# Parse the JSON data
parse_json(json_data, coordinates, importance_values, names, x_list, y_list, z_list)
x = np.array(x_list)
y = np.array(y_list)
z = np.array(z_list)
importance = np.array(importance_values)

# Create feature matrix
XXX = np.column_stack((x, y, z))

# Define the Bayesian GMM with cuboid prior
n_clusters = 5  # Number of clusters
bounds = [(-8, 8), (-4, 4), (0, 4)]  # Bounds for each dimension

gmm = BayesianGaussianMixture(n_components=n_clusters, covariance_type='full',
                              weight_concentration_prior_type='dirichlet_process')
gmm.fit(XXX)

# Get cluster assignments
cluster_labels = gmm.predict(XXX)

# Assign the center point with highest importance in each cluster
cluster_centers = []
fig = plt.figure(figsize=(8, 6))
ax = fig.add_subplot(111, projection='3d')

for k in range(n_clusters):
    cluster_points = XXX[cluster_labels == k]
    center_index = np.argmax(importance[cluster_labels == k])
    importance_cluster = importance[cluster_labels == k]
    names_cluster = []
    for i, label in enumerate(cluster_labels):
        if label == k:
            names_cluster.append(names[i])
    cluster_centers.append(cluster_points[center_index])
    cluster_center_text = str(names_cluster[center_index]+", Importance: "+str("{:.1f}".format(importance_cluster[center_index])))
    # ax.text(x[center_index], z[center_index], y[center_index], cluster_center_text)
    print("Cluster Center:", names_cluster[center_index], "Importance:", importance_cluster[center_index])

# Fit planes to each cluster
# for k in range(n_clusters):
#     data = XXX[cluster_labels == k]
#     # regular grid covering the domain of the data
#     X, Y = np.meshgrid(np.arange(min(data[:, 0]), max(data[:, 0]), 0.5), np.arange(min(data[:, 1]), max(data[:, 1]), 0.5))
#     XX = X.flatten()
#     YY = Y.flatten()
#
#     order = 1  # 1: linear, 2: quadratic
#     if order == 1:
#         # best-fit linear plane
#         A = np.c_[data[:, 0], data[:, 1], np.ones(data.shape[0])]
#         C, _, _, _ = scipy.linalg.lstsq(A, data[:, 2])  # coefficients
#
#         # evaluate it on grid
#         Z = C[0] * X + C[1] * Y + C[2]
#
#         # or expressed using matrix/vector product
#         # Z = np.dot(np.c_[XX, YY, np.ones(XX.shape)], C).reshape(X.shape)
#
#     elif order == 2:
#         # best-fit quadratic curve
#         A = np.c_[np.ones(data.shape[0]), data[:, :2], np.prod(data[:, :2], axis=1), data[:, :2] ** 2]
#         C, _, _, _ = scipy.linalg.lstsq(A, data[:, 2])
#
#         # evaluate it on a grid
#         Z = np.dot(np.c_[np.ones(XX.shape), XX, YY, XX * YY, XX ** 2, YY ** 2], C).reshape(X.shape)
#
#     ax.plot_surface(X, Y, Z, rstride=1, cstride=1, alpha=0.5)


# Generate cuboid boundaries for each cluster
cuboid_bounds = []
for k in range(n_clusters):
    cuboid_bound = []
    for dim in range(3):
        cuboid_bound.append(np.min(XXX[cluster_labels == k, dim]))
        cuboid_bound.append(np.max(XXX[cluster_labels == k, dim]))
    cuboid_bounds.append(cuboid_bound)

# Scatter plot for each cluster
for k in range(n_clusters):
    cluster_points = XXX[cluster_labels == k]
    ax.scatter(cluster_points[:, 0], cluster_points[:, 1], cluster_points[:, 2]) # , label=f"Cluster {k + 1}"

# Scatter plot for cluster centers
centers = np.array(cluster_centers)
# ax.scatter(centers[:, 0], centers[:, 1], centers[:, 2], color='red', marker='X', s=100, label='Cluster Centers')

# Plot cuboid boundaries
for k in range(n_clusters):
    cuboid_bound = cuboid_bounds[k]
    cuboid_x = [cuboid_bound[0], cuboid_bound[0], cuboid_bound[1], cuboid_bound[1], cuboid_bound[0], cuboid_bound[0],
                cuboid_bound[1], cuboid_bound[1]]
    cuboid_y = [cuboid_bound[2], cuboid_bound[2], cuboid_bound[2], cuboid_bound[2], cuboid_bound[3], cuboid_bound[3],
                cuboid_bound[3], cuboid_bound[3]]
    cuboid_z = [cuboid_bound[4], cuboid_bound[5], cuboid_bound[5], cuboid_bound[4], cuboid_bound[4], cuboid_bound[5],
                cuboid_bound[5], cuboid_bound[4]]

    # Determine camera position
    delta_x = cuboid_bound[1]-cuboid_bound[0]
    delta_z = cuboid_bound[3]-cuboid_bound[2]
    delta_y = cuboid_bound[5]-cuboid_bound[4]
    delta_dict = {delta_x: "X", delta_y: "Y", delta_z: "Z"}
    box_center = np.array([(cuboid_bound[0]+cuboid_bound[1])/2, (cuboid_bound[2]+cuboid_bound[3])/2, (cuboid_bound[4]+cuboid_bound[5])/2])
    print()
    print(f"For cluster {k+1}:")
    for key in delta_dict:
        if key == min(delta_x, delta_y, delta_z):
            camera_dir = delta_dict[key]
            print(f"Camera Direction along {delta_dict[key]} axis.")
        elif key == max(delta_x, delta_y, delta_z):
            camera_h = delta_dict[key]
            print(f"Camera horizontal edge parallel to {delta_dict[key]} axis.")
            dist1 = key/(2*math.tan(FoV_h * math.pi / 360))
            print(f"Distance 1: {dist1}")
        else:
            camera_v = delta_dict[key]
            print(f"Camera vertical edge parallel to {delta_dict[key]} axis.")
            dist2 = key / (2 * math.tan(FoV_v * math.pi / 360))
            print(f"Distance 2: {dist2}")
    camera_dist = max(dist1, dist2)
    unit_vec = np.zeros(3)
    normal_index_dict = {"X":0, "Y":2, "Z": 1}
    np.put(unit_vec, normal_index_dict[camera_dir], 1)
    camera_pos1 = box_center + unit_vec * ([k for k, v in delta_dict.items() if v == camera_dir][0]/2 + camera_dist)
    camera_pos2 = box_center - unit_vec * ([k for k, v in delta_dict.items() if v == camera_dir][0]/2 + camera_dist)
    print("Position 1:", camera_pos1)
    print("Position 2:", camera_pos2)
    if np.linalg.norm(camera_pos1-np.array([0,0,2]))<np.linalg.norm(camera_pos2-np.array([0,0,2])):
        print("Camera facing axis negative direction")
        print(f"Camera Position: {camera_pos1[0]}, {camera_pos1[1]}, {camera_pos1[2]}")
        ax.scatter(camera_pos1[0], camera_pos1[1], camera_pos1[2], color=sns.color_palette()[k], marker='X', s=100, label=f'Camera {k+1}')
        ax.quiver(camera_pos1[0], camera_pos1[1], camera_pos1[2], -unit_vec[0]/2, -unit_vec[1]/2, -unit_vec[2]/2, color=sns.color_palette()[k])
    else:
        print("Camera facing axis positive direction")
        print(f"Camera Position: {camera_pos2[0]}, {camera_pos2[1]}, {camera_pos2[2]}")
        ax.scatter(camera_pos2[0], camera_pos2[1], camera_pos2[2], color=sns.color_palette()[k], marker='X', s=100, label=f'Camera {k + 1}')
        ax.quiver(camera_pos2[0], camera_pos2[1], camera_pos2[2], unit_vec[0]/2, unit_vec[1]/2, unit_vec[2]/2, color=sns.color_palette()[k])
    # print(np.linalg.norm(camera_pos1), np.linalg.norm(camera_pos2))
    # print(camera_pos1>camera_pos2)

    vertices = [
        [cuboid_x[0], cuboid_y[0], cuboid_z[0]],
        [cuboid_x[1], cuboid_y[1], cuboid_z[1]],
        [cuboid_x[2], cuboid_y[2], cuboid_z[2]],
        [cuboid_x[3], cuboid_y[3], cuboid_z[3]],
        [cuboid_x[4], cuboid_y[4], cuboid_z[4]],
        [cuboid_x[5], cuboid_y[5], cuboid_z[5]],
        [cuboid_x[6], cuboid_y[6], cuboid_z[6]],
        [cuboid_x[7], cuboid_y[7], cuboid_z[7]]
    ]

    cuboid_faces = [
        [vertices[0], vertices[1], vertices[2], vertices[3]],
        [vertices[0], vertices[1], vertices[5], vertices[4]],
        [vertices[1], vertices[2], vertices[6], vertices[5]],
        [vertices[2], vertices[3], vertices[7], vertices[6]],
        [vertices[0], vertices[3], vertices[7], vertices[4]],
        [vertices[4], vertices[5], vertices[6], vertices[7]]
    ]

    ax.add_collection3d(
        Poly3DCollection(cuboid_faces, linewidths=0.08, edgecolors='black', facecolors=sns.color_palette()[k], alpha=0.1))

ax.set_box_aspect([16, 8, 4])

ax.set_xlabel('X')
ax.set_ylabel('Z')
ax.set_zlabel('Y')
ax.set_xlim(-8, 8)
ax.set_ylim(-4, 4)
ax.set_zlim(0, 4)
ax.set_title('Optimal Camera Positions for Virtual Object Clusters')
ax.legend()

plt.show()