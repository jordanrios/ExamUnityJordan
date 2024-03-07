using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 originalScale;
    
    /*
    Los límites definidos con bound nos hacen falta debido a que el jugador se puede salir de la pantalla
    debido a que su rigidbody es quinemático, por lo que no se ve afectado por la gravedad ni puede colisionar
    con objetos estáticos.
    */
    [SerializeField] private float bound = 4.5f; // x axis bound 

    private Vector2 startPos; // Posición inicial del jugador


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position; // Guardamos la posición inicial del jugador
                                       
        // Guardar el tamaño original de la nave
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
       PlayerMovement();
    }

    void PlayerMovement()
    {
         float moveInput = Input.GetAxisRaw("Horizontal");
        // Controlaríamo el movimiento de la siguiente forma de no ser el rigidbody quinemático
        // transform.position += new Vector3(moveInput * speed * Time.deltaTime, 0f, 0f);

        Vector2 playerPosition = transform.position;
        // Mathf.Clamp nos permite limitar un valor entre un mínimo y un máximo
        playerPosition.x = Mathf.Clamp(playerPosition.x + moveInput * speed * Time.deltaTime, -bound, bound);
        transform.position = playerPosition;
    }

    public void ResetPlayer()
    {
        transform.position = startPos; // Posición inicial del jugador
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("powerUp")) // Si colisionamos con un powerUp
        {
            Destroy(collision.gameObject); // Lo destruimos
            GameManager.Instance.AddLife(); // Añadimos una vida
        }
        if (collision.CompareTag("powerUpLoseLife")) // Si colisionamos con un powerUp
        {
            Destroy(collision.gameObject); // Lo destruimos
            GameManager.Instance.LoseLifePULoseLife(); // Restamos Vida
        }

        if (collision.CompareTag("powerUpSize")) // Si colisionamos con un powerUp
        {
            Destroy(collision.gameObject); // Lo destruimos

            
            if(originalScale == transform.localScale)
            {
                Vector3 scaleX2 = new Vector3(transform.localScale.x * 2, transform.localScale.y, transform.localScale.z);
                // Aumentar el tamaño del player
                transform.localScale = scaleX2 ;
            }
            
            //Para corutina para volver a incrementar los 10seg
            StopAllCoroutines();
            
            // Iniciar la corrutina para volver a su tamaño original después de 3 segundos
            StartCoroutine(ReturnToOriginalSize());
            
        }
    }


    public IEnumerator ReturnToOriginalSize()
    {
        // Esperar 3 segundos
        yield return new WaitForSeconds(10f);
        // Volver a tamaño original
        transform.localScale = originalScale;
    }
}
