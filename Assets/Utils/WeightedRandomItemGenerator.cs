using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeightedRandomItemGenerator<T>
{
    private SortedList<float, T> _items;
    private float _totalWeight;

    public WeightedRandomItemGenerator()
    {
        _items = new SortedList<float, T>();
        _totalWeight = 0f;
    }
    
    public T GetRandomItem()
    {
        if (_items.Count == 0)
            throw new InvalidOperationException("No items have been added to the generator.");

        float randomWeight = Random.Range(0f, _totalWeight);
        int index = BinarySearchForWeight(randomWeight);
        return _items.Values[index];
    }

    public void AddItem(T item, float weight)
    {
        if (weight <= 0f)
            throw new ArgumentException("Weight must be greater than 0");

        _totalWeight += weight;
        _items.Add(_totalWeight, item);
    }
    
    private int BinarySearchForWeight(float targetWeight)
    {
        int left = 0;
        int right = _items.Count - 1;

        while (left <= right)
        {
            int middle = left + (right - left) / 2;
            float middleWeight = _items.Keys[middle];

            if (Math.Abs(middleWeight - targetWeight) < 0.1)
            {
                return middle;
            }

            if (middleWeight < targetWeight)
            {
                left = middle + 1;
            }
            else
            {
                right = middle - 1;
            }
        }

        return left;
    }
}
