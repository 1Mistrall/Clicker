using UnityEngine;
using System;


public class Flower : MonoBehaviour
{
    public AnimationClip AnimationClip;
    public event Action<Flower, double> DieEvent;

    private Animation _animation;
    private SpriteRenderer _spriteRender;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _spriteRender = GetComponent<SpriteRenderer>();

    }
    // Start is called before the first frame update
    void Start()
    { 
    _animation.Play();
    }

    private void OnCreated()
    {

    }
    private void OnMouseDown()
    {
        double scoreTime = 5-Math.Round(_animation["Growing"].time, 2);
        DieEvent(this, scoreTime);
        Destroy(gameObject);
    }

    private void Destroy()
    {
        DieEvent(this, 0);
        Destroy(gameObject);
    }

    void Update()
    {
        
    }
}
