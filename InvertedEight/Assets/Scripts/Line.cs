using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer _renderer;
    //public List<Vector2> _positionsDrawen = new List<Vector2>();

    public void SetPosition(Vector2 position)
    {
        if (!CanAddNewPosition(position)) return;

        _renderer.positionCount++;
        _renderer.SetPosition(_renderer.positionCount - 1, position);
    }

    public List<Vector2> ListPositionDrawen(Vector2 position, ref List<Vector2> listPositionDrawenn)
    {
        listPositionDrawenn.Add(position);
        return listPositionDrawenn;
    }


    public void AddPositionsToList(ref List<Vector2> position, Vector2 mousePosition)
    {
        position.Add(mousePosition);
    }

    private bool CanAddNewPosition(Vector2 position)
    {
        if (_renderer.positionCount == 0) 
            return true;

        return Vector2.Distance(_renderer.GetPosition(_renderer.positionCount - 1), position) > AppManager.SpaceBeetwenDots;
    }




}
