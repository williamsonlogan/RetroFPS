using System.Collections;
using System.Collections.Generic;
using System;

public class Heap<T> where T : IHeapItem<T> {

	T[] items;
	int currentSize;
	public int Count { get { return currentSize; } }

	public Heap(int maxHeapSize)
	{
		items = new T[maxHeapSize];
	}

	public void Add(T item)
	{
		item.HeapIndex = currentSize;
		items [currentSize] = item;
		SortUp (item);
		currentSize++;
	}

	public T RemoveFirst()
	{
		T firstItem = items [0];
		currentSize--;
		items [0] = items [currentSize];
		items [0].HeapIndex = 0;
		SortDown (items[0]);
		return firstItem;
	}

	public bool Contains(T item)
	{
		return Equals (items [item.HeapIndex], item);		
	}

	public void UpdateItem(T item)
	{
		SortUp (item);
	}

	void SortUp(T item)
	{
		int parentIndex = (item.HeapIndex - 1) / 2;

		while (true) {
			T parent = items [parentIndex];
			if (item.CompareTo (parent) > 0) {
				Swap (item, parent);
			} else {
				break;
			}

			parentIndex = (item.HeapIndex - 1) / 2;
		}
	}

	void SortDown(T item)
	{
		while (true) {
			
			int leftIndex = (item.HeapIndex * 2) + 1;
			int rightIndex = (item.HeapIndex * 2) + 2;
			int swapIndex = 0;

			if (leftIndex < currentSize) {
				swapIndex = leftIndex;

				if (rightIndex < currentSize) {
					if (items [leftIndex].CompareTo (items [rightIndex]) < 0)
						swapIndex = rightIndex;
				}
					
				if (item.CompareTo (items [swapIndex]) < 0)
					Swap (item, items [swapIndex]);
				else
					return;
			} else {
				return;
			}
		}
	}

	void Swap (T itemA, T itemB)
	{
		items [itemA.HeapIndex] = itemB;
		items [itemB.HeapIndex] = itemA;

		int itemAIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = itemAIndex;
	}
}

public interface IHeapItem<T> : IComparable<T>
{
	int HeapIndex { get; set; }
}
