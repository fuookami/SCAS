#include "IWebEngineDialogWidget.h"
#include <QtCore/QDebug>
#include "QWebEngineWidget.h"

namespace SSUtils
{
	namespace GUI
	{
		IWebEngineDialogWidget::IWebEngineDialogWidget(const QString &guiEntrance, QWidget * parent)
			: QWebEngineWidget(parent), m_guiEntrance(guiEntrance)
		{
			setAttribute(Qt::WA_ShowModal, true);
		}

		void IWebEngineDialogWidget::showEvent(QShowEvent * event)
		{
			registerContents();
			load(m_guiEntrance);

			connect(m_view, &QWebEngineView::loadFinished, this, &IWebEngineDialogWidget::onLoadFinished);
		}

		void IWebEngineDialogWidget::closeEvent(QCloseEvent * event)
		{
			this->deleteLater();
		}

		void IWebEngineDialogWidget::onLoadFinished(bool ok)
		{
			if (!ok)
			{
				qDebug() << QString::fromLocal8Bit("无法打开GUI入口: ") << m_guiEntrance;
			}
			else
			{
				initGUI();
			}
		}
	};
};
