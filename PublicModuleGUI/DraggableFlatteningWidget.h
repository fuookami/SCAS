#pragma once

#include <QtWidgets/QWidget>
#include <QtGui/QMouseEvent>

class DraggableFlatteningWidget : public QWidget
{
	Q_OBJECT;

public:
	explicit DraggableFlatteningWidget(QWidget *parent = nullptr);
	DraggableFlatteningWidget(const DraggableFlatteningWidget &ano) = delete;
	DraggableFlatteningWidget(const DraggableFlatteningWidget &&ano) = delete;
	DraggableFlatteningWidget &operator=(const DraggableFlatteningWidget &rhs) = delete;
	DraggableFlatteningWidget &operator=(const DraggableFlatteningWidget &&rhs) = delete;
	virtual ~DraggableFlatteningWidget(void);

	virtual const unsigned short MinPositionX(void) const = 0;
	virtual const unsigned short MaxPositionX(void) const = 0;
	virtual const unsigned short MinPositionY(void) const = 0;
	virtual const unsigned short MaxPositionY(void) const = 0;

protected:
	void mouseMoveEvent(QMouseEvent * event);
	void mousePressEvent(QMouseEvent * event);
	void mouseReleaseEvent(QMouseEvent * event);

private:
	QPoint mousePosition;
	bool isMousePressed;
};
