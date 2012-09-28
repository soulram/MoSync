﻿/* Copyright (C) 2011 MoSync AB

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License,
version 2, as published by the Free Software Foundation.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
MA 02110-1301, USA.
*/

/**
 * @file MoSyncListViewItem.cs
 * @author Rata Gabriela and Spiridon Alexandru
 *
 * @brief This represents the ListViewItem implementation for the NativeUI
 *        component on Windows Phone 7, language C#
 * Note: The "AccessoryType" property is not available on WP7.
 * @platform WP 7.1
 **/

using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Navigation;
using System;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Windows.Documents;
using System.Windows.Media;

namespace MoSync
{
	namespace NativeUI
	{
        /**
         * The ListViewItem class defines the attributes and behavior of the
         * items that appear in ListView objects.
         */
        public class ListViewItem : WidgetBaseWindowsPhone
        {
            /**
             * The TextBlock widget that will hold the text of the list view item
             */
			protected System.Windows.Controls.TextBlock mText;

            /**
             * Image wiget for the icon
             */
			protected System.Windows.Controls.Image mIcon;

            /**
             * Stech object that defines the way in which the icon will be streched
             */
			protected System.Windows.Media.Stretch mStretch;

            /**
            * The Grid object holding the icon and the text
            */
			protected System.Windows.Controls.Grid mGrid;

            /**
             * The The Grid will have one row and two colums. First column for the icon,
             * the second for text
             */
			protected RowDefinition mRow;
			protected ColumnDefinition mColumn1;
			protected ColumnDefinition mColumn2;


            // contains the subtitle text
            protected string mSubtitle;

            /**
            * Constructor
            */
			public ListViewItem()
			{
                mGrid = new System.Windows.Controls.Grid();

				mIcon = new System.Windows.Controls.Image();
				mIcon.VerticalAlignment = VerticalAlignment.Center;
				mStretch = new System.Windows.Media.Stretch();

				mText = new System.Windows.Controls.TextBlock();
				mText.TextWrapping = TextWrapping.NoWrap;
				mText.Margin = new Thickness(10);
				mText.VerticalAlignment = VerticalAlignment.Center;
				mText.TextAlignment = TextAlignment.Left;

				mColumn1 = new ColumnDefinition();
				mColumn1.Width = new GridLength(1, GridUnitType.Auto);

				mColumn2 = new ColumnDefinition();
				mColumn2.Width = new GridLength(1, GridUnitType.Star);

				mRow = new RowDefinition();
				mRow.Height = new GridLength(1, GridUnitType.Auto);

				mGrid.RowDefinitions.Add(mRow);
				mGrid.ColumnDefinitions.Add(mColumn1);
				mGrid.ColumnDefinitions.Add(mColumn2);

				Grid.SetRow(mIcon, 0);
				Grid.SetColumn(mIcon, 0);

				Grid.SetRow(mText, 0);
				Grid.SetColumnSpan(mText, 1);
				Grid.SetColumn(mText, 1);

				mGrid.Children.Add(mIcon);
				mGrid.Children.Add(mText);

                mView = mGrid;

                this.ItemSelected = false;
			}

            public override void AddChild(IWidget child)
            {
                base.AddChild(child);
                MoSync.Util.RunActionOnMainThreadSync(() =>
                    {
                        WidgetBaseWindowsPhone widget = (child as WidgetBaseWindowsPhone);
                        mGrid.Children.Add(widget.View);
                    });
            }

            /**
             * Asks for the parent section to rebuild the list view model.
             */
            public void RebuildParentList()
            {
                if (mParent is ListViewSection)
                {
                    ListViewSection parentSection = mParent as ListViewSection;
                    parentSection.RebuildParentList();
                }
            }

            /**
             * Sets the selected state of the item.
             */
            public bool ItemSelected
            {
                get;
                set;
            }

            #region MoSync Widget Properties

            /**
             * Implementation of the "Height" property.
             * Gets/sets the height of the widget.
             */
            [MoSyncWidgetProperty(MoSync.Constants.MAW_WIDGET_HEIGHT)]
            public new double Height
            {
                get
                {
                    return mGrid.Height;
                }
                set
                {
                    mGrid.Height = value;
                }
            }

