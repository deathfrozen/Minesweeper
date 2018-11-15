using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Minesweeper
{

    public class MyList<T> : IList<T>
    {
        public event EventHandler<EventArgs> ListChanged;
        List<T> collection;
        public int Count {
            get {
                return collection.Count;
            }
        }
        public T this[int index] {
            get {
                return collection[index];
            }
            set {
                collection[index] = value;
                ListChanged?.Invoke(this, new PropertyChangedEventArgs("List"));
            }
        }
        public bool IsReadOnly {
            get {
                return false;
            }
        }
        public MyList()
        {
            collection = new List<T>();
        }
        public void Add(T item)
        {
            collection.Add(item);
            ListChanged?.Invoke(this, new PropertyChangedEventArgs("List"));
        }
        public void AddRange(IEnumerable<T> items)
        {
            collection.AddRange(items);
            ListChanged?.Invoke(this, new PropertyChangedEventArgs("List"));
        }
        public bool Contains(T item)
        {
            return collection.Contains(item);
        }
        public void CopyTo(T[] array, Int32 arrayIndex)
        {
            collection.CopyTo(array, arrayIndex);
        }
        public bool Remove(T item)
        {
            bool result = collection.Remove(item);
            ListChanged?.Invoke(this, new PropertyChangedEventArgs("List"));
            return result;
        }
        public void Clear()
        {
            collection.Clear();
            ListChanged?.Invoke(this, new PropertyChangedEventArgs("List"));
        }
        public int IndexOf(T item)
        {
            return collection.IndexOf(item);
        }
        public void Insert(int index, T item)
        {
            collection.Insert(index, item);
            ListChanged?.Invoke(this, new PropertyChangedEventArgs("List"));
        }
        public void RemoveAt(int index)
        {
            collection.RemoveAt(index);
            ListChanged?.Invoke(this, new PropertyChangedEventArgs("List"));
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return collection.GetEnumerator();
        }
        public IEnumerator<T> GetEnumerator()
        {
            return collection.GetEnumerator();
        }
    }
}
