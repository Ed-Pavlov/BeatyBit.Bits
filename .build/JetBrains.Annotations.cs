using System;
using System.Diagnostics;

// ReSharper disable All

namespace JetBrains.Annotations;

/// <summary>
/// Indicates that the marked symbol is used implicitly (e.g. via reflection, in external library),
/// so this symbol will be ignored by usage-checking inspections. <br/>
/// You can use <see cref="ImplicitUseKindFlags"/> and <see cref="ImplicitUseTargetFlags"/>
/// to configure how this attribute is applied.
/// </summary>
/// <example><code>
/// [UsedImplicitly]
/// public class TypeConverter {}
///
/// public class SummaryData
/// {
/// [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
/// public SummaryData() {}
/// }
///
/// [UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.Default)]
/// public interface IService {}
/// </code></example>
[AttributeUsage(AttributeTargets.All)]
[Conditional("JETBRAINS_ANNOTATIONS")]
internal sealed class UsedImplicitlyAttribute : Attribute
{
  public UsedImplicitlyAttribute()
    : this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default) { }

  public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
    : this(useKindFlags, ImplicitUseTargetFlags.Default) { }

  public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
    : this(ImplicitUseKindFlags.Default, targetFlags) { }

  public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
  {
    UseKindFlags = useKindFlags;
    TargetFlags  = targetFlags;
  }

  public ImplicitUseKindFlags UseKindFlags { get; }

  public ImplicitUseTargetFlags TargetFlags { get; }
}

/// <summary>
/// Can be applied to attributes, type parameters, and parameters of a type assignable from <see cref="System.Type"/> .
/// When applied to an attribute, the decorated attribute behaves the same as <see cref="UsedImplicitlyAttribute"/>.
/// When applied to a type parameter or to a parameter of type <see cref="System.Type"/>,
/// indicates that the corresponding type is used implicitly.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.GenericParameter | AttributeTargets.Parameter)]
[Conditional("JETBRAINS_ANNOTATIONS")]
internal sealed class MeansImplicitUseAttribute : Attribute
{
  public MeansImplicitUseAttribute()
    : this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default) { }

  public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags)
    : this(useKindFlags, ImplicitUseTargetFlags.Default) { }

  public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags)
    : this(ImplicitUseKindFlags.Default, targetFlags) { }

  public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
  {
    UseKindFlags = useKindFlags;
    TargetFlags  = targetFlags;
  }

  [UsedImplicitly]
  public ImplicitUseKindFlags UseKindFlags { get; }

  [UsedImplicitly]
  public ImplicitUseTargetFlags TargetFlags { get; }
}

/// <summary>
/// Specifies the details of implicitly used symbol when it is marked
/// with <see cref="MeansImplicitUseAttribute"/> or <see cref="UsedImplicitlyAttribute"/>.
/// </summary>
[Flags]
internal enum ImplicitUseKindFlags
{
  Default = Access | Assign | InstantiatedWithFixedConstructorSignature,

  /// <summary>Only entity marked with attribute considered used.</summary>
  Access = 1,

  /// <summary>Indicates implicit assignment to a member.</summary>
  Assign = 2,

  /// <summary>
  /// Indicates implicit instantiation of a type with fixed constructor signature.
  /// That means any unused constructor parameters won't be reported as such.
  /// </summary>
  InstantiatedWithFixedConstructorSignature = 4,

  /// <summary>Indicates implicit instantiation of a type.</summary>
  InstantiatedNoFixedConstructorSignature = 8,
}

/// <summary>
/// Specifies what is considered to be used implicitly when marked
/// with <see cref="MeansImplicitUseAttribute"/> or <see cref="UsedImplicitlyAttribute"/>.
/// </summary>
[Flags]
internal enum ImplicitUseTargetFlags
{
  Default = Itself,
  Itself  = 1,

  /// <summary>Members of the type marked with the attribute are considered used.</summary>
  Members = 2,

  /// <summary> Inherited entities are considered used. </summary>
  WithInheritors = 4,

  /// <summary>Entity marked with the attribute and all its members considered used.</summary>
  WithMembers = Itself | Members
}

/// <summary>
/// This attribute is intended to mark publicly available API,
/// which should not be removed and so is treated as used.
/// </summary>
[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
[AttributeUsage(AttributeTargets.All, Inherited = false)]
[Conditional("JETBRAINS_ANNOTATIONS")]
internal sealed class PublicAPIAttribute : Attribute
{
  public PublicAPIAttribute() { }

  public PublicAPIAttribute(string comment) => Comment = comment;

  public string? Comment { get; }
}

/// <summary>
/// Tells the code analysis engine if the parameter is completely handled when the invoked method is on stack.
/// If the parameter is a delegate, indicates that the delegate can only be invoked during method execution
/// (the delegate can be invoked zero or multiple times, but not stored to some field and invoked later,
/// when the containing method is no longer on the execution stack).
/// If the parameter is an enumerable, indicates that it is enumerated while the method is executed.
/// If <see cref="RequireAwait"/> is true, the attribute will only take effect
/// if the method invocation is located under the <c>await</c> expression.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class InstantHandleAttribute : Attribute
{
  /// <summary>
  /// Requires the method invocation to be used under the <c>await</c> expression for this attribute to take effect.
  /// Can be used for delegate/enumerable parameters of <c>async</c> methods.
  /// </summary>
  public bool RequireAwait { get; set; }
}