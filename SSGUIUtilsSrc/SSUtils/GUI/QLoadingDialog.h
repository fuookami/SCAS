#pragma once

#include "SSUtils/Global.h"
#include <QtWidgets/QDialog>
#include <QtWidgets/QLabel>
#include <QtGui/QMovie>
#include <memory>

namespace SSUtils
{
	namespace GUI
	{
		class QLoadingDialog : public QDialog
		{
		public:
			static std::shared_ptr<QLoadingDialog> getInstance(void);

		private:
			explicit QLoadingDialog(void);
		public:
			QLoadingDialog(const QLoadingDialog &ano) = delete;
			QLoadingDialog(QLoadingDialog &&ano) = delete;
			QLoadingDialog &operator=(const QLoadingDialog &rhs) = delete;
			QLoadingDialog &operator=(QLoadingDialog &&rhs) = delete;
			~QLoadingDialog(void) = default;
			
			void setUrl(const QString &url);
			void setBackgroundColor(const QColor color);
			inline void setLocation(const QWidget *location) { m_location = location; }

			int exec(void);

		protected:
			void closeEvent(QCloseEvent *);

		private:
			QLabel *m_label;
			std::shared_ptr<QMovie> m_movie;
			const QWidget *m_location;
		};
	};
};
