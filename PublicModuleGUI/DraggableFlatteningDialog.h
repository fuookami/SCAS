#pragma once

#include <QtWidgets/QDialog>
#include <QtGui/QMouseEvent>

class DraggableFlatteningDialog : public QDialog
{
	Q_OBJECT;

public:
	enum Result
	{
		Rejected,
		Accepted
	};

	explicit DraggableFlatteningDialog(QWidget *parent = nullptr);
	DraggableFlatteningDialog(const DraggableFlatteningDialog &ano) = delete;
	DraggableFlatteningDialog(const DraggableFlatteningDialog &&ano) = delete;
	DraggableFlatteningDialog &operator=(const DraggableFlatteningDialog &rhs) = delete;
	DraggableFlatteningDialog &operator=(const DraggableFlatteningDialog &&rhs) = delete;
	virtual ~DraggableFlatteningDialog(void);

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
