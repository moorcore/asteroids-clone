﻿using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ProjBehaviour ProjPrefab;
    public Transform LaunchOffset;
    public Transform Spaceship;

    public float movementSpeed = 6.0f;
    public float hitpoints;
    public float maxHitpoints = 1.0f;
    public float invulnerabilityTime = 2.0f;

    public bool CanDash = true;
    private float dashingPower = 24.0f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1.0f;

    [SerializeField]
    private TrailRenderer trailRenderer;

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();  
        hitpoints = maxHitpoints;
    }

    private void Update()
    {
        var horizontalMovement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(horizontalMovement, 0, 0) * Time.deltaTime * movementSpeed;

        var verticalMovement = Input.GetAxis("Vertical");
        transform.position += new Vector3(0, verticalMovement, 0) * Time.deltaTime * movementSpeed;

        if (Input.GetButtonDown("Jump") && CanDash)
        {
            StartCoroutine(Dash());
            FindObjectOfType<AudioManager>().Play("dash");
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FindObjectOfType<AudioManager>().Play("projSound");

            GameObject proj = ProjObjectPool.instance.GetPooledObject();
            if (proj != null)
            {
                proj.transform.position = LaunchOffset.position;
                proj.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            
        }
    }

    private void OnEnable()
    {
        gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
        Invoke(nameof(TurnOnCollisions), invulnerabilityTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Threat")
        {
            PlayerTakeHit(1);
        }
    }

    private void TurnOnCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void PlayerTakeHit(float damage)
    {
        hitpoints -= damage;

        if (hitpoints > 0)
        {
            FindObjectOfType<AudioManager>().Play("shipdamaged");
        }

        if (hitpoints <= 0)
        {
            FindObjectOfType<AudioManager>().Play("shipdestroyed");
            gameObject.SetActive(false);
            FindObjectOfType<GameManager>().PlayerDestroyed();
            CanDash = true;
        }
    }

    private IEnumerator Dash()
    {
        CanDash = false;
        var originalMovespeed = movementSpeed;

        movementSpeed += dashingPower; 
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);

        trailRenderer.emitting = false;
        movementSpeed = originalMovespeed;
        yield return new WaitForSeconds(dashingCooldown);
        CanDash = true;
    }
}