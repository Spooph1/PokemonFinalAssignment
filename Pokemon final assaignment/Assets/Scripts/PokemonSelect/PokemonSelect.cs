using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PokemonSelect : MonoBehaviour
{
    public Text infotext;
    public float letterPause = 0.05f;

    static public Pokemon player;

    public InputField pokemonName;
    public InputField pokemonLvl;

    private bool readyForJourney = false;
    private bool hasEntered = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TextTyper(infotext, "Please write the name and lvl of the Pokémon you would like to choose and press enter when you are done"));
    }

    // Update is called once per frame
    void Update()
    {
        if(pokemonName.text != "" && pokemonLvl.text != "" && Input.GetKeyDown(KeyCode.Return) && hasEntered==false)
        {
            player = Create(int.Parse(pokemonLvl.text), pokemonName.text);
            hasEntered = true;
            StartCoroutine(TextTyper(infotext, "You chose a Lv" + pokemonLvl.text + " " + pokemonName.text + " to start your journey with"));
            Invoke("EndText", 5);
        }
        if (player != null && Input.GetKeyDown(KeyCode.Return) && readyForJourney == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public static Pokemon Create(int level, string name)
    {

        // Let's parse the Pokedex.csv file to find the requested pokemon!
        // Note that now you can easily add custom pokemons or modify existing ones! :)
        using (var reader = new StreamReader(@"Assets\Pokedex.csv"))
        {
            // Let's call readline to skip the first line (the one with the headers)
            reader.ReadLine();

            // We countinue reading one line at a time until we get to the end of the file
            while (!reader.EndOfStream)
            {
                // Each line is stored in the 'line' variable, and then split using the comma (CSV = Comma Separated Values)
                String line = reader.ReadLine();
                String[] values = line.Split(',');

                // Now we can check if the name required is a match for the current line
                if (values[2].ToLower() == name.ToLower())
                {
                    // We have found the pokemon requested! 
                    return PokemonFactory.Create(level, name);
                }
            }
        }

        //if we get here, it means we couldn't find the specified pokemon, so let's raise an exception
        throw new ArgumentException("Not a valid pokemon!");

    }

    IEnumerator TextTyper(Text text, string message)
    {
        text.text = "";
        foreach (char letter in message.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(letterPause);
        }
    }

    void EndText()
    {
        StartCoroutine(TextTyper(infotext, "Press enter to start your journey"));
        readyForJourney = true;
    }
}
