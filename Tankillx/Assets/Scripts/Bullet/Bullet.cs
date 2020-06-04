using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;

    public GameObject RedScore;
    public GameObject BlueScore;
    public ParticleSystem particles;

    public float speed = 3;
    public int ticks = 0;

    private int _ticks = 0;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        RedScore = GameObject.Find("RedScore");
        BlueScore = GameObject.Find("BlueScore");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = speed * (rb.velocity.normalized);
        _ticks++;
        if (_ticks == ticks) Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            GameObject.Find("Explosion").GetComponent<AudioSource>().Play();

            ParticleSystem explosionEffect = Instantiate(particles) as ParticleSystem;
            explosionEffect.transform.position = transform.position;
            explosionEffect.Play();
            Destroy(explosionEffect.gameObject, 5f);
                
            if (col.gameObject.name.Equals("TankRed(Clone)"))
            {
                BlueScore.gameObject.SetActive(false);
                TMPro.TextMeshProUGUI Text = BlueScore.GetComponent<TMPro.TextMeshProUGUI>();
                Text.text = (int.Parse(Text.text) + 1).ToString();
                BlueScore.gameObject.SetActive(true);
            }
            else if (col.gameObject.name.Equals("TankBlue(Clone)"))
            {
                RedScore.gameObject.SetActive(false);
                TMPro.TextMeshProUGUI Text = RedScore.GetComponent<TMPro.TextMeshProUGUI>();
                Text.text = (int.Parse(Text.text) + 1).ToString();
                RedScore.gameObject.SetActive(true);
            }
            col.gameObject.GetComponent<TankMovement>().isDead = true;
            Destroy(col.gameObject);
            Destroy(this.gameObject);
        }
        else if (col.gameObject.CompareTag("Pickup"))
        {
            Destroy(col.gameObject);
        }
    }
}
