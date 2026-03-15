namespace BeatyBit.Bits;

/// <summary>
/// Represents a delegate that attempts to retrieve a value of type <typeparamref name="TR"/>
/// based on an input argument of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the input argument.</typeparam>
/// <typeparam name="TR">The type of the output value.</typeparam>
/// <param name="argument">The input argument used to attempt the retrieval of the value.</param>
/// <param name="value">
/// When this method returns, contains the retrieved value if the attempt was successful;
/// otherwise, the default value for the type <typeparamref name="TR"/>.
/// </param>
/// <returns>
/// Returns <c>true</c> if the value was successfully retrieved; otherwise, <c>false</c>.
/// </returns>
public delegate bool TryGet<in T, TR>(T argument, out TR value);
