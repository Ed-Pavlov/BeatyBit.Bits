<p align='right'>If my projects has done you any good, consider supporting my future initiatives</p>
<p align="right">
  <a href="https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=ed@pavlov.is&lc=US&item_name=Kudos+for+Bits&no_note=0&cn=&currency_code=EUR">
    <img src=".build/button.png" width="76" height="32">
  </a>
</p>

___

# BeatyBit.Bits

<p align="center">
  <img src="/.build/icon.png" width="86" height="86">
</p>


**Useful bits of architecture and code**

[![Build & Test](https://github.com/Ed-Pavlov/BeatyBit.Bits/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/Ed-Pavlov/BeatyBit.Bits/actions/workflows/build-and-test.yml)
![badge](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/Ed-Pavlov/21c1ecd6072def6cf2300fe365aa46b9/raw/bits-test-coverage.json)
[![Nuget](https://img.shields.io/nuget/dt/BeatyBit.Bits)](https://www.nuget.org/packages/BeatyBit.Bits/)

___

## Powered by
<p align="right">
  <img src="https://resources.jetbrains.com/storage/products/company/brand/logos/Rider.png" width="185" height="64">
</p>

___

## Lifetime Management (Lifetime / LifetimeDefinition)
Instead of the standard `IDisposable` pattern, which scales poorly across complex object hierarchies, `Lifetime` provides an advanced system for managing object lifecycles.

### Leak Prevention
No longer need to remember to manually call `Dispose()` in multiple locations. A resource is simply "bound" to the current scope.

### Hierarchical Structure:
`Lifetime` enables the creation of dependency trees. When a parent `LifetimeDefinition` is terminated, all child `Lifetimes` and their associated resources are automatically released.

### Determinism:
All resources (such as files, network connections, or event subscriptions) are released predictably and in the correct order.

## Path Buddy: Type-Safe Path Management
Path Buddy is a lightweight library designed to replace fragile string-based path manipulation with strongly-typed `AbsolutePath` and `RelativePath` objects. It integrates with a `Lifetime` system to handle temporary file cleanup automatically.

### Purpose
The primary goal is to eliminate "path-string soup" and manual resource cleanup. By distinguishing between absolute and relative paths at the type level, PathBuddy prevents common bugs (like combining two absolute paths).

### Intuitive Path Combining
Uses the / operator for clean, readable path joining (e.g., baseDir / "logs" / "app.log"), replacing cumbersome Path.Combine calls.

## Other bits
* `Maybe` monad
* A couple of extensions for working with `Dictionary`:
  * `GetOrCreateValue` — encapsulation of often necessary action.
  * `GetValueSafe` — for use in places where it doesn't matter whether the value is in the Dictionary or not.
* `LeanList4` — list implementation that stores the first four elements inline and falls back to List.
* `Apply`— function programming style method