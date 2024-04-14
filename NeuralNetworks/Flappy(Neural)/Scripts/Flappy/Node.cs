using UnityEngine;
using System.Collections.Generic;
using System;

public class Node
{
    public int id;
    public int layer;
    public float inputValue;
    public float outputValue;
    public List<Connection> connections;

    public Node(int idNumber)
    {
        id = idNumber;
        layer = 0;
        inputValue = 0;
        outputValue = 0;
        connections = new List<Connection>();
    }

    public void Activate()
    {
        Func<float, float> sigmoid = x => 1 / (1 + Mathf.Exp(-x));

        if (layer == 1)
        {
            outputValue = sigmoid(inputValue);
        }

        for( int i = 0 ; i< connections.Count ; i++)
        {
            connections[i].toNode.inputValue += connections[i].weight * outputValue;
        }
    }
}