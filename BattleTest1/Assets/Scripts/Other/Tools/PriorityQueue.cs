using System;
using System.Collections.Generic;

public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private List<(TElement Element, TPriority Priority)> data;

    public PriorityQueue()
    {
        data = new List<(TElement, TPriority)>();
    }

    // ��Ҹ� �켱������ ���� �߰�
    public void Enqueue(TElement element, TPriority priority)
    {
        data.Add((element, priority));
        int childIndex = data.Count - 1; // �������� �߰��� ���
        while (childIndex > 0)
        {
            int parentIndex = (childIndex - 1) / 2;
            if (data[childIndex].Priority.CompareTo(data[parentIndex].Priority) >= 0)
                break;

            var tmp = data[childIndex];
            data[childIndex] = data[parentIndex];
            data[parentIndex] = tmp;

            childIndex = parentIndex;
        }
    }

    // �켱������ ���� ���� ��� ���� �� ��ȯ
    public TElement Dequeue()
    {
        if (data.Count == 0)
            throw new InvalidOperationException("The queue is empty");

        int lastIndex = data.Count - 1;
        var frontItem = data[0]; // �ֿ켱 ���
        data[0] = data[lastIndex];
        data.RemoveAt(lastIndex);

        --lastIndex;
        int parentIndex = 0;
        while (true)
        {
            int childIndex = parentIndex * 2 + 1;
            if (childIndex > lastIndex)
                break;

            int rightChild = childIndex + 1;
            if (rightChild <= lastIndex && data[rightChild].Priority.CompareTo(data[childIndex].Priority) < 0)
                childIndex = rightChild;

            if (data[parentIndex].Priority.CompareTo(data[childIndex].Priority) <= 0)
                break;

            var tmp = data[parentIndex];
            data[parentIndex] = data[childIndex];
            data[childIndex] = tmp;

            parentIndex = childIndex;
        }
        return frontItem.Element;
    }

    // ���� ť�� ��� ����
    public int Count
    {
        get { return data.Count; }
    }

    // �켱������ ���� ���� ��� ��ȯ (�������� ����)
    public TElement Peek()
    {
        if (data.Count == 0)
            throw new InvalidOperationException("The queue is empty");

        return data[0].Element;
    }
}
