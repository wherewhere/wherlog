using Microsoft.JSInterop;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Wherlog.Helpers
{
    public static class JSRuntimeHelper
    {
        /// <summary>
        /// Flags for a member that is JSON (de)serialized.
        /// </summary>
        public const DynamicallyAccessedMemberTypes JsonSerialized = DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties;

        /// <inheritdoc cref="JSRuntimeExtensions.InvokeAsync{TValue}(IJSRuntime, string, object?[]?)"/>
        public static ValueTask<TValue> TryInvokeAsync<[DynamicallyAccessedMembers(JsonSerialized)] TValue>(this IJSRuntime jsRuntime, string identifier, params object[] args)
        {
            ArgumentNullException.ThrowIfNull(jsRuntime);
            try
            {
                return jsRuntime.InvokeAsync<TValue>(identifier, args);
            }
            catch (Exception ex)
            {
                return ValueTask.FromException<TValue>(ex);
            }
        }

        /// <inheritdoc cref="JSObjectReferenceExtensions.InvokeVoidAsync(IJSObjectReference, string, object?[]?)"/>
        public static ValueTask TryInvokeVoidAsync(this IJSObjectReference jsObjectReference, string identifier, params object[] args)
        {
            ArgumentNullException.ThrowIfNull(jsObjectReference);
            try
            {
                return jsObjectReference.InvokeVoidAsync(identifier, args);
            }
            catch (Exception ex)
            {
                return ValueTask.FromException(ex);
            }
        }

        /// <inheritdoc cref="JSObjectReferenceExtensions.InvokeAsync{TValue}(IJSObjectReference, string, object?[]?)"/>
        public static ValueTask<TValue> TryInvokeAsync<[DynamicallyAccessedMembers(JsonSerialized)] TValue>(this IJSObjectReference jsObjectReference, string identifier, params object[] args)
        {
            ArgumentNullException.ThrowIfNull(jsObjectReference);
            try
            {
                return jsObjectReference.InvokeAsync<TValue>(identifier, args);
            }
            catch (Exception ex)
            {
                return ValueTask.FromException<TValue>(ex);
            }
        }
    }
}
