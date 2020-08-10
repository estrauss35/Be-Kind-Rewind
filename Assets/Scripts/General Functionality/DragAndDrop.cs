using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public GameController gameController;   // Finding GameController

    private bool isDragging;    // Bool to determine whether the item is being dragged

    public VHSTape VHS;     // Reference to VHS (if there is one)
    public MovieBox movieBox;    // Reference to Box (if there is one)

    private bool isVHS; // Bool to know if this item is a VHSTape
    private bool isMovieBox;    // Bool to know if this is a MovieBox

    public string hoveringOver;    // Reference to what item is being held over
    public GameObject hoveringOverGameObject;   // Reference to GameObject being held over

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>(); // Finding GameController

        // Determining whether this item is a VHSTape or a MovieBox
        if(gameObject.GetComponent<VHSTape>() != null)
        {
            VHS = gameObject.GetComponent<VHSTape>();
            isVHS = true;

            isMovieBox = false;
        }
        else if(gameObject.GetComponent<MovieBox>() != null)
        {
            movieBox = GetComponent<MovieBox>();
            isMovieBox = true;

            isVHS = false;
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
    }

    private void OnMouseUp()
    {
        if(hoveringOver == "GoodBin")
        {
            gameController.ProcessPoints(gameObject);
            if (isMovieBox)
            {
                gameController.movieBoxes.Remove(gameObject);
                if (!gameObject.GetComponent<MovieBox>().currentlyOpen)
                {
                    Destroy(gameObject.GetComponent<MovieBox>().currentTape);
                }
                else
                {
                    movieBox.GetComponent<MovieBox>().currentTape.GetComponent<VHSTape>().movieBox = null;
                    Destroy(gameObject.GetComponent<MovieBox>().currentTape);
                }
            }
            else if (isVHS)
            {
                gameObject.GetComponent<VHSTape>().movieBox.GetComponent<MovieBox>().currentTape = null;
                gameController.movieBoxes.Remove(gameObject.GetComponent<VHSTape>().movieBox.GetComponent<MovieBox>().gameObject);
                Destroy(gameObject.GetComponent<VHSTape>().movieBox.GetComponent<MovieBox>().gameObject);
            }
            hoveringOverGameObject.GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
        else if(hoveringOver == "Box")
        {
            if (isVHS)
            {
                hoveringOverGameObject.GetComponent<MovieBox>().CloseBox(VHS.gameObject);
            }
        }
        isDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.Translate(mousePos);
        }
        if(isVHS)
        {
            if (!isDragging && hoveringOver == "Rewinder")
            {
                if (VHS.rewindTime > 0 && !VHS.isRewinding)
                {
                    if (hoveringOverGameObject.GetComponent<Rewinder>().vhsTape == null || hoveringOverGameObject.GetComponent<Rewinder>().vhsTape == gameObject)
                    {
                        hoveringOverGameObject.GetComponent<AudioSource>().Play();
                        VHS.Rewind();
                        hoveringOverGameObject.GetComponent<Rewinder>().isRewinding = true;
                        hoveringOverGameObject.GetComponent<Rewinder>().vhsTape = gameObject;
                    }
                }
                if (VHS.rewindTime <= 0)
                {
                    hoveringOverGameObject.GetComponent<Rewinder>().isRewinding = false;
                }
            }
        }
        if (isMovieBox)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isDragging && !movieBox.currentlyOpen)
            {
                movieBox.OpenBox();
            }
            if (!movieBox.currentlyOpen)
            {
                movieBox.currentTape.SetActive(false);
            }
        }
        if (hoveringOver == "GoodBin" && !isDragging)
        {
            gameController.ProcessPoints(gameObject);
            if (gameObject.GetComponent<MovieBox>())
            {
                if (!gameObject.GetComponent<MovieBox>().currentlyOpen)
                {
                    Destroy(gameObject.GetComponent<MovieBox>().currentTape);
                }
                else
                {
                    movieBox.GetComponent<MovieBox>().currentTape.GetComponent<VHSTape>().movieBox = null;
                    Destroy(gameObject.GetComponent<MovieBox>().currentTape);
                }
            }
            else
            {
                gameObject.GetComponent<VHSTape>().movieBox.GetComponent<MovieBox>().currentTape = null;
                gameController.movieBoxes.Remove(gameObject.GetComponent<VHSTape>().movieBox.GetComponent<MovieBox>().gameObject);
                Destroy(gameObject.GetComponent<VHSTape>().movieBox.GetComponent<MovieBox>().gameObject);
            }
            hoveringOverGameObject.GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
    }

    public bool isBeingDragged()
    {
        return isDragging;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hoveringOver = null;
        hoveringOverGameObject = null;

        hoveringOver = collision.gameObject.tag;    // Setting what MovieBox is currently being held over
        hoveringOverGameObject = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(hoveringOverGameObject != null && hoveringOverGameObject.tag == "Rewinder")
        {
            hoveringOverGameObject.GetComponent<Rewinder>().isRewinding = false;
            hoveringOverGameObject.GetComponent<Rewinder>().vhsTape = null;
        }
        
        hoveringOver = null;    // Clearing hoveringOver since it is not held over anything
        hoveringOverGameObject = null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        hoveringOver = collision.gameObject.tag;    // Setting what MovieBox is currently being held over
        hoveringOverGameObject = collision.gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
    }
}
