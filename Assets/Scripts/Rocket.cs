﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    [SerializeField]
    float explosionRadius;

    [SerializeField]
    int rocketDamage;

    [SerializeField]
    GameObject explosion;

    bool collided;
    Vector3 vel;


    private void Start() {
        collided = false;
        //GetComponent<Rigidbody2D>().velocity = new Vector2(5.0f, 10.0f);
    }

    private void Update() {
        vel = GetComponent<Rigidbody2D>().velocity;
        transform.rotation = Quaternion.LookRotation(-vel);
        transform.Rotate(new Vector2(0, 90));
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collided && (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")) {
            GameObject.Find("Terrain").GetComponent<TerrainLoader>().removeVoxelsInRadius(collision.collider.transform.position, explosionRadius);
            GameObject expl = Instantiate(explosion, collision.collider.transform.position, Quaternion.Euler(0, 0, 0));

            List<GameObject> worms = GameObject.Find("Game").GetComponent<GameController>().getAllWorms();

            foreach(GameObject worm in worms) {
                float dx = worm.transform.position.x - collision.collider.transform.position.x;
                float dy = worm.transform.position.y - collision.collider.transform.position.y;

                if (dx * dx + dy * dy < explosionRadius * explosionRadius) {
                    worm.GetComponent<WormMovement>().takeDamage(rocketDamage);
                }
            }

            Destroy(expl, 0.5f);

            collided = true;
            Destroy(gameObject);
        }
    }
}
