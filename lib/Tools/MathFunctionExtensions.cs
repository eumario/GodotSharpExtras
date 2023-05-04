namespace Godot.Sharp.Extras;

public static partial class MathFunctionExtensions
{
    /// <summary>
    /// Checks to see if <see cref="int"/> value is in range of min value, and Max value.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns>True if in range, false otherwise.</returns>
    public static bool InRange(this int @this, int minValue, int maxValue) => 
        @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;

    /// <summary>
    /// Checks to see if <see cref="float"/> value is in range of min value, and Max value.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns>True if in range, false otherwise.</returns>
    public static bool InRange(this float @this, float minValue, float maxValue) => 
        @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;

    /// <summary>
    /// Checks to see if <see cref="double"/> value is in range of min value, and Max value.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns>True if in range, false otherwise.</returns>
    public static bool InRange(this double @this, double minValue, double maxValue) => 
        @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;

    /// <summary>
    /// Checks to see if <see cref="Vector2"/> value is in range of min value, and Max value.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns>True if in range, false otherwise.</returns>
    public static bool InRange(this Vector2 @this, Vector2 minValue, Vector2 maxValue) => 
        @this.X.InRange(minValue.X, maxValue.X) && @this.Y.InRange(minValue.Y, maxValue.Y);

    /// <summary>
    /// Checks to see if <see cref="Vector2I"/> value is in range of min value, and Max value.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns>True if in range, false otherwise.</returns>
    public static bool InRange(this Vector2I @this, Vector2I minValue, Vector2I maxValue) => 
        @this.X.InRange(minValue.X, maxValue.X) && @this.Y.InRange(minValue.Y, maxValue.Y);

    /// <summary>
    /// Checks to see if <see cref="Vector3"/> value is in range of min value, and Max value.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns>True if in range, false otherwise.</returns>
    public static bool InRange(this Vector3 @this, Vector3 minValue, Vector3 maxValue) => 
        @this.X.InRange(minValue.X, maxValue.X) && @this.Y.InRange(minValue.Y, maxValue.Y) &&
               @this.Z.InRange(minValue.Z, maxValue.Z);

    /// <summary>
    /// Checks to see if <see cref="Vector3I"/> value is in range of min value, and Max value.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns>True if in range, false otherwise.</returns>
    public static bool InRange(this Vector3I @this, Vector3I minValue, Vector3I maxValue) => 
        @this.X.InRange(minValue.X, maxValue.X) && @this.Y.InRange(minValue.Y, maxValue.Y) &&
               @this.Z.InRange(minValue.Z, maxValue.Z);
}