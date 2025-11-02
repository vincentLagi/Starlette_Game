using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Wire : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private string dataType;
    private Image _image;
    private Canvas _canvas;
    private bool _isDraggedStart = false;
    public bool isLeftWire;
    public bool isSuccess = false;
    private WireTask _wireTask;
    
    [SerializeField] private RectTransform wireRectTransform;
    [SerializeField] private Image wireImage;
    private Vector2 initialWireSize;

    public void OnDrag(PointerEventData eventData) { }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isLeftWire) { return; }
        if (isSuccess) { return; }
        _isDraggedStart = true;
        _wireTask.currentDraggedWire = this;
        
        wireImage.enabled = true;
        initialWireSize = wireRectTransform.sizeDelta;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_wireTask.currentHoveredWire != null)
        {
            Debug.Log(dataType);
            Debug.Log(_wireTask.getValueByDataType(dataType).ToString());
            Debug.Log(_wireTask.currentHoveredWire.GetComponentInChildren<TMPro.TextMeshProUGUI>().text);
            if (_wireTask.getValueByDataType(dataType).ToString() == _wireTask.currentHoveredWire.GetComponentInChildren<TMPro.TextMeshProUGUI>().text && !_wireTask.currentHoveredWire.isLeftWire)
            {
                isSuccess = true;
                _wireTask.currentHoveredWire.isSuccess = true;
            }
        }
        _isDraggedStart = false;
        _wireTask.currentDraggedWire = null;
        
        if (!isSuccess)
        {
            wireImage.enabled = false;
        }
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
        _canvas = GetComponentInParent<Canvas>();
        _wireTask = GetComponentInParent<WireTask>();
        
        if (wireRectTransform == null)
        {
            wireRectTransform = new GameObject("Wire").AddComponent<RectTransform>();
            wireRectTransform.SetParent(transform);
            wireRectTransform.localPosition = Vector3.zero;
            wireRectTransform.localScale = Vector3.one;
            wireImage = wireRectTransform.gameObject.AddComponent<Image>();
            wireImage.sprite = _image.sprite;
        
            wireImage.color = _image.color;
        
            wireImage.type = _image.type;
            wireImage.enabled = false;
        }
        
        Debug.Log(_wireTask);
    }

    void Update()
    {
        if (_isDraggedStart)
        {
            Vector2 movePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                Input.mousePosition,
                _canvas.worldCamera,
                out movePos
            );
            
            Vector2 localMousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform as RectTransform,
                Input.mousePosition,
                _canvas.worldCamera,
                out localMousePos
            );
            
            Vector2 direction = localMousePos;
            float distance = direction.magnitude;
            
            wireRectTransform.sizeDelta = new Vector2(distance, 5f);
            wireRectTransform.pivot = new Vector2(0, 0.5f);
            wireRectTransform.localPosition = new Vector3(20f,0,0);
            wireRectTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        }
        else if (!isSuccess)
        {
            wireImage.enabled = false;
        }

        bool isHovered = RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Input.mousePosition, _canvas.worldCamera);
        if (isHovered)
        {
            _wireTask.currentHoveredWire = this;
        }
    }

    public void setDataType(string dataType)
    {
        this.dataType = dataType;
    }
}