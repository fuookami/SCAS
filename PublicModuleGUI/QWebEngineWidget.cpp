#include "QWebEngineWidget.h"

QWebEngineWidget::QWebEngineWidget(QWidget * parent)
	: QWidget(parent), m_view(new QWebEngineView(this)), m_channal(new QWebChannel(this))
{
	m_view->setGeometry(this->geometry());
	m_view->page()->setWebChannel(m_channal);
}

void QWebEngineWidget::resizeEvent(QResizeEvent * e)
{
	m_view->setGeometry(this->geometry());
}
