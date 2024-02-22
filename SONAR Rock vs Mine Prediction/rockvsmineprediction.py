
import numpy as np
import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import accuracy_score

"""Data Collection and Data Processing

"""

#loading the dataset to pandas' dataframe
sonar_data = pd.read_csv("Copy of sonar data.csv", header= None)

#sonar_data.head()

#sonar_data.shape

#sonar_data.describe()

#sonar_data[60].value_counts()

#sonar_data.groupby(60).mean()

X = sonar_data.drop(columns = 60, axis=1)
Y = sonar_data[60]

"""Training and Test Data"""

X_train, X_test, Y_train, Y_test = train_test_split(X, Y, test_size = 0.1, stratify = Y, random_state = 1)

#print(X.shape, X_train.shape, X_test.shape
#      )

"""Model Training using Logistic Training Model"""

model = LogisticRegression()

model.fit(X_train,Y_train)

"""Model Evaluation"""

#accuracy on training data
X_train_prediction = model.predict(X_train)
training_data_accuracy = accuracy_score(X_train_prediction, Y_train)

#print(training_data_accuracy)

#accuracy on training data
X_test_prediction = model.predict(X_test)
test_data_accuracy=accuracy_score(X_test_prediction, Y_test)

#print(test_data_accuracy)

"""Predicting

"""

input_data = (0.0762,0.0666,0.0481,0.0394,0.0590,0.0649,0.1209,0.2467,0.3564,0.4459,0.4152,0.3952,0.4256,0.4135,0.4528,0.5326,0.7306,0.6193,0.2032,0.4636,0.4148,0.4292,0.5730,0.5399,0.3161,0.2285,0.6995,1.0000,0.7262,0.4724,0.5103,0.5459,0.2881,0.0981,0.1951,0.4181,0.4604,0.3217,0.2828,0.2430,0.1979,0.2444,0.1847,0.0841,0.0692,0.0528,0.0357,0.0085,0.0230,0.0046,0.0156,0.0031,0.0054,0.0105,0.0110,0.0015,0.0072,0.0048,0.0107,0.0094)
#changing the input_data to a numpy array
input_data_as_numpy_array = np.asarray(input_data)
#reshape the array as we are predicting for only one instance
input_data_reshaped = input_data_as_numpy_array.reshape(1,-1)

prediction = model.predict(input_data_reshaped)
print(prediction)

