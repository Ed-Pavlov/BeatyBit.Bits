# BeatyBit.Bits

<p align="center">
  <img src="/.build/icon.png" width="86" height="86">
</p>

[![Build & Test](https://github.com/Ed-Pavlov/BeatyBit.Bits/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/Ed-Pavlov/Armature/actions/workflows/build-and-test.yml)
[![Nuget](https://img.shields.io/nuget/dt/BeatyBit.Bits)](https://www.nuget.org/packages/Armature/)

## Powered by
<p align="right">
  <img src="https://resources.jetbrains.com/storage/products/company/brand/logos/Rider.png" width="185" height="64">
</p>

**Useful bits for reuse in other projects**
* Maybe monad
* A couple of extensions for working with Dictionary.
  * GetOrCreateValue—encapsulation of often necessary action.
  * GetValueSafe—for use in places where it doesn't matter whether the value is in the Dictionary or not.
* LeanList4—list implementation that stores the first four elements inline and falls back to List.