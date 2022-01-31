﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.UI.Dispatching;
using Uno.UI.Tests.App.Xaml;
using Uno.UI.Tests.Helpers;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Uno.UI.Tests.Windows_UI_Xaml
{
	[TestClass]
	public class Given_FrameworkTemplate
	{
		private FrameworkTemplatePoolMockPlatformProvider _mockProvider;
		private bool _previousPoolingEnabled;

		[TestInitialize]
		public void Init()
		{
			UnitTestsApp.App.EnsureApplication();

			_previousPoolingEnabled = FrameworkTemplatePool.IsPoolingEnabled;
			FrameworkTemplatePool.Instance.SetPlatformProvider(_mockProvider = new());

			FrameworkTemplatePool.IsPoolingEnabled = true;
		}

		[TestCleanup]
		public void Cleanup()
		{
			FrameworkTemplatePool.IsPoolingEnabled = _previousPoolingEnabled;
			FrameworkTemplatePool.Instance.SetPlatformProvider(null);
			FrameworkTemplatePool.Scavenge();
		}

		[TestMethod]
		public void When_RemoveTemplate()
		{
			_mockProvider.CanUseMemoryManager = false;

			var TemplateCreated = 0;
			List<TemplatePoolAwareControl> _created = new List<TemplatePoolAwareControl>();
			var dataTemplate = new DataTemplate(() =>
			{
				TemplateCreated++;
				var b = new TemplatePoolAwareControl();
				_created.Add(b);
				return b;
			});

			var SUT = new ContentControl()
			{
				ContentTemplate = dataTemplate
			};

			var root = new Grid();
			root.ForceLoaded();
			root.Children.Add(SUT);

			Assert.AreEqual(1, TemplateCreated);

			SUT.ContentTemplate = null;

			Assert.AreEqual(1, TemplateCreated);
			Assert.AreEqual(1, _created.Count);
			Assert.AreEqual(1, _created[0].TemplateRecycled);
		}

		[TestMethod]
		public void When_RemoveTemplate_And_Reuse()
		{
			_mockProvider.CanUseMemoryManager = false;

			var TemplateCreated = 0;
			List<TemplatePoolAwareControl> _created = new List<TemplatePoolAwareControl>();
			var dataTemplate = new DataTemplate(() =>
			{
				TemplateCreated++;
				var b = new TemplatePoolAwareControl();
				_created.Add(b);
				return b;
			});

			var SUT = new ContentControl()
			{
				ContentTemplate = dataTemplate
			};

			var root = new Grid();
			root.ForceLoaded();
			root.Children.Add(SUT);

			Assert.AreEqual(1, TemplateCreated);

			SUT.ContentTemplate = null;

			Assert.AreEqual(1, TemplateCreated);
			Assert.AreEqual(1, _created.Count);
			Assert.AreEqual(1, _created[0].TemplateRecycled);
			Assert.IsNull(SUT.ContentTemplateRoot);

			SUT.ContentTemplate = dataTemplate;

			Assert.AreEqual(1, TemplateCreated);
			Assert.AreEqual(1, _created.Count);
			Assert.AreEqual(1, _created[0].TemplateRecycled);
			Assert.IsNotNull(SUT.ContentTemplateRoot);
		}

		[TestMethod]
		public void When_RemoveTemplate_And_Timeout()
		{
			_mockProvider.CanUseMemoryManager = false;

			List<TemplatePoolAwareControl> _created = new();
			var dataTemplate = new DataTemplate(() =>
			{
				var b = new TemplatePoolAwareControl();
				_created.Add(b);
				return b;
			});

			var SUT = new ContentControl()
			{
				ContentTemplate = dataTemplate
			};

			var root = new Grid();
			root.ForceLoaded();
			root.Children.Add(SUT);

			Assert.AreEqual(1, _created.Count);

			SUT.ContentTemplate = null;

			Assert.AreEqual(1, _created.Count);
			Assert.AreEqual(1, _created[0].TemplateRecycled);
			Assert.IsNull(SUT.ContentTemplateRoot);

			_mockProvider.Now = TimeSpan.FromMinutes(2);
			FrameworkTemplatePool.Instance.Scavenge(false);

			SUT.ContentTemplate = dataTemplate;

			Assert.AreEqual(2, _created.Count);
			Assert.AreEqual(1, _created[0].TemplateRecycled);
			Assert.IsNotNull(SUT.ContentTemplateRoot);
		}

		[TestMethod]
		public void When_RemoveTemplate_And_OutOfMemory()
		{
			_mockProvider.CanUseMemoryManager = true;
			_mockProvider.AppMemoryUsageLimit = 100;

			List<TemplatePoolAwareControl> _created = new();
			var dataTemplate = new DataTemplate(() =>
			{
				var b = new TemplatePoolAwareControl();
				_created.Add(b);
				return b;
			});

			var SUT = new ContentControl()
			{
				ContentTemplate = dataTemplate
			};

			var root = new Grid();
			root.ForceLoaded();
			root.Children.Add(SUT);

			Assert.AreEqual(1, _created.Count);

			_mockProvider.AppMemoryUsage = 81;

			SUT.ContentTemplate = null;

			Assert.AreEqual(1, _created.Count);
			Assert.AreEqual(0, _created[0].TemplateRecycled);
			Assert.IsNull(SUT.ContentTemplateRoot);

			_mockProvider.Now = TimeSpan.FromMinutes(2);
			FrameworkTemplatePool.Instance.Scavenge(false);

			SUT.ContentTemplate = dataTemplate;

			Assert.AreEqual(2, _created.Count);
			Assert.AreEqual(0, _created[0].TemplateRecycled);
			Assert.IsNotNull(SUT.ContentTemplateRoot);

			_mockProvider.AppMemoryUsage = 79;

			SUT.ContentTemplate = null;

			Assert.AreEqual(2, _created.Count);
			Assert.AreEqual(0, _created[0].TemplateRecycled);
			Assert.AreEqual(1, _created[1].TemplateRecycled);
			Assert.IsNull(SUT.ContentTemplateRoot);
		}

		private class FrameworkTemplatePoolMockPlatformProvider : IFrameworkTemplatePoolPlatformProvider
		{
			public TimeSpan Now { get; set; }

			public bool CanUseMemoryManager { get; set; }

			public ulong AppMemoryUsage { get; set; }

			public ulong AppMemoryUsageLimit { get; set; }

			public Task Delay(TimeSpan duration) => throw new NotImplementedException();
			public void Schedule(IdleDispatchedHandler action) => throw new NotImplementedException();
		}
	}

	public class TemplatePoolAwareControl : Grid, IFrameworkTemplatePoolAware
	{
		public int TemplateRecycled { get; private set; }

		public void OnTemplateRecycled()
		{
			TemplateRecycled++;
		}
	}
}