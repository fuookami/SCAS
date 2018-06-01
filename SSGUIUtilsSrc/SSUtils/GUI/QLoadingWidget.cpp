#include "QLoadingWidget.h"
#include <QtWidgets/QGridLayout>

namespace SSUtils
{
	namespace GUI
	{
		QLoadingWidget * QLoadingWidget::getInstance(void)
		{
			static QLoadingWidget *ret(new QLoadingWidget());
			return ret;
		}

		QLoadingWidget::QLoadingWidget(void)
			: QWidget(nullptr)
		{
			this->setAutoFillBackground(true);
			auto palette = this->palette();
			palette.setColor(QPalette::Window, QColor(118, 118, 118, 180));
			this->setPalette(palette);

			auto layout = new QGridLayout();
			m_label = new QLabel(nullptr);
			m_label->setStyleSheet(QString("border: 0;"));
			layout->addWidget(m_label, 1, 1);
			layout->setRowStretch(0, 1);
			layout->setRowStretch(2, 1);
			layout->setColumnStretch(0, 1);
			layout->setColumnStretch(2, 1);
			this->setLayout(layout);
		}

		void QLoadingWidget::setUrl(const QString & url)
		{
			m_movie.reset(new QMovie(url));
			m_label->setMovie(m_movie.get());
		}

		void QLoadingWidget::setBackgroundColor(const QColor color)
		{
			auto palette = this->palette();
			palette.setColor(QPalette::Window, color);
			this->setPalette(palette);
		}

		void QLoadingWidget::show(void)
		{
			if (this->parent() == nullptr
				|| m_movie == nullptr)
			{
				return;
			}

			this->move(0, 0);
			this->resize(dynamic_cast<QWidget *>(parent())->size());
			
			m_movie->start();
			QWidget::show();
		}

		void QLoadingWidget::hideEvent(QHideEvent * event)
		{
			if (m_movie != nullptr)
			{
				m_movie->stop();
			}
		}
	};
};
