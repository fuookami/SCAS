#include "DraggableFlatteningWidget.h"

DraggableFlatteningWidget::DraggableFlatteningWidget(QWidget * parent)
	: QWidget(parent)
{
	setWindowFlags(windowFlags() | Qt::FramelessWindowHint);
}

DraggableFlatteningWidget::~DraggableFlatteningWidget(void)
{
}

void DraggableFlatteningWidget::mouseMoveEvent(QMouseEvent * event)
{
	if (isMousePressed == true)
	{
		QPoint movePot = event->globalPos() - mousePosition;
		move(movePot);
	}
}

void DraggableFlatteningWidget::mousePressEvent(QMouseEvent * event)
{
	mousePosition = event->pos();
	if (mousePosition.x() <= MinPositionX()
		|| mousePosition.x() >= MaxPositionX()
		|| mousePosition.y() <= MinPositionY()
		|| mousePosition.y() >= MaxPositionY())
	{
		return;
	}

	isMousePressed = true;
}

void DraggableFlatteningWidget::mouseReleaseEvent(QMouseEvent * event)
{
	isMousePressed = false;
}
