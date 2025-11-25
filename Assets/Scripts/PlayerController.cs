using UnityEngine;

// Questo attributo assicura che ci sia sempre un Rigidbody2D sull'oggetto
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Impostazioni Movimento")]
    public float forwardSpeed = 5.0f;   // La potenza del motore
    public float turnSpeed = 200.0f;   // La velocità di rotazione in gradi al secondo

    private Rigidbody2D rb;
    private float forwardInput;
    private float turnInput;

    public GameObject bulletPrefab; // Prefab del proiettile da sparare

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update viene chiamato ogni frame: QUI leggiamo l'input del giocatore
    void Update()
    {
        // "Vertical" mappa W/S o Freccia Su/Giù
        forwardInput = Input.GetAxis("Vertical"); 

        // "Horizontal" mappa A/D o Freccia Sinistra/Destra
        turnInput = Input.GetAxis("Horizontal"); 

       if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            TeleportRandomly();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
           Shoot();
        }
    }


    void Shoot()
    {
        // Implementa qui la logica di sparo
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        Debug.Log("Sparo!");
    }
    // FixedUpdate viene chiamato a intervalli fissi di tempo: QUI applichiamo la fisica
    void FixedUpdate()
    {
        // 1. Gestione Rotazione
        // Usiamo MoveRotation per una rotazione fluida ma controllata
        if (turnInput != 0)
        {
            // Il meno (-) serve perché in Unity la rotazione positiva è antioraria, 
            // ma premendo destra vogliamo andare in senso orario.
            float rotationAmount = -turnInput * turnSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation + rotationAmount);
        }

        // 2. Gestione Spinta (Thrust)
        // Applichiamo forza solo se il giocatore preme "Avanti" (W o Freccia Su)
        // In Asteroids non esiste la retromarcia!
        if (forwardInput > 0)
        {
            // transform.up è la direzione "davanti" della navicella (la punta del triangolo)
            rb.AddForce(transform.up * forwardSpeed * forwardInput);
        }
       
    }

    // NUOVO: Funzione dedicata al teletrasporto
    void TeleportRandomly()
    {
        // 1. Calcoliamo i limiti dello schermo in coordinate di gioco
        // Mettiamo "10" come Z, ma essendo 2D va bene qualsiasi valore positivo davanti alla cam
        float distanceZ = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

        Vector2 screenBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, distanceZ));
        Vector2 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distanceZ));

        // 2. Generiamo coordinate casuali dentro quei limiti
        // Aggiungo un piccolo margine (es. 1.0f) per non apparire esattamente metà fuori dallo schermo
        float randomX = Random.Range(screenBottomLeft.x + 1f, screenTopRight.x - 1f);
        float randomY = Random.Range(screenBottomLeft.y + 1f, screenTopRight.y - 1f);

        // 3. Spostiamo la nave
        transform.position = new Vector3(randomX, randomY, 0);

        // OPZIONALE: Azzerare la velocità? 
        // In molti giochi, il teletrasporto ti ferma anche. Se vuoi farlo, togli il commento sotto:
        // rb.velocity = Vector2.zero;
    }

    
}