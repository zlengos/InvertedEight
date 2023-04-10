using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    private Camera _cam;

    [SerializeField] private Line _linePrefab;
    private Line _currentLine;

    public const float SpaceBeetwenDots = .1f;

    private Vector2 _startPosition;
    private List<Vector2> _positionsDrawen = new List<Vector2>();
    private const float _spaceBeetwenHalfs = .5f;

    [SerializeField] private GameObject _invertedEightDrawed;

    private bool _findHalf;
    private int _indexHalf;

    bool breakpointer = false;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        Vector2 mousePosition = _cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
            _currentLine = Instantiate(_linePrefab, mousePosition, Quaternion.identity);

        if (Input.GetMouseButtonUp(0))
            _startPosition = mousePosition;

        if (Input.GetMouseButton(0) && mousePosition.y > -4) // чтобы на кнопках не рисовало
        {
            _currentLine.SetPosition(mousePosition);
            _currentLine.AddPositionsToList(ref _positionsDrawen, mousePosition);
        }
    }

    public void IsCorrectInvertedEight() => ShowInDebugPositionsDrawen();

    public void Clear() => SceneManager.LoadScene(0);

    public bool ShowInDebugPositionsDrawen()
    {
        for (int i = 0; i < _positionsDrawen.Count; i++)
        {
            if (Vector2.Distance(_positionsDrawen[i], _startPosition) > _spaceBeetwenHalfs)
                breakpointer = true;

            if (breakpointer)
            {
                _findHalf = Vector2.Distance(_positionsDrawen[i], _startPosition) < _spaceBeetwenHalfs;
                if (_findHalf)
                {
                    _indexHalf = i;
                    break;
                }
            }
        }

        Debug.Log($"{_indexHalf}");

        if (!_findHalf)
            return false;

        List<Vector2> FirstHalf = _positionsDrawen.GetRange(0, _indexHalf);
        List<Vector2> SecondHalf = _positionsDrawen.GetRange(_indexHalf, _positionsDrawen.Count - _indexHalf);
        _positionsDrawen = null;
        Debug.Log($"{FirstHalf.Count} sec: {SecondHalf.Count}");


        Vector2 upFirstHalf = FirstHalf[FirstHalf.FindIndex(v => v.y == FirstHalf.Max(v => v.y))];

        Vector2 leftFirstHalf = FirstHalf[FirstHalf.FindIndex(v => v.x == FirstHalf.Min(m => m.x))];

        Vector2 downFirstHalf = FirstHalf[FirstHalf.FindIndex(v => v.y == FirstHalf.Min(m => m.y))];

        Vector2 rightFirstHalf = FirstHalf[FirstHalf.FindIndex(v => v.x == FirstHalf.Max(m => m.x))];

        Debug.Log($"верхн€€ половина левой: {upFirstHalf} лева€ {leftFirstHalf} нижн€€ {downFirstHalf} права€ {rightFirstHalf}");

        bool FirstCorrect = (upFirstHalf.y > _startPosition.y && upFirstHalf.x < _startPosition.x) &&
            (leftFirstHalf.y < upFirstHalf.y && leftFirstHalf.x < upFirstHalf.x) &&
            (downFirstHalf.y < leftFirstHalf.y && downFirstHalf.x > leftFirstHalf.x) &&
            (rightFirstHalf.y > downFirstHalf.y && rightFirstHalf.x > downFirstHalf.x);


        Vector2 upSecondHalf = SecondHalf[SecondHalf.FindIndex(v => v.y == SecondHalf.Max(m => m.y))];

        Vector2 rightSecondHalf = SecondHalf[SecondHalf.FindIndex(v => v.x == SecondHalf.Max(m => m.x))];

        Vector2 downSecondHalf = SecondHalf[SecondHalf.FindIndex(v => v.y == SecondHalf.Min(m => m.y))];

        Vector2 leftSecondHalf = SecondHalf[SecondHalf.FindIndex(v => v.x == SecondHalf.Min(m => m.x))]; ;

        bool SecondCorrect = (upSecondHalf.y > rightFirstHalf.y && upSecondHalf.x > rightFirstHalf.x) &&
            (rightSecondHalf.y < upSecondHalf.y && rightSecondHalf.x > upSecondHalf.x) &&
            (downSecondHalf.y < rightSecondHalf.y && downSecondHalf.x < rightSecondHalf.x) &&
            (leftSecondHalf.y > downSecondHalf.y && leftSecondHalf.x < downSecondHalf.x);


        Debug.Log($"верхн€€ половина правой: {upSecondHalf} лева€ {rightSecondHalf} нижн€€ {downSecondHalf} права€ {leftSecondHalf}");

        Debug.Log($"FindHalf = {_findHalf}, FirstCorrect = {FirstCorrect}, SecondCorrect = {SecondCorrect}, indexHalf = {_indexHalf}");

        if (FirstCorrect && SecondCorrect)
        {
            _invertedEightDrawed.SetActive(true);
            return true;
        }
        else
            return false;
    }
}
