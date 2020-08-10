using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float timeLimit;
    private float newTimeChallenge;

    public float totalPay;

    public int[] numBoxesToSpawn;
    public int gameOverNum;
    public float spawnTimer;

    [HideInInspector]
    public int numTapesProcessed;
    public float correctlyProcessed;
    public float incorrectlyProcessed;

    public int lowestRewind;
    public int highestRewind;
    public int rewinderPrice;
    public int clearPrice;
    public int speedPrice;
    public float rewindRate = 0.1f;

    private GameObject[] spawnPoints;
    private GameObject[] throwPoints;

    [SerializeField]
    private GameObject movieBoxPrefab;
    [SerializeField]
    private float throwStrength;
    private Text scoreText;
    private Text moneyText;
    private Text rewinderButtonText;
    private Text clearButtonText;
    private Text speedButtonText;
    [SerializeField]
    private GameObject[] rewinders;

    public List<GameObject> movieBoxes = new List<GameObject>();

    public bool spawnWithButton = true;

    private string[] movieNames =
    {
        "Predator", "Alien", "Back to the Future", "Ghostbusters", "Total Recall", "Gremlins", "The Thing", "Die Hard", "Blade Runner", "The Neverending Story"
    };

    // Start is called before the first frame update
    void Start()
    {
        newTimeChallenge = timeLimit;

        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        throwPoints = GameObject.FindGameObjectsWithTag("ThrowTarget");
        scoreText = GameObject.FindGameObjectWithTag("PointCounter").GetComponent<Text>();
        moneyText = GameObject.FindGameObjectWithTag("MoneyText").GetComponent<Text>();
        //rewinders = GameObject.FindGameObjectsWithTag("Rewinder");
        rewinderButtonText = GameObject.FindGameObjectWithTag("RewinderButton").GetComponent<Text>();
        rewinderButtonText.text = "Buy Rewinder (" + rewinderPrice + ")";

        clearButtonText = GameObject.FindGameObjectWithTag("ClearButton").GetComponent<Text>();
        clearButtonText.text = "Clear All Movies (" + clearPrice + ")";

        speedButtonText = GameObject.FindGameObjectWithTag("SpeedButton").GetComponent<Text>();
        speedButtonText.text = "Increase Rewind Speed (" + speedPrice + ")";

        for (int i = numBoxesToSpawn[0]; i < Random.Range(numBoxesToSpawn[0], numBoxesToSpawn[1]); i++)
        {
            SpawnNewBox();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(incorrectlyProcessed >= gameOverNum)
        {
            SceneManager.LoadScene(3, LoadSceneMode.Single);
        }
        if(timeLimit >= newTimeChallenge)
        {
            timeLimit = 0;
            numBoxesToSpawn[1] += 5;
            numBoxesToSpawn[0] += 2;
        }
        if(spawnTimer >= 10)
        {
            spawnTimer = 0;
            for(int i = numBoxesToSpawn[0]; i < Random.Range(numBoxesToSpawn[0], numBoxesToSpawn[1]); i++)
            {
                SpawnNewBox();
            }
        }
        if (spawnWithButton && Input.GetKeyDown(KeyCode.Q))
        {
            SpawnNewBox();
        }
        if(scoreText != null)
        {
            scoreText.text = "Score: " + correctlyProcessed;
        }
        if(moneyText != null)
        {
            moneyText.text = "Money: " + totalPay;
        }
        timeLimit += Time.deltaTime;
        spawnTimer += Time.deltaTime;
    }

    public void IncrementCorrect(float amount)
    {
        correctlyProcessed += amount;
        totalPay += amount * 5;
        numTapesProcessed++;
    }

    public void IncrementIncorrect()
    {
        incorrectlyProcessed++;
        totalPay -= 5;
        if(totalPay < 0) { totalPay = 0; }
        numTapesProcessed++;
    }

    public void ProcessPoints(GameObject submittedObject)
    {
        if (submittedObject.GetComponent<VHSTape>() != null)
        {
            if(submittedObject.GetComponent<VHSTape>().rewindTime <= 0)
            {
                IncrementCorrect(0.5f);
            }
            else
            {
                IncrementIncorrect();
            }
            
        }
        else if (submittedObject.GetComponent<MovieBox>() != null)
        {
            if (submittedObject.GetComponent<MovieBox>().goodToGo)
            {
                IncrementCorrect(1.0f);
            }
            else
            {
                IncrementIncorrect();
            }
            movieBoxes.Remove(submittedObject.gameObject);
        }
    }

    public void SpawnNewBox()
    {
        var _spawnIndex = Random.Range(0, spawnPoints.Length);
        var _nameIndex = Random.Range(0, movieNames.Length);
        var _throwIndex = Random.Range(0, throwPoints.Length);

        var _spawnPoint = spawnPoints[_spawnIndex];
        var _throwPoint = throwPoints[_throwIndex];

        var _movieBox = Instantiate(movieBoxPrefab, _spawnPoint.transform.position, Quaternion.identity);
        _movieBox.GetComponent<MovieBox>().movieName = movieNames[_nameIndex];

        var _throwDirection = (_throwPoint.transform.position - _movieBox.transform.position).normalized;
        _movieBox.GetComponent<Rigidbody2D>().AddForce(_throwDirection * throwStrength, ForceMode2D.Impulse);

        movieBoxes.Add(_movieBox);

        foreach(GameObject movie in movieBoxes)
        {
            Debug.Log(movie.name);
        }
        
    }

    public void BuyRewinder()
    {
        if(totalPay >= rewinderPrice)
        {
            rewinderPrice *= 2;
            foreach(GameObject rewinder in rewinders)
            {
                if(rewinder.activeSelf == false)
                {
                    totalPay -= rewinderPrice;
                    rewinder.SetActive(true);
                    break;
                }
            }
            rewinderButtonText.text = "Buy Rewinder (" + rewinderPrice + ")";
        }
    }

    public void ClearBoard()
    {
        if(totalPay >= clearPrice)
        {
            clearPrice *= 2;
            totalPay -= clearPrice;
            foreach(GameObject movieBox in movieBoxes)
            {
                movieBox.GetComponent<MovieBox>().currentTape.GetComponent<VHSTape>().movieBox = null;
                Destroy(movieBox.GetComponent<MovieBox>().currentTape);
                Destroy(movieBox);
            }
            movieBoxes.Clear();
            clearButtonText.text = "Clear All Movies (" + clearPrice + ")";
        }
    }

    public void IncreaseRewindSpeed()
    {
        if(totalPay >= speedPrice)
        {
            rewindRate += 0.2f;
        }
    }
}
