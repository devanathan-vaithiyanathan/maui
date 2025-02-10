﻿using System;
using System.Collections.Specialized;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using AppCompatAlertDialog = AndroidX.AppCompat.App.AlertDialog;
using AResource = Android.Resource;

namespace Microsoft.Maui.Handlers
{
	public partial class PickerHandler : ViewHandler<IPicker, MauiPicker>
	{
		AppCompatAlertDialog? _dialog;

		protected override MauiPicker CreatePlatformView() =>
			new MauiPicker(Context);

		protected override void ConnectHandler(MauiPicker platformView)
		{
			platformView.FocusChange += OnFocusChange;
			platformView.Click += OnClick;

			base.ConnectHandler(platformView);
		}

		protected override void DisconnectHandler(MauiPicker platformView)
		{
			platformView.FocusChange -= OnFocusChange;
			platformView.Click -= OnClick;

			base.DisconnectHandler(platformView);
		}

		// This is a Android-specific mapping
		public static void MapBackground(IPickerHandler handler, IPicker picker)
		{
			handler.PlatformView?.UpdateBackground(picker);
		}

		// TODO Uncomment me on NET8 [Obsolete]
		public static void MapReload(IPickerHandler handler, IPicker picker, object? args) => Reload(handler);

		internal static void MapItems(IPickerHandler handler, IPicker picker) => Reload(handler);

		public static void MapTitle(IPickerHandler handler, IPicker picker)
		{
			handler.PlatformView?.UpdateTitle(picker);
		}

		public static void MapTitleColor(IPickerHandler handler, IPicker picker)
		{
			handler.PlatformView?.UpdateTitleColor(picker);
		}

		public static void MapSelectedIndex(IPickerHandler handler, IPicker picker)
		{
			handler.PlatformView?.UpdateSelectedIndex(picker);
		}

		public static void MapCharacterSpacing(IPickerHandler handler, IPicker picker)
		{
			handler.PlatformView?.UpdateCharacterSpacing(picker);
		}

		public static void MapFont(IPickerHandler handler, IPicker picker)
		{
			var fontManager = handler.GetRequiredService<IFontManager>();

			handler.PlatformView?.UpdateFont(picker, fontManager);
		}

		public static void MapHorizontalTextAlignment(IPickerHandler handler, IPicker picker)
		{
			handler.PlatformView?.UpdateHorizontalAlignment(picker.HorizontalTextAlignment);
		}

		public static void MapTextColor(IPickerHandler handler, IPicker picker)
		{
			handler.PlatformView.UpdateTextColor(picker);
			if (handler is PickerHandler pickerHandler)
				pickerHandler.UpdateDialogItemsColor(picker.TextColor.ToPlatform());
			
		}

		internal void UpdateDialogItemsColor(Color? color)
		{
			if (_dialog?.ListView != null && color != null)
            {
                for (int i = 0; i < _dialog.ListView.ChildCount; i++)
                {
                    if (_dialog.ListView.GetChildAt(i) is TextView textView)
                    {
                    	textView.SetTextColor(color.Value);
                    }
                }
            }
		}

		public static void MapVerticalTextAlignment(IPickerHandler handler, IPicker picker)
		{
			handler.PlatformView?.UpdateVerticalAlignment(picker.VerticalTextAlignment);
		}

		void OnFocusChange(object? sender, global::Android.Views.View.FocusChangeEventArgs e)
		{
			if (PlatformView == null)
				return;

			if (e.HasFocus)
			{
				if (PlatformView.Clickable)
					PlatformView.CallOnClick();
				else
					OnClick(PlatformView, EventArgs.Empty);
			}
			else if (_dialog != null)
			{
				_dialog.Hide();
				_dialog = null;
			}
		}

		void OnClick(object? sender, EventArgs e)
		{
			if (_dialog == null && VirtualView != null)
			{
				using (var builder = new AppCompatAlertDialog.Builder(Context))
				{
					if (VirtualView.TitleColor == null)
					{
						builder.SetTitle(VirtualView.Title ?? string.Empty);
					}
					else
					{
						var title = new SpannableString(VirtualView.Title ?? string.Empty);
#pragma warning disable CA1416 // https://github.com/xamarin/xamarin-android/issues/6962
						title.SetSpan(new ForegroundColorSpan(VirtualView.TitleColor.ToPlatform()), 0, title.Length(), SpanTypes.ExclusiveExclusive);
#pragma warning restore CA1416
						builder.SetTitle(title);
					}

					string[] items = VirtualView.GetItemsAsArray();

					for (var i = 0; i < items.Length; i++)
					{
						var item = items[i];
						if (item == null)
							items[i] = String.Empty;
					}

					var adapter = new ColorAdapter(Context, Android.Resource.Layout.SimpleListItem1, items, VirtualView.TextColor?.ToPlatform());

					builder.SetAdapter(adapter, (s, e) =>
					{
						var selectedIndex = e.Which;
						VirtualView.SelectedIndex = selectedIndex;
						base.PlatformView?.UpdatePicker(VirtualView);
					});

					builder.SetNegativeButton(AResource.String.Cancel, (o, args) => { });

					_dialog = builder.Create();
				}

				if (_dialog == null)
					return;

				_dialog.UpdateFlowDirection(PlatformView);

				_dialog.SetCanceledOnTouchOutside(true);

				_dialog.DismissEvent += (sender, args) =>
				{
					_dialog = null;
				};

				_dialog.Show();
			}
		}

		static void Reload(IPickerHandler handler)
		{
			handler.PlatformView.UpdatePicker(handler.VirtualView);
		}
	}

	class ColorAdapter : ArrayAdapter<string>
	{
		Color? _textColor;

		public ColorAdapter(Context context, int resource, string[] objects, Color? textColor) 
			: base(context, resource, objects)
		{
			_textColor = textColor;
		}

		public override View GetView(int position, View? convertView, ViewGroup parent)
		{
			View view = base.GetView(position, convertView, parent);

			if (view is TextView textView && _textColor.HasValue)
			{
				textView.SetTextColor(_textColor.Value);
				textView.SetTextSize(Android.Util.ComplexUnitType.Sp, 14);
			}

			return view;
		}
	}
}