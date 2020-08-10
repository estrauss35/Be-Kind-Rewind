using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieBox : MonoBehaviour
{
    public GameController gameController;
    public GameObject VHSPrefab;    // Prefab for instantiating our random VHS Tape

    private DragAndDrop dragAndDrop;    // Reference to DragAndDrop class

    public string movieName;    // Name of movie that belongs in the box
    public string hoveringOver; // String to see what object VHS is currently being held over
    public bool currentlyOpen = false;  // Bool for if the box is currently open (no gameobject in box)
    public bool goodToGo = false;   // Bool to determine whether the box is good to be dropped in the bin

    public GameObject currentTape;  // Gameobject that is currently in the box

    [SerializeField]
    private Text nameText;  // UI Element for name on the box

    private bool beenOpened = false;    // Bool for if the box has been opened in the past
    private bool correctTape = true;    // Bool for if the correct tape is in the box

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();     // Finding GameController
        nameText.text = movieName;  // Setting box's movie

        // Creating VHS tape gameobject
        currentTape = Instantiate(VHSPrefab, transform.position, Quaternion.identity);
        currentTape.GetComponent<VHSTape>().movieName = movieName;
        currentTape.GetComponent<VHSTape>().movieBox = this;
        currentTape.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(currentTape != null)
        {
            goodToGo = (correctTape && currentTape.GetComponent<VHSTape>().rewindTime <= 0 && beenOpened && !currentlyOpen);  // Checking if box is good to be submitted
        }
        
        if (currentlyOpen)
        {
            gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            gameObject.GetComponent<Collider2D>().isTrigger = false;
        }
        try
        {
            var _checker = currentTape;
        }
        catch (MissingReferenceException)
        {
            currentTape = null;
        }
    }

    // Function to switch VHS's activeness
    public void SetTapeActive(bool active)
    {
        currentTape.gameObject.SetActive(active);
    }

    // Will set VHS to active and have it appear next to the box
    public void OpenBox()
    {
        audioSource.Play();
        currentTape.transform.position = new Vector3(transform.position.x, transform.position.y, -0.25f);    // Tape will appear next to open box
        currentlyOpen = true;
        SetTapeActive(true);

        if (!beenOpened) { beenOpened = true; }     // Box has now been opened at some point
    }

    // Will set VHS to inactive and update Box's currentTape
    public void CloseBox(GameObject newTape)
    {
        audioSource.Play();
        currentTape = newTape;  // The currentTape is whatever tape has been passed in the function
        newTape.GetComponent<VHSTape>().movieBox = this;

        // Set correct tape to true if the tape's name matches the box's name
        if (currentTape.GetComponent<VHSTape>().movieName == movieName)
        {
            correctTape = true;
        }
        else
        {
            correctTape = false;
        }

        SetTapeActive(false);   // Make tape invisible, since it is now "inside" the box
        currentlyOpen = false;  // Box is now currently closed
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hoveringOver = collision.gameObject.tag;    // Setting what MovieBox is currently being held over
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hoveringOver = null;    // Clearing hoveringOver since it is not held over anything
    }
    */
}
