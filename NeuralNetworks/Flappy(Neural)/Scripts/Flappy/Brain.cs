using UnityEngine;
using System.Collections.Generic;

public class Brain 
{
    List<Connection> connections = new List<Connection>();
    private List<Node> nodes;
    private List<Node> net;

    int inputs;
    int layers = 2;

    public Brain(int inputs) //bool clone = false)
    {
        this.inputs = inputs;
        nodes = new List<Node>();
        net = new List<Node>();

        // if (!clone)
        // {
        //     // Create input nodes
        // for (int i = 0; i < inputs; i++)
        // {
        //     nodes.Add(new Node(i));
        //     nodes[i].layer = 0;
        // }
        // // Create bias node
        // nodes.Add(new Node(3));
        // nodes[3].layer = 0;
        // // Create output node
        // nodes.Add(new Node(4));
        // nodes[4].layer = 1;
        //
        // // Create connections
        // for (int i = 0; i < 4; i++)
        // {
        //     connections.Add(new Connection(nodes[i], nodes[4], Random.Range(-1f, 1f)));
        // }
        // }
        
        for (int i = 0; i < inputs; i++)
        {
            nodes.Add(new Node(i));
            nodes[i].layer = 0;
        }
        // Create bias node
        nodes.Add(new Node(2));
        nodes[2].layer = 0;
        // Create output node
        nodes.Add(new Node(3));
        nodes[3].layer = 1;
        
        // Create connections
        for (int i = 0; i < 3; i++)
        {
            connections.Add(new Connection(nodes[i], nodes[3], Random.Range(-1f, 1f)));
        }
    }

    public void ConnectNodes()
    {
        foreach (Node node in nodes)
        {
            node.connections = new List<Connection>();
        }

        foreach (Connection connection in connections)
        {
            connection.fromNode.connections.Add(connection);
        }
    }

    public void GenerateNet()
    {
        ConnectNodes();
        net = new List<Node>();

        for (int j = 0; j < layers; j++)
        {
            foreach (Node node in nodes)
            {
                if (node.layer == j)
                {
                    net.Add(node);
                }
            }
        }
    }

    public float FeedForward(float[] vision)
    {
        for (int i = 0; i < inputs; i++)
        {
            nodes[i].outputValue = vision[i];
        }

        nodes[2].outputValue = 1;

        foreach (Node node in net)
        {
            node.Activate();
        }

        // Get output value from output node
        float outputValue = nodes[3].outputValue;

        // Reset node input values - only node 6 Missing Natural Selection in this case
        foreach (Node node in nodes)
        {
            node.inputValue = 0;
        }

        return outputValue;
    }

    // public Brain Clone()
    // {
    //     Brain clone = new Brain(inputs, true);
    //
    //     // Clone all the nodes
    //     foreach (Node n in nodes)
    //     {
    //         clone.nodes.Add(n.Clone());
    //     }
    //
    //     // Clone all connections
    //     foreach (Connection c in connections)
    //     {
    //         clone.connections.Add(c.Clone(clone.GetNode(c.fromNode.id), clone.GetNode(c.toNode.id)));
    //     }
    //
    //     clone.layers = layers;
    //     clone.ConnectNodes();
    //     return clone;
    // }

    // public Node GetNode(int id)
    // {
    //     foreach (Node n in nodes)
    //     {
    //         if (n.id == id)
    //         {
    //             return n;
    //         }
    //     }
    //     return null;
    // }
    //
    // // 80 % chance that a connection undergoes mutation
    // public void Mutate()
    // {
    //     if (Random.value < 0.8f)
    //     {
    //         foreach (Connection c in connections)
    //         {
    //             c.MutateWeight();
    //         }
    //     }
    // }
}
