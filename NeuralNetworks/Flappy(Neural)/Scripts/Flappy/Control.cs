using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Control : MonoBehaviour
{
    [SerializeField] private float jump = 4;
    [SerializeField] private float gravity = 19;
    public int populationNum = 400;
    [SerializeField] private GameObject env;
    [SerializeField] private Slider gr;
    [SerializeField] private Slider jmp;
    public float[] velocity;
    [SerializeField] private GameObject birdPrefab;
    public GameObject population;
    public float birdUpdate =0.01f;
    private float birdUpdateTime ;
    public Brain[] birdBrains = new Brain[400];
    

    public void Start()
    {
        Application.targetFrameRate = 60;
        for (int i = 0; i < populationNum; i++)
        {
            GameObject bird = Instantiate(birdPrefab);
            bird.name = (i + 1).ToString();
            
            bird.transform.parent = this.transform;
            birdBrains[i] = new Brain(2);
            birdBrains[i].GenerateNet();
        }
        velocity = new float[populationNum];

        
    }

    void Update()
    {

        // Slider
        gr.onValueChanged.AddListener((v) =>{
            gravity = v;
        });
        jmp.onValueChanged.AddListener((v) =>{
            jump = v;
        });



        if (this.transform.childCount == 0)
            Start();
        
        birdUpdateTime -= Time.deltaTime;
        
        if (birdUpdateTime <= 0)
        {
            
            
            birdUpdateTime = birdUpdate;
            
            
            foreach (Transform bird in population.transform)
            {
                float closestPipeX = 100;
                float closestPipeY = 100;
                

                foreach (Transform pipe in env.transform.Find("pipeParentHolder"))
                {
                    if (pipe.transform.position.x - bird.transform.position.x < closestPipeX &&
                        pipe.transform.position.x - bird.transform.position.x > 0f)
                    {
                        closestPipeX = pipe.transform.position.x - bird.transform.position.x;
                        closestPipeY = pipe.transform.position.y - bird.transform.position.y;
                        
                        
                        
                    }
                }
                



                float[] vision = new float[2];
                vision[0] = closestPipeX;
                vision[1] = closestPipeY;
                
                
                int t = int.Parse(bird.name) - 1;
                float decision = birdBrains[t].FeedForward(vision);
                Debug.Log(decision);
                velocity[t] += -gravity * Time.deltaTime;
                
                if (decision >= 0.73f)
                {
                    velocity[t] = 0;
                    velocity[t] += jump;
                    bird.transform.Rotate(Vector3.forward, 30f);
                }

                bird.transform.position += velocity[t] * Time.deltaTime * Vector3.up;
                
            }
        }
        else
        {
            foreach (Transform bird in population.transform)
            {
                int t = int.Parse(bird.name) - 1;
                velocity[t] += -gravity * Time.deltaTime;
                bird.transform.position += velocity[t] * Time.deltaTime * Vector3.up ;
            } 
        }

    }
}
