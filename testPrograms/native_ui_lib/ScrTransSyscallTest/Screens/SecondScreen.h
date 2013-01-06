/*
 Copyright (C) 2011 MoSync AB

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
 * @file SecondScreen.h
 * @author Bogdan Iusco & Mircea Vasiliniuc
 * @date 20 Nov 2012
 *
 * @brief Second screen.
 */

#ifndef SECOND_SCREEN_H_
#define SECOND_SCREEN_H_

#include <NativeUI/Screen.h>
#include <NativeUI/ButtonListener.h>

#include "../Observers/FirstScreenObserver.h"
#include "../Observers/SecondScreenObserver.h"

namespace NativeUI
{
	class Button;
	class Label;
	class VerticalLayout;
}

namespace Transitions
{

	class SecondScreen:
		public NativeUI::Screen,
		public NativeUI::ButtonListener
	{
	public:
		/**
		 * Constructor.
		 * @param observer Observer for this screen.
		 */
		SecondScreen(SecondScreenObserver& observer);

		/**
		 * Destructor.
		 */
		virtual ~SecondScreen();

        /**
         * This method is called if the touch-up event was inside the
         * bounds of the button.
         * Platform: iOS, Android, Windows Phone.
         * @param button The button object that generated the event.
         */
        virtual void buttonClicked(NativeUI::Widget* button);

		/**
		 * Appends text to title.
		 */
        void resetTitleWithString(const char* appendText);

	private:
		/**
		 * Create screen's UI.
		 */
		void createUI();

	private:
		/**
		 * Observer for this screen.
		 */
		SecondScreenObserver& mObserver;

		/**
		 * Screen's layout.
		 */
		NativeUI::VerticalLayout* mMainLayout;

		/**
		 * Hide this screen on click.
		 */
		NativeUI::Button* mHideScreenButton;

		/**
		 * Screen Title
		 */
		NativeUI::Label* mTitleLabel;

		/**
		 * Footer layout.
		 */
		NativeUI::HorizontalLayout* mFooterLayout;
	};
}

#endif /* SECOND_SCREEN_H_ */
