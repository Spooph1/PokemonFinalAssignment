using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

class PokemonFactory
{
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

                    return InstantiatePokemon(level, values);
                }
            }
        }

        //if we get here, it means we couldn't find the specified pokemon, so let's raise an exception
        throw new ArgumentException("Not a valid pokemon!");

    }

    static Pokemon InstantiatePokemon(int level, String[] values)
    {
        // Get the element from the pokemon info
        Elements element;
        switch (values[10])
        {
            case "Fire":
                element = Elements.Fire;
                break;
            case "Water":
                element = Elements.Water;
                break;
            case "Grass":
                element = Elements.Grass;
                break;
            default:
                // in case it's an element we don't know, let's just choose a random one
                Array allElements = Enum.GetValues(typeof(Elements));
                element = (Elements)UnityEngine.Random.Range(0, allElements.Length);
                break;
        }

        // create the moves for the pokemon
        List<Move> moves = new List<Move>();
        moves.Add(new Move(values[13]));
        if (values[14] != "")
            moves.Add(new Move(values[14]));

        // create and return the pokemon 
        return new Pokemon(values[2], level, int.Parse(values[4]), int.Parse(values[5]), int.Parse(values[3]), element, moves);
    }

    public static Pokemon CreateRandom()
    {
        // Read all lines in the Pokedex and store them into a file
        String[] lines = File.ReadAllLines(@"Assets\Pokedex.csv");
        // Create a Pokemon by picking a random line, starting from line 1 to skip the headers
        String randomLine = lines[UnityEngine.Random.Range(1, lines.Length)];
        // Split the line into values
        String[] values = randomLine.Split(',');
        // Initialize and return pokemon
        return InstantiatePokemon(UnityEngine.Random.Range(1, 11), values);
    }
}
