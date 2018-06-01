#include "QLoadingDialog.h"
#include <QtWidgets/QGridLayout>

namespace SSUtils
{
	namespace GUI
	{
		std::shared_ptr<QLoadingDialog> QLoadingDialog::getInstance(void)
		{
			static std::shared_ptr<QLoadingDialog> ret(new QLoadingDialog());
			return ret;
		}

		QLoadingDialog::QLoadingDialog(void)
			: QDialog(nullptr)
		{
			this->setWindowFlags(windowFlags() | Qt::FramelessWindowHint);
			this->setAutoFillBackground(true);
			auto palette = this->palette();
			palette.setColor(QPalette::Window, QColor(118, 118, 118, 180));
			this->setPalette(palette);

			auto layout = new QGridLayout();
			m_label = new QLabel(nullptr);
			layout->addWidget(m_label, 1, 1);
			layout->setRowStretch(0, 1);
			layout->setRowStretch(2, 1);
			layout->setColumnStretch(0, 1);
			layout->setColumnStretch(2, 1);
			this->setLayout(layout);
		}

		void QLoadingDialog::setUrl(const QString & url)
		{
			m_movie.reset(new QMovie(url));
			m_label->setMovie(m_movie.get());
		}

		void QLoadingDialog::setBackgroundColor(const QColor color)
		{
			auto palette = this->palette();
			palette.setColor(QPalette::Window, color);
			this->setPalette(palette);
		}

		int QLoadingDialog::exec(void)
		{
			if (m_movie == nullptr)
			{
				return 0;
			}

			if (m_location != nullptr)
			{
				this->setGeometry(m_location->x(), m_location->y(), m_location->width(), m_location->height());
			}

			m_movie->start();
			return QDialog::exec();
		}

		void QLoadingDialog::closeEvent(QCloseEvent *)
		{
			if (m_movie != nullptr)
			{
				m_movie->stop();
			}
		}
	};
};
