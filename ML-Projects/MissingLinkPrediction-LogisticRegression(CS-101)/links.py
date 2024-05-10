import pandas as pd
import networkx as nx
import numpy as np
from sklearn.linear_model import LogisticRegression
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score

# Read the file
file_path = 'modified_impression_network.csv'
df = pd.read_csv(file_path)

# Create a directed graph
G = nx.DiGraph()

# Adding directed edges
for index, row in df.iterrows():
    person = row[0]  # The person who is impressed
    impressed_by = row[1:].dropna()  # The people they are impressed by
    for impressed_person in impressed_by:
        G.add_edge(person, impressed_person)  # Add an edge from person to impressed_person

# Get the adjacency matrix of the graph
adj_matrix = nx.adjacency_matrix(G).todense()

# Modify the adjacency matrix based on the percentage of common successors
def Common (i, j, adj_matrix, G):    #This function returns the number of common successors of i and j in the graph G.

    i_node = list(G.nodes())[i]
    j_node = list(G.nodes())[j]
    i_successors = set(G.successors(i_node))
    j_successors = set(G.successors(j_node))
    if len(i_successors) == 0:
        return 0
    else:
        return len(i_successors.intersection(j_successors)) / len(i_successors)  #This function returns the feature matrix of the graph G.

def feature_matrix(G, adj_matrix):
    n = len(G.nodes())
    feature_matrix = np.zeros((n, n))
    for i in range(n):
        for j in range(n):
            feature_matrix[i, j] = Common(i, j, adj_matrix, G)
    return feature_matrix

# Generate feature matrix
feature_matrix = feature_matrix(G, adj_matrix)
print(feature_matrix)

# Flatten feature matrix
X = feature_matrix.flatten()

# Flatten adjacency matrix and convert to labels (1 for existing interaction, 0 for non-existing)
y = adj_matrix.flatten()

# Split data into train and test sets
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

# Train logistic regression model
log_reg = LogisticRegression()
log_reg.fit(X_train.reshape(-1, 1), y_train)

# Predictions
y_pred = log_reg.predict(X_test.reshape(-1, 1))

# Function to predict link between two people
def predict_link(person1, person2, G, log_reg):
    # Get the integer indices corresponding to the names of the people
    person1_index = list(G.nodes()).index(person1)
    person2_index = list(G.nodes()).index(person2)
    
    features = Common(person1_index, person2_index, adj_matrix, G)
    # Reshape features for prediction
    features = np.array(features).reshape(1, -1)
    # Predict link
    prediction = log_reg.predict(features)
    return prediction[0]

person1 = "2023CSB1161"     #me
person2 = "2023CSB1164"     #sumit boy
prediction = predict_link(person1, person2, G, log_reg)
print(f"There is {'a' if prediction == 1 else 'no'} link predicted between {person1} and {person2}.")

# # Calculate accuracy
# accuracy = accuracy_score(y_test, y_pred)
# print("Accuracy:", accuracy)


# Function to predict link between all pairs of individuals
def predict_all_links(G, log_reg):
    num_nodes = len(G.nodes())
    total_predicted_links = 0
    for i in range(num_nodes):
        for j in range(num_nodes):
            if i != j:  # Exclude self-loops
                features = Common(i, j, adj_matrix, G)
                features = np.array(features).reshape(1, -1)
                prediction = log_reg.predict(features)
                total_predicted_links += prediction
    return total_predicted_links

# Calculate total number of predicted links
total_predicted_links = predict_all_links(G, log_reg)

print("Total number of predicted links:", total_predicted_links)
