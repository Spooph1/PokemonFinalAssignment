using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonComponent : MonoBehaviour {

    public Pokemon pokemon;

	// Use this for initialization
	void Start () {
        pokemon = PokemonFactory.Create(2, "pikachu");
        // You can also crate random pokemons:
        //pokemon = PokemonFactory.CreateRandom();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
