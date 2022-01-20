using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeCollider : MonoBehaviour
{
    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider.size = _spriteRenderer.size;
    }
}
