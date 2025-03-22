Useful bits for reuse in other projects
* Maybe monad
* A couple of extensions for working with Dictionary.
  * GetOrCreateValue—encapsulation of often necessary action.
  * GetValueSafe—for use in places where it doesn't matter whether the value is in the Dictionary or not.
* LeanList4—list implementation that stores the first four elements inline and falls back to List.
