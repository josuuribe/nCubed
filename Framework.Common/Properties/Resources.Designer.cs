﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RaraAvis.nCubed.Core.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RaraAvis.nCubed.Core.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error composition:\nCheck default constructor with [ImportConstructor]\nCheck dependencies\n\nInner Exception: {0}.
        /// </summary>
        internal static string ExceptionCanNotCreateObject {
            get {
                return ResourceManager.GetString("ExceptionCanNotCreateObject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can not destroy an object, maybe stack is empty.
        /// </summary>
        internal static string ExceptionDestroyNonExistingObject {
            get {
                return ResourceManager.GetString("ExceptionDestroyNonExistingObject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The configured exception formatter &apos;{0}&apos; must expose a public constructor that takes a TextWriter object, an Exception object and a GUID instance as parameters..
        /// </summary>
        internal static string ExceptionFormatterError {
            get {
                return ResourceManager.GetString("ExceptionFormatterError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The system configuration is missing, review and complete it.
        /// </summary>
        internal static string ExceptionSystemConfigurationMissing {
            get {
                return ResourceManager.GetString("ExceptionSystemConfigurationMissing", resourceCulture);
            }
        }
    }
}
