using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 direction;    
    Quaternion rotation;
    
    private List<Transform> segments = new List<Transform>();

    public Transform prefab;

    private int size = 2;
    public string tmp = "";
    
    public GameObject textPrefab;
    public GameObject textDeathPrefab;
    
    public CameraShake camShake;

    public CanvasGroup go;

    // Start is called before the first frame update
    void Start()
    {
        go.alpha = 0f;
        go.interactable = false;

        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (direction != Vector2.down)
            {
                direction = Vector2.up;
                rotation = Quaternion.Euler(180f, 0f, 0f);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (direction != Vector2.right)
            {
                direction = Vector2.left;
                rotation = Quaternion.Euler(180f, 0f, -90f);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (direction != Vector2.up)
            {
                direction = Vector2.down;
                rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (direction != Vector2.left)
            {
                direction = Vector2.right;
                rotation = Quaternion.Euler(0f, 0f, 90f);
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i= segments.Count-1; i>0; i--)
        {
            segments[i].position = segments[i - 1].position;
            segments[i].rotation = segments[i - 1].rotation;
        }
        
        transform.position = new Vector3(Mathf.Round(this.transform.position.x + direction.x),
                                         Mathf.Round(this.transform.position.y + direction.y),
                                         0.0f);
        transform.rotation = rotation;
    }

    private void GrowUp()
    {
        Transform seg = Instantiate(this.prefab);
        seg.position = segments[segments.Count - 1].position;

        segments.Add(seg);
    }

    public void ResetGame()
    {
        gameObject.GetComponent<Snake>().enabled = true;
        GameObject apple = GameObject.Find("Apple");
        apple.GetComponent<Apple>().enabled = true;
        apple.GetComponent<Apple>().RandomFoodPlacement();

        for (int i = 1; i <segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(this.transform);

        for (int i = 1; i < this.size; i++)
        {
            segments.Add(Instantiate(this.prefab));
        }
        
        this.transform.position = Vector2.zero;

        GameObject appleGO = GameObject.Find("Apple");
        int score = appleGO.GetComponent<Apple>().score = 0;
        appleGO.GetComponent<Apple>().scoreText.text = score.ToString();
    }

    private void ShowFloatingText()
    {   
        if (tmp == "apple")
        {
            Instantiate(textPrefab, transform.position, Quaternion.identity, transform);
        }else if (tmp == "death" || tmp == "wall")
        {
            Instantiate(textDeathPrefab);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Apple"))
        {
            tmp = "apple";
            ShowFloatingText();
            GrowUp();
        }
        else if (collision.CompareTag("Wall") || (collision.CompareTag("Player") && segments.Count > 2))
        {
            tmp = collision.CompareTag("Wall") ? "wall" : "death";
            ShowFloatingText();
            StartCoroutine(ShakeAndFade());
        }
    }

    IEnumerator ShakeAndFade()
    {
        gameObject.GetComponent<Snake>().enabled = false;
        GameObject.Find("Apple").GetComponent<Apple>().enabled = false;

        yield return StartCoroutine(camShake.Shake(.5f));
        yield return StartCoroutine(Fade(go, 1f, 1f));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = to;
        canvasGroup.interactable = true;
    }

}
