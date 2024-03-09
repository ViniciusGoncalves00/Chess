using Pieces;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private delegate void Platform();
    private Platform _platform;

    [SerializeField] private Camera _camera;

    private static Vector3 _mousePosition;
    
    public static bool IsNodeSelected;
    public static Vector2Int NodeSelected;

    private void Start()
    {
        _platform = Desktop;
    }
        
    private void Update()
    {
        _platform.Invoke();
    }

    private void Desktop()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mousePosition = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(_mousePosition);

            if (Physics.Raycast(ray, out var hit) && hit.collider.CompareTag("Node"))
            {
                var pos = hit.transform.position;
                var x = Mathf.RoundToInt(pos.x);
                var y = Mathf.RoundToInt(pos.y);
                
                NodeSelected = new Vector2Int(x,y);
                IsNodeSelected = true;
            }
            else
            {
                IsNodeSelected = false;
            }
        }
    }
}