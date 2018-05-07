#include "QWebEngineWidget.h"

namespace SSUtils
{
	namespace GUI
	{
		QWebEngineWidget::QWebEngineWidget(QWidget * parent)
			: QWidget(parent), m_view(new QWebEngineView(this)), m_channal(new QWebChannel(this))
		{
			m_view->move(0, 0);
			m_view->resize(this->size());
			m_view->page()->setWebChannel(m_channal);
		}

		void QWebEngineWidget::resizeEvent(QResizeEvent * e)
		{
			qDebug() << m_view->geometry();
			m_view->resize(this->size());
		}
	};
};
