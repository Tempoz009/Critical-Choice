using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Items")]
    public GameObject[] goodItems;
    public GameObject[] badItems;

    [SerializeField] private Text _goodItemsCountText;
    [SerializeField] private Text _badItemsCountText;


    [Header("Bonuses")]
    [SerializeField] private int playerGoodItemCounter = 0;
    [SerializeField] private int enemyGoodItemCounter = 0;
    [SerializeField] private int playerBadItemCounter = 0;
    [SerializeField] private int enemyBadItemCounter = 0;
    [SerializeField] private int _ThreeItemsInARowBonus = 2;
    [SerializeField] private Text _messageText;
    [SerializeField] private bool canInspect = false;
    [SerializeField] private GameObject itemToInspect;


    [Header("Other")]
    public HealthManager playerHealthManager;
    public HealthManager enemyHealthManager;
    [SerializeField] private bool playerTurn = true;
    public int damageFactor;
    public int increaseFactor;

    public Animator bossAnimator;
    // public GameObject bossGameObject;

    // public bool isSitting = true;
    public GameObject bossModel;
    public GameObject boss;
    public GameObject player;

    public Text pressEscapeText;

    private void Awake()
    {
        goodItems = GameObject.FindGameObjectsWithTag("GoodItem");
        badItems = GameObject.FindGameObjectsWithTag("BadItem");
    }

    void Start()
    {
        Debug.Log($"Количество хороших предметов на столе: {goodItems.Length}");
        Debug.Log($"Количество плохих предметов на столе: {badItems.Length}");

        pressEscapeText.text = "";

        _goodItemsCountText.text = $"";
        _badItemsCountText.text = $"";

        damageFactor = 3; // goodItems.Length / badItems.Length * 4;
        increaseFactor = 3; // goodItems.Length / badItems.Length;

        DisplayItems();

        bossAnimator = bossModel.GetComponent<Animator>();

        if (bossAnimator == null)
        {
            Debug.LogError("Animator component is missing on the boss object.");
            return;
        }

        bossAnimator.Play("Sitting Idle"); // Запускаем анимацию Sitting Idle

        StartCoroutine(GameLoop());
    }
    void Update()
    {
        goodItems = GameObject.FindGameObjectsWithTag("GoodItem");
        badItems = GameObject.FindGameObjectsWithTag("BadItem");

        if (SettingsMenu.displayItems)
        {
            _goodItemsCountText.text = $"Remaining Good Items: {goodItems.Length}";
            _badItemsCountText.text = $"Remaining Bad Items: {badItems.Length}";
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator DisplayRemainingItems()
    {
        if (goodItems.Length > 0 || badItems.Length > 0)
        {
            goodItems = GameObject.FindGameObjectsWithTag("GoodItem");
            badItems = GameObject.FindGameObjectsWithTag("BadItem");

            Debug.Log($"Remaining Good Items: {goodItems.Length}");
            Debug.Log($"Remaining Bad Items: {badItems.Length}");
        }

        yield return new WaitForSeconds(0.7f);
    }

    public void DisplayItems()
    {
        string goodItemsNames = string.Empty;
        string badItemsNames = string.Empty;

        foreach (GameObject item in goodItems)
        {
            goodItemsNames += $"{item.name}\n";
        }
        Debug.Log($"Good Items:\n{goodItemsNames}");

        foreach (GameObject item in badItems)
        {
            badItemsNames += $"{item.name}\n";
        }
        Debug.Log($"Bad Items:\n{badItemsNames}");
    }

    IEnumerator GameLoop()
    {
        while (player != null || boss != null)
        {
            while (playerTurn != false)
            {
                // Ждем хода игрока
                yield return StartCoroutine(PlayerTurn());

                if (player == null)
                {
                    pressEscapeText.text = "Press Escape to go back to the main menu!";
                    break;
                }
            }

            // Передаем ход противнику
            bossAnimator.Play("Sitting Thumbs Up"); // Запускаем анимацию Sitting Thumbs Up
            yield return new WaitForSeconds(bossAnimator.GetCurrentAnimatorStateInfo(0).length); // Ждем, пока анимация завершится полностью
            yield return StartCoroutine(EnemyTurn());

            if (boss != null && goodItems.Length + badItems.Length > 0)
            {
                bossAnimator.Play("Sitting Idle"); // Запускаем анимацию Sitting Idle перед следующим ходом игрока
            }
            else if (boss == null)
            {
                bossAnimator.Play("Flying Back Death");
                AudioManager.Instance.PlaySFX("ShootSound");
                AudioManager.Instance.PlaySFX("Victory");
                pressEscapeText.text = "Press Escape to go back to the main menu!";
                break;
            }
        }
    }

    IEnumerator PlayerTurn()
    {
        // Проверяем наличие предметов перед ходом игрока
        if (CheckForDraw())
        {
            pressEscapeText.text = "Press Escape to go back to the main menu!";
            yield break; // Завершаем ход игрока
        }

        // Включить какой-то интерфейс для игрока и ожидать выбора предмета
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("GoodItem") || hit.collider.CompareTag("BadItem"))
            {
                Item itemScript = hit.collider.gameObject.GetComponent<Item>();

                if (itemScript != null)
                {
                    if (hit.collider.CompareTag("GoodItem"))
                    {
                        playerHealthManager.ModifyHealth(increaseFactor);

                        playerGoodItemCounter++;

                        if (playerGoodItemCounter >= 3)
                        {
                            string bonusMessage1 = $"You have 3 good items in a row!\nBonus: +{_ThreeItemsInARowBonus} health points";
                            StartCoroutine(DisplayMessage(bonusMessage1, 2f, Color.white));
                            playerHealthManager.ModifyHealth(_ThreeItemsInARowBonus); // Увеличиваем здоровье на 3
                            playerGoodItemCounter = 0;
                        }
                    }
                    else if (hit.collider.CompareTag("BadItem"))
                    {
                        playerHealthManager.ModifyHealth(-damageFactor);
                        playerGoodItemCounter = 0;
                        playerBadItemCounter++;

                        if (playerBadItemCounter >= 2)
                        {
                            canInspect = true;
                            itemToInspect = hit.collider.gameObject;
                        }

                        AudioManager.Instance.PlaySFX("VeryNedovolny");
                    }

                    Debug.Log($"Выбранный игроком предмет: {hit.collider.gameObject}");
                    Destroy(hit.collider.gameObject);
                }

                playerTurn = false;
            }
            else
            {
                Debug.Log("Invalid selection: You can only remove objects by directly pointing at them!");
            }
        }

        yield return new WaitForSeconds(1f);

        if (canInspect)
        {
            StartCoroutine(InspectItem());
        }
    }

    IEnumerator EnemyTurn()
    {

        if (CheckForDraw())
        {
            pressEscapeText.text = "Press Escape to go back to the main menu!";
            yield break; // Завершаем ход врага
        }

        List<GameObject> remainingItems = new List<GameObject>();
        remainingItems.AddRange(goodItems);
        remainingItems.AddRange(badItems);

        if (remainingItems.Count > 0)
        {
            GameObject randomItem = remainingItems[Random.Range(0, remainingItems.Count)];
            remainingItems.Remove(randomItem);

            Item itemScript = randomItem.GetComponent<Item>();

            if (itemScript != null)
            {
                if (randomItem.CompareTag("GoodItem"))
                {
                    enemyHealthManager.ModifyHealth(increaseFactor);

                    enemyGoodItemCounter++;

                    if (enemyGoodItemCounter >= 3)
                    {
                        string bonusMessage2 = $"Boss has 3 good items in a row!\nBonus: +{_ThreeItemsInARowBonus} health points";
                        StartCoroutine(DisplayMessage(bonusMessage2, 2f, Color.white));
                        enemyHealthManager.ModifyHealth(_ThreeItemsInARowBonus); // Увеличиваем здоровье на 3
                        enemyGoodItemCounter = 0;
                    }
                }
                else if (randomItem.CompareTag("BadItem"))
                {
                    enemyHealthManager.ModifyHealth(-damageFactor);

                    enemyGoodItemCounter = 0;

                    AudioManager.Instance.PlaySFX("Vorchyn");
                }
            }

            Debug.Log($"Выбранный противником предмет: {randomItem}");
            Destroy(randomItem);

            remainingItems.Clear();
            remainingItems.AddRange(goodItems);
            remainingItems.AddRange(badItems);
        }
        else
        {
            if (playerHealthManager.HealthPoints > 0 && enemyHealthManager.HealthPoints > 0)
            {
                Debug.Log("It's a draw!"); // Ничья
                _messageText.text = "It's a draw!";
                bossAnimator.StopPlayback();

            }
            else
            {
                Debug.Log("Random item is null!");
            }
        }

        playerTurn = true;

        yield return null;
    }


    IEnumerator DisplayMessage(string message, float duration, Color mesColor)
    {
        _messageText.text = message; // Устанавливаем текст сообщения
        _messageText.color = mesColor;
        yield return new WaitForSeconds(duration); // Ждем указанное количество секунд
        _messageText.text = ""; // Скрываем сообщение
    }

    IEnumerator InspectItem()
    {
        canInspect = false; // Запрещаем дальнейший подсмотр
        string bonusMessage5 = $"You can inspect the quality of any item!"; // Показываем сообщение о возможности подсмотра
        StartCoroutine(DisplayMessage(bonusMessage5, 2f, Color.white));

        while (!Input.GetMouseButtonDown(1)) // Ждем нажатия кнопки для подсмотра
        {
            yield return null;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("GoodItem"))
            {
                _messageText.color = Color.green;
                _messageText.text = "Good item!";
                _messageText.color = Color.white;
                Debug.Log($"Inspected item: {hit.collider.gameObject.name}"); // Показываем имя предмета, на который наведен курсор
            
                AudioManager.Instance.PlaySFX("Povezlo");
            }
            else if (hit.collider.CompareTag("BadItem"))
            {
                _messageText.color = Color.red;
                _messageText.text = "Bad item!";
                _messageText.color = Color.white;
                Debug.Log($"Inspected item: {hit.collider.gameObject.name}");

                AudioManager.Instance.PlaySFX("SoundNeudachi");
            }
        }

        if (itemToInspect != null)
        {
            Destroy(itemToInspect);
            itemToInspect = null;
        }

        yield return new WaitForSeconds(2f);

        // Скрываем сообщение
        _messageText.text = "";
    }

    private bool CheckForDraw()
    {
        if (goodItems.Length == 0 && badItems.Length == 0 && playerHealthManager.HealthPoints > 0 && enemyHealthManager.HealthPoints > 0)
        {
            // Debug.Log("It's a draw!"); // Ничья
            _messageText.text = "It's a draw!";
            
            AudioManager.Instance.PlaySFX("Draw");
            
            return true;
        }

        return false;
    }
}