**Useful bits for reuse in other projects**

# Lifetime Management (Lifetime / LifetimeDefinition)
Instead of the standard `IDisposable` pattern, which scales poorly across complex object hierarchies, `Lifetime` provides an advanced system for managing object lifecycles.

## Leak Prevention
No longer need to remember to manually call `Dispose()` in multiple locations. A resource is simply "bound" to the current scope.

## Hierarchical Structure:
`Lifetime` enables the creation of dependency trees. When a parent `LifetimeDefinition` is terminated, all child `Lifetimes` and their associated resources are automatically released.

## Determinism:
All resources (such as files, network connections, or event subscriptions) are released predictably and in the correct order.

# PathBuddy: Type-Safe Path Management
PathBuddy is a lightweight library designed to replace fragile string-based path manipulation with strongly-typed `AbsolutePath` and `RelativePath` objects. It integrates with a `Lifetime` system to handle temporary file cleanup automatically.

## Purpose
The primary goal is to eliminate "path-string soup" and manual resource cleanup. By distinguishing between absolute and relative paths at the type level, PathBuddy prevents common bugs (like combining two absolute paths).

## Intuitive Path Combining
Uses the / operator for clean, readable path joining (e.g., baseDir / "logs" / "app.log"), replacing cumbersome Path.Combine calls.

# Other bits
* `Maybe` monad
* A couple of extensions for working with `Dictionary`:
  * `GetOrCreateValue` — encapsulation of often necessary action.
  * `GetValueSafe` — for use in places where it doesn't matter whether the value is in the Dictionary or not.
* `LeanList4` — list implementation that stores the first four elements inline and falls back to List.
* `Apply`— function programming style method