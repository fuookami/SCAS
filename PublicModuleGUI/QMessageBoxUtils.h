#pragma once

#include <QtCore/QString>

namespace SSUtils
{
	namespace GUI
	{
		namespace QMessageBoxUtils
		{
			void information(const QString &title, const QString &information);
			void about(const QString &title, const QString &information);
			void aboutQt(void);

			bool comfirm(const QString &title, const QString &information);
			bool yesOrNot(const QString &title, const QString &information);
			int tricomfirm(const QString &title, const QString &information, const QString &button1Text, const QString &button2Text);

			int dualSelect(const QString &title, const QString &information, const QString &button1Text, const QString &button2Text);
			int triSelect(const QString &title, const QString &information, const QString &button1Text, const QString &button2Text, const QString &button3Text);
		};
	}
};
