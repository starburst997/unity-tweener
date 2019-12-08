using System.Collections;
using Tween;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Tweener Test1;
    public Tweener Test2;
    public Tweener Test3;
    public Tweener Test4;

    private int _state;
    private bool _mouseDown;
    private bool _mouseDown2;

    private Coroutine _currentTween;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Left click
        if (Input.GetMouseButtonDown(0))
        {
            if (!_mouseDown)
            {
                _mouseDown = true;
                
                Test1.X(_state % 2 == 0 ? 5 : -5, 1.25f, Easing.ExpoOut, false);
                Test2.ScaleX(_state % 2 == 0 ? 2 : 1, 0.5f, Easing.BackOut, false);
                Test2.ScaleY(_state % 2 == 0 ? 2 : 1, 0.5f, Easing.BackOut, false);
                Test3.Rotation(_state % 2 == 0 ? 359.99f : 0, 1.0f, Easing.BounceOut, false);
                
                Test4.Y(_state % 2 == 0 ? -2 : 2, 1.25f, Easing.ExpoOut, false);
                Test4.ScaleX(_state % 2 == 0 ? 2 : 1, 0.5f, Easing.BackOut, false);
                Test4.ScaleY(_state % 2 == 0 ? 2 : 1, 0.5f, Easing.BackOut, false);

                _state++;
            }
        }
        else
        {
            _mouseDown = false;
        }
        
        // Right click
        if (Input.GetMouseButtonDown(1))
        {
            if (!_mouseDown)
            {
                _mouseDown = true;

                if (_currentTween != null) StopCoroutine(_currentTween);
                _currentTween = StartCoroutine(ComplexTween());
            }
        }
        else
        {
            _mouseDown = false;
        }
    }

    private IEnumerator ComplexTween()
    {
        float wait = 0.5f;
        Test2.ScaleX(2, wait, Easing.BackOut, false);
        Test2.ScaleY(2, wait, Easing.BackOut, false);
        
        yield return new WaitForSeconds(wait);

        wait = 2.0f;
        Test2.Rotation(720, wait, Easing.BounceOut, false);
        Test2.X(5, wait, Easing.BackIn, false);
        
        yield return new WaitForSeconds(wait);
        
        wait = 2.0f;
        Test2.X(-5, wait, Easing.BackInOut, false);
        Test2.ScaleX(1, wait, Easing.BackIn, false);
        Test2.ScaleY(1, wait, Easing.BackIn, false);
        
        yield return new WaitForSeconds(wait);
        _currentTween = null;
    }
}
