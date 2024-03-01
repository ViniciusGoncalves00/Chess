using Pieces;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private delegate void Platform();
    private Platform _platform;

    [SerializeField] private Camera _camera;
    
    public static Vector3 MousePosition;
    
    public static bool IsNodeSelected;
    public static Vector2Int NodeSelected;

    private void Start()
    {
        var platform = Application.platform;

        _platform = platform switch
        {
            RuntimePlatform.WindowsEditor => Desktop,
            RuntimePlatform.WindowsPlayer => Desktop,
            RuntimePlatform.Android => Mobile,
            _ => Desktop
        };
    }
        
    private void Update()
    {
        _platform.Invoke();
    }

    private void Desktop()
    {
        MousePosition = Input.mousePosition;
        var ray = _camera.ScreenPointToRay(MousePosition);

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

    private void Mobile()
    {
        
    }
}