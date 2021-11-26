namespace Vue.cs.Framework.Extensions
{
    public struct NullObject<T>
    {
        public NullObject(T? value)
        {
            _value = value;
        }

        private T? _value;

        public bool IsNull => _value is null;


        public override string? ToString()
        {
            return _value?.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return IsNull;

            if (obj is NullObject<T> no)
                return _value?.Equals(no._value) ?? no._value is null;

            if (obj is T objValueType)
                return obj.Equals(_value);

            return false;
        }

        public override int GetHashCode()
        {
            if (_value is null)
                return int.MinValue;

            var hashCode = _value.GetHashCode();
            return hashCode != int.MaxValue
                ? hashCode + 1
                : hashCode - 1;
        }

        public static implicit operator T?(NullObject<T> nullObject)
        {
            return nullObject._value;
        }

        public static implicit operator NullObject<T>(T? item)
        {
            return new NullObject<T>(item);
        }

        public static NullObject<T> Null()
        {
            return new NullObject<T>(default);
        }
    }
}