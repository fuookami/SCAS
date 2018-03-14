#include "DraggableFlatteningDialog.h"

DraggableFlatteningDialog::DraggableFlatteningDialog(QWidget * parent)
	: QDialog(parent)
{
	setWindowFlags(windowFlags() | Qt::FramelessWindowHint);
	this->setWindowModality(Qt::WindowModal);
}

DraggableFlatteningDialog::~DraggableFlatteningDialog(void)
{
}

void DraggableFlatteningDialog::mouseMoveEvent(QMouseEvent * event)
{
	if (isMousePressed == true)
	{
		QPoint movePot = event->globalPos() - mousePosition;
		move(movePot);
	}
}

void DraggableFlatteningDialog::mousePressEvent(QMouseEvent * event)
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

void DraggableFlatteningDialog::mouseReleaseEvent(QMouseEvent * event)
{
	isMousePressed = false;
}
