#pragma warning disable 108 // new keyword hiding
#pragma warning disable 114 // new keyword hiding
namespace Windows.Devices.Printers
{
	#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
	[global::Uno.NotImplemented]
	#endif
	public  partial class IppAttributeError 
	{
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  global::System.Exception ExtendedError
		{
			get
			{
				throw new global::System.NotImplementedException("The member Exception IppAttributeError.ExtendedError is not implemented in Uno.");
			}
		}
		#endif
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  global::Windows.Devices.Printers.IppAttributeErrorReason Reason
		{
			get
			{
				throw new global::System.NotImplementedException("The member IppAttributeErrorReason IppAttributeError.Reason is not implemented in Uno.");
			}
		}
		#endif
		// Forced skipping of method Windows.Devices.Printers.IppAttributeError.Reason.get
		// Forced skipping of method Windows.Devices.Printers.IppAttributeError.ExtendedError.get
		#if __ANDROID__ || __IOS__ || NET461 || __WASM__ || __SKIA__ || __NETSTD_REFERENCE__ || __MACOS__
		[global::Uno.NotImplemented("__ANDROID__", "__IOS__", "NET461", "__WASM__", "__SKIA__", "__NETSTD_REFERENCE__", "__MACOS__")]
		public  global::System.Collections.Generic.IReadOnlyList<global::Windows.Devices.Printers.IppAttributeValue> GetUnsupportedValues()
		{
			throw new global::System.NotImplementedException("The member IReadOnlyList<IppAttributeValue> IppAttributeError.GetUnsupportedValues() is not implemented in Uno.");
		}
		#endif
	}
}
