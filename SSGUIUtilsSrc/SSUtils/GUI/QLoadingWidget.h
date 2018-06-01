#pragma once

#include "SSUtils/Global.h"
#include <QtWidgets/QWidget>
#include <QtWidgets/QLabel>
#include <QtGui/QMovie>
#include <memory>

namespace SSUtils
{
	namespace GUI
	{
		class QLoadingWidget : public QWidget
		{
		public:
			static QLoadingWidget *getInstance(void);

		private:
			explicit QLoadingWidget(void);
		public:
			QLoadingWidget(const QLoadingWidget &ano) = delete;
			QLoadingWidget(QLoadingWidget &&ano) = delete;
			QLoadingWidget &operator=(const QLoadingWidget &rhs) = delete;
			QLoadingWidget &operator=(QLoadingWidget &&rhs) = delete;
			~QLoadingWidget(void) = default;

			void setUrl(const QString &url);
			void setBackgroundColor(const QColor color);

			void show(void);

		protected:
			void hideEvent(QHideEvent *event);

		private:
			QLabel * m_label;
			std::shared_ptr<QMovie> m_movie;
		};
	};
};