            /**
             * Implementation of the "Width" property.
             * Gets/sets the height of the widget.
             */
            [MoSyncWidgetProperty(MoSync.Constants.MAW_WIDGET_WIDTH)]
            public new double Width
            {
                get
                {
                    return mGrid.Width;
                }
                set
                {
                    mGrid.Width = value;
                }
            }

            /**
             * Implementation of the "Text" property.
             * Sets the text that will appear on the list view item
             */
			[MoSyncWidgetProperty(MoSync.Constants.MAW_LIST_VIEW_ITEM_TEXT)]
			public String Text
			{
				set
				{
                    mText.Text = value;
                    RebuildParentList();
				}
				get
				{
                    return mText.Text;
				}
			}

            /**
             * Implementation of the "Text" property.
             * Sets the text that will appear on the list view item
             */
            [MoSyncWidgetProperty(MoSync.Constants.MAW_LIST_VIEW_ITEM_SUBTITLE)]
            public String Subtitle
            {
                set
                {
                    mSubtitle = value;
                    RebuildParentList();
                }
                get
                {
                    return mSubtitle;
                }
            }

            /**
             * Implementation of the "IsSelected" property.
             * Gets the selected state of the current item.
             */
            [MoSyncWidgetProperty(MoSync.Constants.MAW_LIST_VIEW_ITEM_IS_SELECTED)]
            public string IsSelected
            {
                get
                {
                    if (this.ItemSelected == true)
                    {
                        return "true";
                    }
                    return "false";
                }
            }

            /**
             * Implementation of the "Icon" property.
             * Sets the Icon that will appear on the list view item, on the left of the text
             */
			[MoSyncWidgetProperty(MoSync.Constants.MAW_LIST_VIEW_ITEM_ICON)]
			public String Icon
			{
				set
				{
					int val = 0;
                    if (int.TryParse(value, out val))
                    {
                        Resource res = mRuntime.GetResource(MoSync.Constants.RT_IMAGE, val);
                        if (null != res && res.GetInternalObject() != null)
                        {
                            mIcon.Width = mText.Height;
                            mIcon.Height = mText.Height;
                            mIcon.Margin = new Thickness(mText.Margin.Left, mText.Margin.Top, 0, mText.Margin.Bottom);
                            mStretch = System.Windows.Media.Stretch.Fill;
                            mIcon.Stretch = mStretch;

                            System.Windows.Media.Imaging.BitmapSource bmpSource =
                            (System.Windows.Media.Imaging.BitmapSource)(res.GetInternalObject());

                            mIcon.Source = bmpSource;
                        }
                        else throw new InvalidPropertyValueException();
                    }
                    else throw new InvalidPropertyValueException();
                    RebuildParentList();
				}
			}

            /**
             * Returns the Icon image source (needed to set the image of the list view item
             * if the list is alphabetical or segmented).
             */
            public ImageSource IconImageSource
            {
                get
                {
                    return mIcon.Source;
                }
            }

            /**
             * The implementation of the "FontColor" property.
             * Sets the font color of the item's text.
             */
			[MoSyncWidgetProperty(MoSync.Constants.MAW_LIST_VIEW_ITEM_FONT_COLOR)]
			public String FontColor
			{
				set
				{
					System.Windows.Media.SolidColorBrush brush;
					MoSync.Util.convertStringToColor(value, out brush);
					mText.Foreground = brush;
				}
			}

            /**
             * The implementation of the "FontSize" property.
             * Sets the font size of the item's text
             */
			[MoSyncWidgetProperty(MoSync.Constants.MAW_LIST_VIEW_ITEM_FONT_SIZE)]
			public double FontSize
			{
                set
                {
                    mText.FontSize = value;
                }
			}

            /**
             * The implementation of the "FontHandle" property.
             * Sets the font handle used to display the item's text
             */
            [MoSyncWidgetProperty(MoSync.Constants.MAW_LABEL_FONT_HANDLE)]
            public int FontHandle
            {
                set
                {
					FontModule.FontInfo fontInfo =
						mRuntime.GetModule<FontModule>().GetFont(value);

					mText.FontFamily = fontInfo.family;
					mText.FontWeight = fontInfo.weight;
					mText.FontStyle = fontInfo.style;
				}
            }

            #endregion
        } // end of ListViewItem class
	} // end of NativeUI namespace
} // end of MoSync namespace