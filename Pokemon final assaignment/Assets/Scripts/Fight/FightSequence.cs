using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class FightSequence : MonoBehaviour
{
    public Pokemon player;
    public Pokemon enemy;

    public Text playerPokemon;
    public Text playerLvl;
    public Slider playerHp;
    public Image playerFillImage;

    public Text enemyPokemon;
    public Text enemyLvl;
    public Slider enemyHp;
    public Image enemyFillImage;

    public Color fullHpColor = Color.green;
    public Color zeroHpColor = Color.red;

    public Text actionText;
    public Button move1;
    public Button move2;

    private bool fightLoopOnGoing = true;
    private bool isTyping = false;

    public float letterPause = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        player = PokemonSelect.player;
        enemy = PokemonFactory.CreateRandom();

        playerPokemon.text = player.name;
        playerLvl.text = "Lv" + player.level.ToString();
        playerHp.maxValue = player.maxHp;

        enemyPokemon.text = enemy.name;
        enemyLvl.text = "Lv" + enemy.level.ToString();
        enemyHp.maxValue = enemy.maxHp;

        playerHp.value = player.hp;
        enemyHp.value = enemy.hp;
        UpdateHp(player);
        UpdateHp(enemy);
        actionText.text = "";

        if (player.moves.Count == 2)
        {
            move1.gameObject.SetActive(true);
            move2.gameObject.SetActive(true);
            move1.GetComponentInChildren<Text>().text = player.moves[0].name;
            move2.GetComponentInChildren<Text>().text = player.moves[1].name;
        }
        else if (player.moves.Count == 1)
        {
            move1.gameObject.SetActive(true);
            move1.GetComponentInChildren<Text>().text = player.moves[0].name;
        }
        StartCoroutine(TextTyper(actionText, "Choose your next move"));
        Invoke("SetFightLoop", 1f);
    }

    // Update is called once per frame
    void UpdateHp(Pokemon pokemon)
    {
        if (pokemon == player)
        {
            if (player.hp <= 0)
            {
                playerHp.gameObject.SetActive(false);
            }
            playerHp.value = player.hp;
            float p = player.hp / player.maxHp;
            playerFillImage.color = Color.Lerp(zeroHpColor, fullHpColor, p);
        }
        else if(pokemon == enemy)
        {
            if (enemy.hp <= 0)
            {
                enemyHp.gameObject.SetActive(false);
            }
            enemyHp.value = enemy.hp;
            float e = enemy.hp / enemy.maxHp;
            enemyFillImage.color = Color.Lerp(zeroHpColor, fullHpColor, e);
        }
    }
    void SetFightLoop()
    {
        fightLoopOnGoing = false;
    }
    IEnumerator TextTyper(Text text, string message)
    {
        isTyping = true;
        text.text = "";
        foreach (char letter in message.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(letterPause);
        }
        yield return new WaitForSeconds(1);
        isTyping = false;
    }

    public void OnClicked(Button button)
    {
        if (fightLoopOnGoing == false)
        {
            StartCoroutine(AttackSequence(button));
        }
    }
    IEnumerator AttackSequence(Button button)
    {
        fightLoopOnGoing = true;
        StartCoroutine(TextTyper(actionText, (player.name + " used " + button.GetComponentInChildren<Text>().text + " and dealt " + player.Attack(enemy) + " damage to " + enemy.name).ToString()));
        UpdateHp(enemy);
        yield return new WaitWhile(() => isTyping == true);
        if (enemy.hp <= 0 && isTyping == false)
        {
            StartCoroutine(TextTyper(actionText, ("The wild " + enemy.name + " fainted").ToString()));
            yield return new WaitWhile(() => isTyping == true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        StartCoroutine(TextTyper(actionText, (enemy.name + " used " + RandomEnemyAttack() + " and dealt " + enemy.Attack(player) + " damage to " + player.name).ToString()));
        UpdateHp(player);
        yield return new WaitWhile(() => isTyping == true);
        if (player.hp <= 0 && isTyping == false)
        {
            StartCoroutine(TextTyper(actionText, (player.name + " fainted" + "\nYou are out of Pokémon's").ToString()));
            yield return new WaitWhile(() => isTyping == true);
            player.Restore();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        yield return new WaitWhile(() => isTyping == true);
        StartCoroutine(TextTyper(actionText, ("Choose your next move").ToString()));
        yield return new WaitWhile(() => isTyping == true);
        fightLoopOnGoing = false;
    }

string RandomEnemyAttack()
    {
        if (enemy.moves.Count == 2)
        {
            int ranNum = UnityEngine.Random.Range(0, 1);
            return enemy.moves[ranNum].name;
        }
        else
        {
            return enemy.moves[0].name;
        }
    }
}
