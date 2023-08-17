import json

def add_importance(data, hierarchy_level=0):
    if isinstance(data, dict):
        data["importance"] = 0
        data["hierarchy_level"] = hierarchy_level
        if "components" in data:
            for component in data["components"]:
                if component.get("type") == "UnityEngine.MeshRenderer":
                    data["importance"] = data.get("importance", 0) + 0.15
                if component.get("type") == "UnityEngine.Animator":
                    data["importance"] = data.get("importance", 0) + 0.2
                if component.get("type") == "UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable":
                    data["importance"] = data.get("importance", 0) + 0.3
                if component.get("type") == "UnityEngine.XR.Interaction.Toolkit.TeleportationAnchor":
                    data["importance"] = data.get("importance", 0) + 0.3
                if component.get("type") == "XROffsetGrabbable":
                    data["importance"] = data.get("importance", 0) + 0.2
                if component.get("type") == "IndestructableObj":
                    data["importance"] = data.get("importance", 0) + 0.2
                if component.get("type") == "UnityEngine.BoxCollider":
                    data["importance"] = data.get("importance", 0) + 0.15
        if "name" in data and data["name"] == "Interactables":
            for child in data.get("children", []):
                child["importance"] = child.get("importance", 0) + 0.3
                add_importance(child, hierarchy_level + 1)
        if "name" in data and data["name"] == "Potions":
            for child in data.get("children", []):
                child["importance"] = child.get("importance", 0) + 0.1
                add_importance(child, hierarchy_level + 1)
        if "name" in data and data["name"] == "Interior":
            for child in data.get("children", []):
                child["importance"] = child.get("importance", 0) + 0.2
                add_importance(child, hierarchy_level + 1)
        for key, value in data.items():
            if key == "children" or key == "components":
                for item in value:
                    add_importance(item, hierarchy_level + 1)
            else:
                add_importance(value, hierarchy_level + 1)
    elif isinstance(data, list):
        for item in data:
            add_importance(item, hierarchy_level + 1)

def add_description(data, hierarchy_level=0):
    if isinstance(data, dict):
        if "description" not in data:
            data["description"] = "Hi!"
            data["hierarchy_level"] = hierarchy_level
            for key, value in data.items():
                if key == "children" or key == "components":
                    for item in value:
                        add_description(item, hierarchy_level + 1)
                else:
                    add_description(value, hierarchy_level + 1)
    elif isinstance(data, list):
        for item in data:
            add_description(item, hierarchy_level + 1)

# Load JSON data from file
with open('scene_graph.json') as file:
    json_data = json.load(file)

# Add "importance" and "hierarchy_level" properties
add_importance(json_data)
add_description(json_data)

# Save modified JSON data back to file
with open('scene_graph_importance.json', 'w') as file:
    json.dump(json_data, file, indent=4)  # Add line breaks with indentation
