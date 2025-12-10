using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float speed = 5f;
    public float maxAngularVelocity = 50f;

    public enum Type {Big, Medium, Small};// 0 = grande, 1 = medio, 2 = piccolo

    public Type asteroidType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // Spinta in una direzione casuale
        rb.AddForce(Random.insideUnitCircle.normalized * speed);
        // Rotazione casuale
        rb.angularVelocity = Random.Range(-maxAngularVelocity, maxAngularVelocity);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collisione con: " + collision.gameObject.name);
        Debug.Log("Tag dell'oggetto: " + collision.gameObject.tag);
        // Controlla se l'asteroide è stato colpito da un proiettile
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if ( asteroidType == Type.Big ){ // Se l'asteroide non è il più piccolo
                GameManager.Instance.SpawnAsteroid(transform.position, Type.Medium); // Asteroide medio
                GameManager.Instance.SpawnAsteroid(transform.position, Type.Medium); // Asteroide piccolo
            }
            else if ( asteroidType == Type.Medium ){ // Se l'asteroide non è il più piccolo
                GameManager.Instance.SpawnAsteroid(transform.position, Type.Small); // Asteroide piccolo
                GameManager.Instance.SpawnAsteroid(transform.position, Type.Small); // Asteroide piccolo
            }
           
            // Distruggi l'asteroide
            Destroy(gameObject);
            // Distruggi il proiettile
            Destroy(collision.gameObject);
            // Aggiungi qui eventuali effetti sonori o di particelle
            
            //instanzia i due asteroidi più piccoli se la dimensione lo permette

        }
    }
}
