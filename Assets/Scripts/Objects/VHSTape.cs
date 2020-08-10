using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VHSTape : MonoBehaviour
{
    public GameController gameController;   // Finding GameController

    private DragAndDrop dragAndDrop;    // Reference to DragAndDrop class

    public float rewindTime;    // Yeaaaaaw. It's rewind time (time left in seconds)
    public string movieName;    // Name of movie on tape
    public string hoveringOver; // String to see what object VHS is currently being held over
    public GameObject hoveringOverGameObject;   // Reference to GameObject being held over
    public MovieBox movieBox;    // Reference to VHS's parent box

    [SerializeField]
    private Text nameText;  // UI Element for name on the box

    [HideInInspector]
    public bool isRewinding;    // Bool to tell if the VHS is currently rewinding

    private void Start()
    {
        nameText.text = movieName;

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>(); // Finding GameController

        rewindTime = Random.Range(gameController.lowestRewind, gameController.highestRewind);   // Setting range for rewind times

        dragAndDrop = GetComponent<DragAndDrop>();

        transform.position = new Vector3(transform.position.x, transform.position.y, -0.25f);
    }

    public void Rewind()
    {
        if(rewindTime > 0)
        {
            StartCoroutine(StartRewinding());
            /*
            isRewinding = true;
            rewindTime -= Time.deltaTime;
            isRewinding = false;
            */
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hoveringOver = collision.gameObject.tag;    // Setting what VHS is currently being held over
        hoveringOverGameObject = collision.GetComponent<GameObject>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hoveringOver = null;    // Clearing hoveringOver since it is not held over anything
        hoveringOverGameObject = null;
    }
    */

    public IEnumerator StartRewinding()
    {
        isRewinding = true;
        yield return new WaitForSeconds(gameController.rewindRate);
        rewindTime -= gameController.rewindRate;
        isRewinding = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Box")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
    }
}
