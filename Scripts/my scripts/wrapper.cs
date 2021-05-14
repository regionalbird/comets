using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wrapper : MonoBehaviour
{
    /*
    Renderer[] renderers;
    private bool isWrappingX;
    private bool isWrappingY;

    Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckRenderers();
        ScreenWrap();
    }

    bool CheckRenderers()
    {
        foreach(var renderer in renderers)
        {
            // If at least one render is visible, return true
            if(renderer.isVisible)
            {
                return true;
            }
        }
 
    // Otherwise, the object is invisible
        return false;
    }

    void ScreenWrap()
    {
        
        var isVisible = CheckRenderers();
 
        if(isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }
 
        if(isWrappingX && isWrappingY) {
            return;
        }
         
        
 
        var cam = Camera.main;
        var viewportPosition = cam.WorldToViewportPoint(rigidbody.position);
        var newPosition = rigidbody.position;
    
        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;
    
            isWrappingX = true;
        }
    
        if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;
    
            isWrappingY = true;
        }
    
        rigidbody.position = newPosition;
    }
    */

    public bool wrapWidth = true;
    public bool wrapHeight = true;

    private Renderer _renderer;
    private Transform _transform;
    private Camera _camera;
    private Vector2 _viewportPosition;
    private bool isWrappingWidth;
    private bool isWrappingHeight;
    private Vector2 _newPosition;

    void Start () 
    {
        _renderer = GetComponent <Renderer> ();
        _transform = transform;
        _camera = Camera.main;
        _viewportPosition = Vector2.zero;
        isWrappingWidth = false;
        isWrappingHeight = false;
        _newPosition = _transform.position;
    }

    void LateUpdate () 
    {
        Wrap ();
    }

    private void Wrap ()
    {
        bool isVisible = IsBeingRendered ();

        if (isVisible)
        {
            isWrappingWidth = false;
            isWrappingHeight = false;
        }

        _newPosition = _transform.position;
        _viewportPosition = _camera.WorldToViewportPoint (_newPosition);

        if (wrapWidth)
        {
            if (!isWrappingWidth)
            {
                if (_viewportPosition.x > 1)
                {
                    _newPosition.x = _camera.ViewportToWorldPoint (Vector2.zero).x;
                    isWrappingWidth = true;
                }
                else if (_viewportPosition.x < 0)
                {
                    _newPosition.x = _camera.ViewportToWorldPoint (Vector2.one).x;
                    isWrappingWidth = true;
                }
            }
        }

        if (wrapHeight)
        {
            if (!isWrappingHeight)
            {
                if (_viewportPosition.y > 1)
                {
                    _newPosition.y = _camera.ViewportToWorldPoint (Vector2.zero).y;
                    isWrappingHeight = true;
                }
                else if (_viewportPosition.y < 0)
                {
                    _newPosition.y = _camera.ViewportToWorldPoint (Vector2.one).y;
                    isWrappingHeight = true;
                }
            }
        }

        _transform.position = _newPosition;
    } 

    private bool IsBeingRendered ()
    {
        if (_renderer.isVisible)
            return true;
        return false;
    }
}
