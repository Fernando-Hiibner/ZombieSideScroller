using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomizeZombieAppereance : MonoBehaviour
{

    public Material[] zombieMaterial; //Array com varios Materials com skins de zombie

    // Start is called before the first frame update
    void Start()
    {
        SkinnedMeshRenderer myRenderer = GetComponent<SkinnedMeshRenderer>(); //Esse objeto tem a capacidade de trocar os materials
        myRenderer.material = zombieMaterial[Random.Range(0,zombieMaterial.Length)]; //Pega um material numa posição aleatoria dentro da lista de Material
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
