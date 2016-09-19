namespace SFA.Apprenticeships.Domain.Entities.Collections.Generic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Dictionary which pretends that keys not present have a default value
    /// </summary>
    public class SparseDictionary<K, V> : IReadOnlyDictionary<K, V>
    {
        private IReadOnlyDictionary<K, V> _dict;

        private V _default;

        public SparseDictionary(IReadOnlyDictionary<K, V> dict, V defaultvalue)
        {
            _dict = dict;
            _default = defaultvalue;
        }

        public V this[K key]
        {
            get
            {
                V result;
                if (_dict.TryGetValue(key, out result))
                    return result;
                else
                    return _default;
            }
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException("This is a sparse dictionary and so a count isn't very meaningful");
            }
        }

        public IEnumerable<K> Keys
        {
            get
            {
                throw new NotImplementedException("This is a sparse dictionary and logically contains all possible keys");
            }
        }

        public IEnumerable<V> Values
        {
            get
            {
                throw new NotImplementedException("This is a sparse dictionary and logically contains values for all possible keys");
            }
        }

        public bool ContainsKey(K key)
        {
            return true;
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            throw new NotImplementedException("This is a sparse dictionary and logically contains all possible keys");
        }

        public bool TryGetValue(K key, out V value)
        {
            value = this[key];
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException("This is a sparse dictionary and logically contains all possible keys");
        }
    }
}