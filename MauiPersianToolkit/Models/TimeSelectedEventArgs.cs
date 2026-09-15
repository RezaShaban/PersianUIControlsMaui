namespace MauiPersianToolkit.Models;

/// <summary>Raised when the user confirms a time in <see cref="Controls.TimePickerView"/>.</summary>
public sealed class TimeSelectedEventArgs : EventArgs
{
    public TimeSelectedEventArgs(TimeSpan time) => Time = time;

    public TimeSpan Time { get; }
}

/// <summary>One row in a time wheel (hour/minute/second).</summary>
public sealed class TimeWheelItem : IEquatable<TimeWheelItem>
{
    public TimeWheelItem(int value, string label)
    {
        Value = value;
        Label = label;
    }

    public int Value { get; }
    public string Label { get; }

    public bool Equals(TimeWheelItem? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is TimeWheelItem other && Equals(other);
    public override int GetHashCode() => Value;
    public override string ToString() => Label;
}
