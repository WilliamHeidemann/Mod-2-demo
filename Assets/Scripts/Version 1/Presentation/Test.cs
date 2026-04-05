using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Version_1;
using Version_1.Domain;
using Version_1.Presentation;

public class Test : MonoBehaviour
{
    private Segment[] _segments;
    private GameObject _gameObject;
    private int _index;
    private Factory _factory;
    [SerializeField] private SegmentFactoryData _segmentFactoryData;
    
    
    private void Start()
    {
        _segments = Generator.CenterCube.Translate(Direction.Up).GetAllStates().ToArray();
        print($"Created {_segments.Length} segments.");
        _factory = new Factory(_segmentFactoryData);
        _gameObject = _factory.SegmentToGameObject(_segments[0]);
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Next();
        }
    }
    
    private void Next()
    {
        _index = (_index + 1) % _segments.Length;
        Destroy(_gameObject);
        _gameObject = _factory.SegmentToGameObject(_segments[_index]);
        print($"Index {_index} created.");
    }
}
