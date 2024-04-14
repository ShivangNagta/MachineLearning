using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class PipeSpawn : MonoBehaviour
{
    //[SerializeField] private GameObject bird;
    [SerializeField] private GameObject PipePrefab;
    [SerializeField] private GameObject population;
    [SerializeField] private GameObject birdPrefab;
    public GameObject pipeParentHolder;
    [SerializeField] private float pipeSpawnInterval = 2.0f;
    [SerializeField] private float pipeLeftSpeed = 2.0f;
    private float pipeSpawnCountDown ;
    private int pipeCount = 0;
    public TMP_Text scoreText;
    private int score;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //bird.transform.eulerAngles = Vector3.zero;
        Destroy(pipeParentHolder);
        pipeCount = 0;
        score = 0;
        scoreText.text = score.ToString();
        //var birdScript = bird.GetComponent<Control>();
        //birdScript.velocity = 0.0f;
        pipeParentHolder = new GameObject("pipeParentHolder");
        pipeParentHolder.transform.parent = this.transform;
        
}

    // Update is called once per frame
    void Update()
    {
        
     
        
        int child = population.transform.childCount;
        if (child == 0)
        {
            Start();
        }
            
        pipeSpawnCountDown -= Time.deltaTime;

        if (pipeSpawnCountDown <= 0)
        {
            
            pipeSpawnCountDown = pipeSpawnInterval;
            GameObject pipe = Instantiate(PipePrefab);
            pipe.transform.parent = pipeParentHolder.transform;
            
            
            pipe.transform.position += Vector3.up * Mathf.Lerp(-3.5f, 3.5f, Random.value);
            pipe.transform.name = (++pipeCount).ToString();
            
        }
        
        foreach (Transform pipe in pipeParentHolder.transform)
        {
            
            if (pipe.position.x <= -5.0f)
            {
                int pipeID = int.Parse(pipe.name);
                if (pipeID > score)
                {
                    score = pipeID;
                    scoreText.text = score.ToString();
                }
            }            
            if (pipe.position.x <= -11.0f)
                Destroy(pipe.GameObject());
        }
        
        
        
        
        pipeParentHolder.transform.position += pipeLeftSpeed * Time.deltaTime* Vector3.left ;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "bird")
        {
            Destroy(other.gameObject);
        }
    }
    
}
