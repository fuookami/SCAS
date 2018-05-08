#include "QMessageBoxUtils.h"
#include <QtWidgets/QMessageBox>
#include <memory>

namespace SSUtils
{
	namespace GUI
	{
		namespace QMessageBoxUtils
		{
			void information(const QString & title, const QString & information)
			{
				QMessageBox::information(nullptr, title, information);
			}

			void about(const QString & title, const QString & information)
			{
				QMessageBox::about(nullptr, title, information);
			}

			void aboutQt(void)
			{
				QMessageBox::aboutQt(nullptr);
			}

			bool comfirm(const QString & title, const QString & information)
			{
				static const QString ComfrimButtonText(QString::fromLocal8Bit("确定"));
				static const QString CancelButtonText(QString::fromLocal8Bit("取消"));
				std::shared_ptr<QMessageBox> dialog(new QMessageBox(QMessageBox::NoIcon, title, information, QMessageBox::Yes | QMessageBox::No));
				dialog->setButtonText(QMessageBox::Yes, ComfrimButtonText);
				dialog->setButtonText(QMessageBox::No, CancelButtonText);
				dialog->exec();
				return dialog->result() == QMessageBox::Yes;
			}

			bool yesOrNot(const QString & title, const QString & information)
			{
				static const QString YesButtonText(QString::fromLocal8Bit("是"));
				static const QString NoButtonText(QString::fromLocal8Bit("否"));
				std::shared_ptr<QMessageBox> dialog(new QMessageBox(QMessageBox::NoIcon, title, information, QMessageBox::Yes | QMessageBox::No));
				dialog->setButtonText(QMessageBox::Yes, YesButtonText);
				dialog->setButtonText(QMessageBox::No, NoButtonText);
				dialog->exec();
				return dialog->result() == QMessageBox::Yes;
			}

			int tricomfirm(const QString & title, const QString & information, const QString & button1Text, const QString & button2Text)
			{
				static const QString CancelButtonText(QString::fromLocal8Bit("取消"));
				std::shared_ptr<QMessageBox> dialog(new QMessageBox(QMessageBox::NoIcon, title, information, QMessageBox::Yes | QMessageBox::No | QMessageBox::Cancel));
				dialog->setButtonText(QMessageBox::Yes, button1Text);
				dialog->setButtonText(QMessageBox::No, button2Text);
				dialog->setButtonText(QMessageBox::Cancel, CancelButtonText);
				dialog->exec();
				return dialog->result() == QMessageBox::Yes ? 1
					: dialog->result() == QMessageBox::No ? 2
					: 0;
			}

			int dualSelect(const QString & title, const QString & information, const QString & button1Text, const QString & button2Text)
			{
				std::shared_ptr<QMessageBox> dialog(new QMessageBox(QMessageBox::NoIcon, title, information, QMessageBox::Yes | QMessageBox::No));
				dialog->setButtonText(QMessageBox::Yes, button1Text);
				dialog->setButtonText(QMessageBox::No, button2Text);
				dialog->exec();
				return dialog->result() == QMessageBox::Yes ? 1 : 0;
			}

			int triSelect(const QString & title, const QString & information, const QString & button1Text, const QString & button2Text, const QString & button3Text)
			{
				std::shared_ptr<QMessageBox> dialog(new QMessageBox(QMessageBox::NoIcon, title, information, QMessageBox::Yes | QMessageBox::No | QMessageBox::Cancel));
				dialog->setButtonText(QMessageBox::Yes, button1Text);
				dialog->setButtonText(QMessageBox::No, button2Text);
				dialog->setButtonText(QMessageBox::Cancel, button3Text);
				dialog->exec();
				return dialog->result() == QMessageBox::Yes ? 1
					: dialog->result() == QMessageBox::No ? 2
					: 3;
			}
		};
	};
};
