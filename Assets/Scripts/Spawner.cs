using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameManager _manager;
    private Coroutine _cor;
    private float _bounds = 2.3F;
    [SerializeField]
    private float _spawnTime;
    private float _sHeight, _sWidth;
    private float _uiOffsetY = 3.5F;
    private List<Flower> _flowers = new List<Flower>();

    // Start is called before the first frame update
    private void Start()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float camHalfHeight = _manager.Camera.orthographicSize;

        _sWidth = screenAspect * camHalfHeight;
        _sHeight = _manager.Camera.orthographicSize;
        _cor = StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (_flowers.Count  < 25)
        {
            Vector2 randPos = GetRandomPosition();

            if (_flowers.Count > 0)
            {
                if (CheckCollision(randPos))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        randPos = GetRandomPosition();
                        if (!CheckCollision(randPos))
                            break;
                    }
                }
            }
            GameObject obj = Instantiate(_manager.FlowerPrefab, randPos, Quaternion.identity);
            var flower = obj.GetComponent<Flower>();
            flower.DieEvent += _manager.OnFlowerDieEvent;
            flower.DieEvent += DeleteFromTheList;
            _flowers.Add(flower);
            yield return new WaitForSeconds(_spawnTime);
        }
    }


    public void Restart()
    {
        this.StopAllCoroutines();
        _cor = null;
        _flowers.ForEach(x => Destroy(x.gameObject));
        _flowers.Clear();
        _cor = StartCoroutine(Spawn());
    }

    private void DeleteFromTheList(Flower flower, double score)
    {
        _flowers.Remove(flower);
        flower.DieEvent -= DeleteFromTheList;
        flower.DieEvent -= _manager.OnFlowerDieEvent;
    }
    private Vector2 GetRandomPosition()
    {
       return new Vector2(Random.Range(-_sWidth + _bounds, _sWidth - _bounds), Random.Range(-_sHeight + _bounds, _sHeight -_uiOffsetY - _bounds));
    }
    private bool CheckCollision(Vector2 prefabPos)
    {
        foreach (var item in _flowers)
        {
            if (item == null)
                continue;

            if (Vector2.Distance(item.transform.position, prefabPos) < _bounds)
            {
                return true;
            }
        }
        return false;
    }

    public void SetSpawnTime(float time)
    {
        _spawnTime = time;
    }

    public void SetManager(GameManager manager)
    {
        _manager = manager;
    }

    private void OnDestroy()
    {
        StopCoroutine(_cor);
        _cor = null;
    }
}
